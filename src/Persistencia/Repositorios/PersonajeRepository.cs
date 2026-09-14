namespace Persistencia.Repositorios;

using System.Data;
using Aplicacion.Dominio;
using Aplicacion.Interfaces;
using Dapper;
using Persistencia.Conexion;

public class PersonajeRepository : IPersonajeRepository
{
    private readonly IDbConnectionFactory _factory;

    public PersonajeRepository(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public Personaje? ObtenerPorId(int id)
    {
        using IDbConnection db = _factory.CrearConexionDesarrollo();
        var row = db.QueryFirstOrDefault("SELECT * FROM Personaje WHERE Id = @Id", new { Id = id });
        if (row == null) return null;

        string tipo = row.Tipo;
        Personaje p = tipo switch
        {
            "Guerrero" => new Guerrero(),
            "Mago" => new Mago(),
            "Arquero" => new Arquero(),
            "Asesino" => new Asesino(),
            _ => new Guerrero()
        };

        p.Id = (int)row.Id;
        p.Nombre = row.Nombre;
        p.Vida = (int)row.Vida;
        p.VidaMaxima = (int)row.Vida;
        p.Ataque = (int)row.Ataque;
        p.Defensa = (int)row.Defensa;
        return p;
    }

    public IEnumerable<Personaje> ObtenerTodos()
    {
        using IDbConnection db = _factory.CrearConexionDesarrollo();
        return db.Query<Guerrero>("SELECT * FROM Personaje");
    }

    public int Guardar(Personaje p)
    {
        using IDbConnection db = _factory.CrearConexionDesarrollo();
        string sql = "INSERT INTO Personaje (Nombre, Tipo, Vida, Ataque, Defensa) VALUES (@Nombre, @Tipo, @Vida, @Ataque, @Defensa); SELECT LAST_INSERT_ID();";
        return db.ExecuteScalar<int>(sql, new { p.Nombre, Tipo = p.Tipo, p.Vida, p.Ataque, p.Defensa });
    }
}
