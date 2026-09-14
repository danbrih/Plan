namespace Persistencia.Repositorios;

using System.Data;
using Aplicacion.Dominio;
using Aplicacion.Interfaces;
using Dapper;
using Persistencia.Conexion;

public class BatallaRepository : IBatallaRepository
{
    private readonly IDbConnectionFactory _factory;

    public BatallaRepository(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public int Crear(Batalla batalla)
    {
        using IDbConnection db = _factory.CrearConexionDesarrollo();
        string sql = "INSERT INTO Batalla (Fecha, GanadorId) VALUES (@Fecha, @GanadorId); SELECT LAST_INSERT_ID();";
        return db.ExecuteScalar<int>(sql, batalla);
    }

    public Batalla? ObtenerPorId(int id)
    {
        using IDbConnection db = _factory.CrearConexionDesarrollo();
        return db.QueryFirstOrDefault<Batalla>("SELECT * FROM Batalla WHERE Id = @Id", new { Id = id });
    }

    public IEnumerable<Batalla> ObtenerTodas()
    {
        using IDbConnection db = _factory.CrearConexionDesarrollo();
        return db.Query<Batalla>("SELECT * FROM Batalla");
    }

    public void FinalizarBatalla(int batallaId, int ganadorId)
    {
        using IDbConnection db = _factory.CrearConexionDesarrollo();
        db.Execute("UPDATE Batalla SET GanadorId = @GanadorId WHERE Id = @BatallaId", new { GanadorId = ganadorId, BatallaId = batallaId });
    }
}
