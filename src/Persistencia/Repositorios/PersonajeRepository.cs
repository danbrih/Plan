using System.Data;
using Dapper;
using Persistencia.Entidades;
using Persistencia.Repositorios.Interfaces;

namespace Persistencia.Repositorios;

public sealed class PersonajeRepository : IPersonajeApiRepository
{
    private readonly IDbConnectionFactory factory;
    public PersonajeRepository(IDbConnectionFactory factory) => this.factory = factory;

    public int Guardar(Personaje personaje)
    {
        using IDbConnection db = factory.CrearConexionAdministrador();
        const string sql = @"INSERT INTO Personaje (Nombre, Tipo, Vida, VidaMaxima, Ataque, Defensa)
VALUES (@Nombre, @Tipo, @Vida, @VidaMaxima, @Ataque, @Defensa);
SELECT LAST_INSERT_ID();";
        return db.ExecuteScalar<int>(sql, new
        {
            personaje.Nombre,
            Tipo = personaje.Tipo,
            personaje.Vida,
            VidaMaxima = personaje.VidaMaxima > 0 ? personaje.VidaMaxima : personaje.Vida,
            personaje.Ataque,
            personaje.Defensa
        });
    }

    public Personaje? ObtenerPorId(int id)
    {
        using IDbConnection db = factory.CrearConexionInvitado();
        const string sql = @"SELECT Id, Nombre, Tipo, Vida, VidaMaxima, Ataque, Defensa
FROM Personaje WHERE Id = @Id;";
        PersonajeDatos? fila = db.QueryFirstOrDefault<PersonajeDatos>(sql, new { Id = id });
        return fila is null ? null : Convertir(fila);
    }

    public IEnumerable<Personaje> ObtenerTodos()
    {
        using IDbConnection db = factory.CrearConexionInvitado();
        const string sql = @"SELECT Id, Nombre, Tipo, Vida, VidaMaxima, Ataque, Defensa
FROM Personaje ORDER BY Id;";
        return db.Query<PersonajeDatos>(sql).Select(Convertir).ToList();
    }

    public IEnumerable<Batalla> ObtenerCombates(int personajeId)
    {
        using IDbConnection db = factory.CrearConexionInvitado();
        const string sql = @"SELECT Id, Fecha, Personaje1Id, Personaje2Id, GanadorId, Estado
FROM Batalla WHERE Personaje1Id = @Id OR Personaje2Id = @Id ORDER BY Fecha DESC;";
        return db.Query<Batalla>(sql, new { Id = personajeId }).ToList();
    }

    public bool ParticipoEnCombate(int personajeId)
    {
        using IDbConnection db = factory.CrearConexionInvitado();
        const string sql = @"SELECT EXISTS(SELECT 1 FROM Batalla
WHERE Personaje1Id = @Id OR Personaje2Id = @Id);";
        return db.ExecuteScalar<bool>(sql, new { Id = personajeId });
    }

    public bool ActualizarNombre(int id, string nombre)
    {
        using IDbConnection db = factory.CrearConexionAdministrador();
        return db.Execute("UPDATE Personaje SET Nombre = @Nombre WHERE Id = @Id;",
            new { Id = id, Nombre = nombre }) == 1;
    }

    public void Eliminar(int id)
    {
        using IDbConnection db = factory.CrearConexionAdministrador();
        db.Execute("DELETE FROM Personaje WHERE Id = @Id;", new { Id = id });
    }

    private static Personaje Convertir(PersonajeDatos fila)
    {
        Personaje personaje = fila.Tipo.ToLowerInvariant() switch
        {
            "guerrero" => new Guerrero(),
            "mago" => new Mago(),
            "arquero" => new Arquero(),
            "asesino" => new Asesino(),
            _ => throw new DataException($"Tipo de personaje no reconocido: {fila.Tipo}")
        };
        personaje.Id = fila.Id;
        personaje.Nombre = fila.Nombre;
        personaje.Vida = fila.Vida;
        personaje.VidaMaxima = fila.VidaMaxima;
        personaje.Ataque = fila.Ataque;
        personaje.Defensa = fila.Defensa;
        return personaje;
    }

    private sealed class PersonajeDatos
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int Vida { get; set; }
        public int VidaMaxima { get; set; }
        public int Ataque { get; set; }
        public int Defensa { get; set; }
    }
}
