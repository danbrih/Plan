namespace Persistencia.Conexion;

using System.Data;
using MySqlConnector;

public class MySqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _strDev = "Server=localhost;Database=juego_db;Uid=desarrollo;Pwd=dev123;";
    private readonly string _strAdmin = "Server=localhost;Uid=administrador;Pwd=admin123;";

    public IDbConnection CrearConexionDesarrollo() => new MySqlConnection(_strDev);
    public IDbConnection CrearConexionAdmin() => new MySqlConnection(_strAdmin);
}
