namespace Persistencia.Repositorios;

using System.Data;
using Aplicacion.Dominio;
using Dapper;
using Persistencia.Conexion;

public class PersonajeRepository
{
    private readonly IDbConnectionFactory _factory;

    public PersonajeRepository(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public Personaje ObtenerPorId(int id)
    {
        using IDbConnection db = _factory.CrearConexionDesarrollo();
        return db.QueryFirst<Guerrero>("SELECT * FROM Personaje WHERE Id = @Id", new { Id = id });
    }

    public void Guardar(Personaje p)
    {
        using IDbConnection db = _factory.CrearConexionDesarrollo();
        db.Execute("INSERT INTO Personaje (Nombre, Tipo, Vida, Ataque, Defensa) VALUES (@Nombre, 'Guerrero', @Vida, @Ataque, @Defensa)", p);
    }
}
