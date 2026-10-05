namespace Persistencia;

using System.Data;

public interface IDbConnectionFactory
{
    IDbConnection CrearConexionAdministrador();
    IDbConnection CrearConexionDesarrollo();
}
