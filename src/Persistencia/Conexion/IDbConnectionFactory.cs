namespace Persistencia.Conexion;

using System.Data;

public interface IDbConnectionFactory
{
    IDbConnection CrearConexionDesarrollo();
    IDbConnection CrearConexionAdmin();
}
