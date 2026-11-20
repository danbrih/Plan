using System.Data;
using Dapper;
using Persistencia.Entidades;
using Persistencia.Repositorios.Interfaces;

namespace Persistencia.Repositorios;

public sealed class BatallaRepository : IBatallaApiRepository
{
    private readonly IDbConnectionFactory factory;
    public BatallaRepository(IDbConnectionFactory factory) => this.factory = factory;

    public int Crear(Batalla batalla)
    {
        using IDbConnection db = factory.CrearConexionAdministrador();
        const string sql = @"INSERT INTO Batalla (Fecha, Personaje1Id, Personaje2Id, GanadorId, Estado)
VALUES (@Fecha, @Personaje1Id, @Personaje2Id, NULL, 'EnProgreso');
SELECT LAST_INSERT_ID();";
        return db.ExecuteScalar<int>(sql, batalla);
    }

    public Batalla? ObtenerPorId(int id)
    {
        using IDbConnection db = factory.CrearConexionInvitado();
        const string sql = @"SELECT Id, Fecha, Personaje1Id, Personaje2Id, GanadorId, Estado
FROM Batalla WHERE Id = @Id;";
        return db.QueryFirstOrDefault<Batalla>(sql, new { Id = id });
    }

    public IEnumerable<Batalla> ObtenerTodas()
    {
        using IDbConnection db = factory.CrearConexionInvitado();
        const string sql = @"SELECT Id, Fecha, Personaje1Id, Personaje2Id, GanadorId, Estado
FROM Batalla ORDER BY Fecha DESC, Id DESC;";
        return db.Query<Batalla>(sql).ToList();
    }

    public IEnumerable<Turno> ObtenerHistorial(int batallaId)
    {
        using IDbConnection db = factory.CrearConexionInvitado();
        const string sql = @"SELECT Id, BatallaId, AtacanteId, DefensorId, Danio, TipoAccion,
Numero, Fecha, Nota, VidaPersonaje1Antes, VidaPersonaje2Antes, EstadoAntes, GanadorAntes
FROM Turno WHERE BatallaId = @BatallaId ORDER BY Numero;";
        return db.Query<Turno>(sql, new { BatallaId = batallaId }).ToList();
    }

    public void RegistrarTurno(Turno turno)
    {
        using IDbConnection db = factory.CrearConexionAdministrador();
        const string sql = @"INSERT INTO Turno
(BatallaId, AtacanteId, DefensorId, Danio, TipoAccion, Numero, Fecha, Nota,
 VidaPersonaje1Antes, VidaPersonaje2Antes, EstadoAntes, GanadorAntes)
VALUES (@BatallaId, @AtacanteId, @DefensorId, @Danio, @TipoAccion, @Numero, @Fecha, @Nota,
 @VidaPersonaje1Antes, @VidaPersonaje2Antes, @EstadoAntes, @GanadorAntes);";
        db.Execute(sql, turno);
    }

    public void FinalizarBatalla(int batallaId, int ganadorId)
    {
        using IDbConnection db = factory.CrearConexionAdministrador();
        db.Execute("UPDATE Batalla SET GanadorId = @GanadorId, Estado = 'Finalizada' WHERE Id = @Id;",
            new { Id = batallaId, GanadorId = ganadorId });
    }

    public bool TieneTurnos(int batallaId) => ContarTurnos(batallaId) > 0;

    public int ContarTurnos(int batallaId)
    {
        using IDbConnection db = factory.CrearConexionInvitado();
        return db.ExecuteScalar<int>("SELECT COUNT(*) FROM Turno WHERE BatallaId = @Id;", new { Id = batallaId });
    }

    public bool ActualizarParticipantes(int batallaId, int personaje1Id, int personaje2Id)
    {
        using IDbConnection db = factory.CrearConexionAdministrador();
        const string sql = @"UPDATE Batalla
SET Personaje1Id = @P1, Personaje2Id = @P2
WHERE Id = @Id AND NOT EXISTS (SELECT 1 FROM Turno WHERE BatallaId = @Id);";
        return db.Execute(sql, new { Id = batallaId, P1 = personaje1Id, P2 = personaje2Id }) == 1;
    }

    public void Eliminar(int batallaId)
    {
        using IDbConnection db = factory.CrearConexionAdministrador();
        db.Execute("DELETE FROM Batalla WHERE Id = @Id AND NOT EXISTS (SELECT 1 FROM Turno WHERE BatallaId = @Id);",
            new { Id = batallaId });
    }

    public Turno? ObtenerTurno(int batallaId, int turnoId)
    {
        using IDbConnection db = factory.CrearConexionInvitado();
        const string sql = @"SELECT Id, BatallaId, AtacanteId, DefensorId, Danio, TipoAccion,
Numero, Fecha, Nota, VidaPersonaje1Antes, VidaPersonaje2Antes, EstadoAntes, GanadorAntes
FROM Turno WHERE BatallaId = @BatallaId AND Id = @TurnoId;";
        return db.QueryFirstOrDefault<Turno>(sql, new { BatallaId = batallaId, TurnoId = turnoId });
    }

    public bool ActualizarNota(int batallaId, int turnoId, string nota)
    {
        using IDbConnection db = factory.CrearConexionAdministrador();
        return db.Execute("UPDATE Turno SET Nota = @Nota WHERE BatallaId = @BatallaId AND Id = @TurnoId;",
            new { BatallaId = batallaId, TurnoId = turnoId, Nota = nota }) == 1;
    }

    public int RegistrarTurnoTransaccional(
        Turno turno, int vidaPersonaje1Despues, int vidaPersonaje2Despues,
        string estadoDespues, int? ganadorDespues)
    {
        using IDbConnection db = factory.CrearConexionAdministrador();
        db.Open();
        using IDbTransaction tx = db.BeginTransaction();

        try
        {
            var batalla = db.QuerySingleOrDefault<BatallaBloqueada>(
                "SELECT Id, Personaje1Id, Personaje2Id, Estado FROM Batalla WHERE Id = @Id FOR UPDATE;",
                new { Id = turno.BatallaId }, tx);
            if (batalla is null) throw new KeyNotFoundException("El combate no existe.");
            if (batalla.Estado != "EnProgreso") throw new InvalidOperationException("El combate ya finalizó.");

            int cantidad = db.ExecuteScalar<int>("SELECT COUNT(*) FROM Turno WHERE BatallaId = @Id;",
                new { Id = turno.BatallaId }, tx);
            int atacanteEsperado = cantidad % 2 == 0 ? batalla.Personaje1Id : batalla.Personaje2Id;
            if (turno.Numero != cantidad + 1 || turno.AtacanteId != atacanteEsperado)
                throw new InvalidOperationException("El atacante está fuera de turno.");

            var vidas = db.Query<VidaFila>(
                "SELECT Id, Vida FROM Personaje WHERE Id IN (@P1, @P2) FOR UPDATE;",
                new { P1 = batalla.Personaje1Id, P2 = batalla.Personaje2Id }, tx).ToDictionary(x => x.Id, x => x.Vida);
            if (vidas.Count != 2) throw new KeyNotFoundException("Falta uno de los personajes del combate.");
            if (vidas[batalla.Personaje1Id] != turno.VidaPersonaje1Antes ||
                vidas[batalla.Personaje2Id] != turno.VidaPersonaje2Antes)
                throw new InvalidOperationException("El estado del combate cambió; volvé a consultar antes de ejecutar el turno.");

            const string insertar = @"INSERT INTO Turno
(BatallaId, AtacanteId, DefensorId, Danio, TipoAccion, Numero, Fecha, Nota,
 VidaPersonaje1Antes, VidaPersonaje2Antes, EstadoAntes, GanadorAntes)
VALUES (@BatallaId, @AtacanteId, @DefensorId, @Danio, @TipoAccion, @Numero, @Fecha, @Nota,
 @VidaPersonaje1Antes, @VidaPersonaje2Antes, @EstadoAntes, @GanadorAntes);";
            db.Execute(insertar, turno, tx);
            int id = db.ExecuteScalar<int>("SELECT LAST_INSERT_ID();", transaction: tx);
            db.Execute("UPDATE Personaje SET Vida = @Vida WHERE Id = @Id;",
                new { Id = batalla.Personaje1Id, Vida = vidaPersonaje1Despues }, tx);
            db.Execute("UPDATE Personaje SET Vida = @Vida WHERE Id = @Id;",
                new { Id = batalla.Personaje2Id, Vida = vidaPersonaje2Despues }, tx);
            db.Execute("UPDATE Batalla SET Estado = @Estado, GanadorId = @GanadorId WHERE Id = @Id;",
                new { Id = turno.BatallaId, Estado = estadoDespues, GanadorId = ganadorDespues }, tx);
            tx.Commit();
            return id;
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    public int EliminarUltimoTurno(int batallaId, int turnoId)
    {
        using IDbConnection db = factory.CrearConexionAdministrador();
        db.Open();
        using IDbTransaction tx = db.BeginTransaction();
        try
        {
            var batalla = db.QuerySingleOrDefault<BatallaBloqueada>(
                "SELECT Id, Personaje1Id, Personaje2Id, Estado FROM Batalla WHERE Id = @Id FOR UPDATE;",
                new { Id = batallaId }, tx);
            if (batalla is null) return 0;

            var solicitado = db.QuerySingleOrDefault<Turno>(
                @"SELECT Id, BatallaId, Numero, VidaPersonaje1Antes, VidaPersonaje2Antes,
EstadoAntes, GanadorAntes FROM Turno WHERE BatallaId = @BatallaId AND Id = @TurnoId;",
                new { BatallaId = batallaId, TurnoId = turnoId }, tx);
            if (solicitado is null) return 0;

            var ultimo = db.QuerySingleOrDefault<IdFila>(
                "SELECT Id FROM Turno WHERE BatallaId = @Id ORDER BY Numero DESC LIMIT 1 FOR UPDATE;",
                new { Id = batallaId }, tx);
            if (ultimo is null || ultimo.Id != turnoId) return -1;

            db.Execute("UPDATE Personaje SET Vida = @Vida WHERE Id = @Id;",
                new { Id = batalla.Personaje1Id, Vida = solicitado.VidaPersonaje1Antes }, tx);
            db.Execute("UPDATE Personaje SET Vida = @Vida WHERE Id = @Id;",
                new { Id = batalla.Personaje2Id, Vida = solicitado.VidaPersonaje2Antes }, tx);
            db.Execute("UPDATE Batalla SET Estado = @Estado, GanadorId = @Ganador WHERE Id = @Id;",
                new { Id = batallaId, Estado = solicitado.EstadoAntes, Ganador = solicitado.GanadorAntes }, tx);
            db.Execute("DELETE FROM Turno WHERE BatallaId = @BatallaId AND Id = @TurnoId;",
                new { BatallaId = batallaId, TurnoId = turnoId }, tx);
            tx.Commit();
            return 1;
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    public IEnumerable<EstadisticaTipo> ObtenerResumenPorTipo(DateTime inicio, DateTime fin) =>
        EjecutarEstadistica<EstadisticaTipo>("ObtenerResumenBatallasPorTipo", inicio, fin);
    public IEnumerable<EstadisticaTipo> ObtenerVictoriasDerrotasPorTipo(DateTime inicio, DateTime fin) =>
        EjecutarEstadistica<EstadisticaTipo>("ObtenerVictoriasDerrotasPorTipo", inicio, fin);
    public IEnumerable<RankingPersonaje> ObtenerRanking(DateTime inicio, DateTime fin) =>
        EjecutarEstadistica<RankingPersonaje>("ObtenerRankingPersonajes", inicio, fin);
    public IEnumerable<EstadisticaDanio> ObtenerDanioPromedio(DateTime inicio, DateTime fin) =>
        EjecutarEstadistica<EstadisticaDanio>("ObtenerDanioPromedio", inicio, fin);
    public IEnumerable<EstadisticaDanio> ObtenerDanioTotal(DateTime inicio, DateTime fin) =>
        EjecutarEstadistica<EstadisticaDanio>("ObtenerDanioTotalPorPersonaje", inicio, fin);
    public IEnumerable<EstadisticaPeriodo> ObtenerBatallasPorPeriodo(DateTime inicio, DateTime fin) =>
        EjecutarEstadistica<EstadisticaPeriodo>("ObtenerBatallasPorPeriodo", inicio, fin);

    private IEnumerable<T> EjecutarEstadistica<T>(string procedimiento, DateTime inicio, DateTime fin)
    {
        using IDbConnection db = factory.CrearConexionInvitado();
        return db.Query<T>(procedimiento, new { p_FechaInicio = inicio, p_FechaFin = fin },
            commandType: CommandType.StoredProcedure).ToList();
    }

    private sealed class BatallaBloqueada
    {
        public int Id { get; set; }
        public int Personaje1Id { get; set; }
        public int Personaje2Id { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
    private sealed class VidaFila { public int Id { get; set; } public int Vida { get; set; } }
    private sealed class IdFila { public int Id { get; set; } }
}
