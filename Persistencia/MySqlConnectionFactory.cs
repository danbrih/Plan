namespace Persistencia;

using System.Data;
using MySqlConnector;

public class MySqlConnectionFactory : IDbConnectionFactory
{
    private readonly string cadenaAdministrador =
        "Server=localhost;Database=mysql;Uid=administrador;Pwd=admin123;";

    private readonly string cadenaDesarrollo =
        "Server=localhost;Database=juego_db;Uid=desarrollo;Pwd=dev123;";

    public IDbConnection CrearConexionAdministrador()
    {
        return new MySqlConnection(cadenaAdministrador);
    }

    public IDbConnection CrearConexionDesarrollo()
    {
        return new MySqlConnection(cadenaDesarrollo);
    }
}
