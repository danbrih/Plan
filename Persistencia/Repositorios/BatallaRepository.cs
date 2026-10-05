namespace Persistencia.Repositorios;

using System.Data;
using Dapper;
using Persistencia;
using Persistencia.Entidades;
using Persistencia.Repositorios.Interfaces;

public class BatallaRepository : IBatallaRepository
{
    private readonly IDbConnectionFactory factory;

    public BatallaRepository(IDbConnectionFactory factory)
    {
        this.factory = factory;
    }

    public int Crear(Batalla batalla)
    {
        using IDbConnection db = factory.CrearConexionDesarrollo();

        var parametros = new
        {
            p_Personaje1Id = batalla.Personaje1Id,
            p_Personaje2Id = batalla.Personaje2Id
        };

        return db.ExecuteScalar<int>(
            "RegistrarBatalla",
            parametros,
            commandType: CommandType.StoredProcedure);
    }

    public Batalla? ObtenerPorId(int id)
    {
        using IDbConnection db = factory.CrearConexionDesarrollo();

        string sql = "SELECT Id, Fecha, Personaje1Id, Personaje2Id, GanadorId, Estado " +
                     "FROM Batalla WHERE Id = @Id";

        return db.QueryFirstOrDefault<Batalla>(sql, new { Id = id });
    }

    public IEnumerable<Batalla> ObtenerTodas()
    {
        using IDbConnection db = factory.CrearConexionDesarrollo();

        string sql = "SELECT Id, Fecha, Personaje1Id, Personaje2Id, GanadorId, Estado " +
                     "FROM Batalla ORDER BY Fecha DESC";

        return db.Query<Batalla>(sql);
    }

    public IEnumerable<Turno> ObtenerHistorial(int batallaId)
    {
        using IDbConnection db = factory.CrearConexionDesarrollo();

        string sql = "SELECT Id, BatallaId, AtacanteId, DefensorId, Danio, TipoAccion, Numero, Fecha " +
                     "FROM Turno WHERE BatallaId = @BatallaId ORDER BY Numero";

        return db.Query<Turno>(sql, new { BatallaId = batallaId });
    }

    public void RegistrarTurno(Turno turno)
    {
        using IDbConnection db = factory.CrearConexionDesarrollo();

        db.Execute(
            "RegistrarTurno",
            new
            {
                p_BatallaId = turno.BatallaId,
                p_AtacanteId = turno.AtacanteId,
                p_DefensorId = turno.DefensorId,
                p_Danio = turno.Danio,
                p_TipoAccion = turno.TipoAccion,
                p_Numero = turno.Numero
            },
            commandType: CommandType.StoredProcedure);
    }

    public void FinalizarBatalla(int batallaId, int ganadorId)
    {
        using IDbConnection db = factory.CrearConexionDesarrollo();

        db.Execute(
            "FinalizarBatalla",
            new
            {
                p_BatallaId = batallaId,
                p_GanadorId = ganadorId
            },
            commandType: CommandType.StoredProcedure);
    }

    public IEnumerable<EstadisticaTipo> ObtenerResumenPorTipo(DateTime inicio, DateTime fin)
    {
        return EjecutarEstadistica<EstadisticaTipo>(
            "ObtenerResumenBatallasPorTipo", inicio, fin);
    }

    public IEnumerable<EstadisticaTipo> ObtenerVictoriasDerrotasPorTipo(DateTime inicio, DateTime fin)
    {
        return EjecutarEstadistica<EstadisticaTipo>(
            "ObtenerVictoriasDerrotasPorTipo", inicio, fin);
    }

    public IEnumerable<RankingPersonaje> ObtenerRanking(DateTime inicio, DateTime fin)
    {
        return EjecutarEstadistica<RankingPersonaje>(
            "ObtenerRankingPersonajes", inicio, fin);
    }

    public IEnumerable<EstadisticaDanio> ObtenerDanioPromedio(DateTime inicio, DateTime fin)
    {
        return EjecutarEstadistica<EstadisticaDanio>(
            "ObtenerDanioPromedio", inicio, fin);
    }

    public IEnumerable<EstadisticaDanio> ObtenerDanioTotal(DateTime inicio, DateTime fin)
    {
        return EjecutarEstadistica<EstadisticaDanio>(
            "ObtenerDanioTotalPorPersonaje", inicio, fin);
    }

    public IEnumerable<EstadisticaPeriodo> ObtenerBatallasPorPeriodo(DateTime inicio, DateTime fin)
    {
        return EjecutarEstadistica<EstadisticaPeriodo>(
            "ObtenerBatallasPorPeriodo", inicio, fin);
    }

    private IEnumerable<T> EjecutarEstadistica<T>(
        string procedimiento,
        DateTime inicio,
        DateTime fin)
    {
        using IDbConnection db = factory.CrearConexionDesarrollo();

        return db.Query<T>(
            procedimiento,
            new
            {
                p_FechaInicio = inicio,
                p_FechaFin = fin
            },
            commandType: CommandType.StoredProcedure);
    }
}
