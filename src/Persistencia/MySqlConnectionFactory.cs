using System.Data;
using MySqlConnector;

namespace Persistencia;

public sealed class MySqlConnectionFactory : IDbConnectionFactory
{
    private static string ObtenerCadena(string variable)
    {
        string? valor = Environment.GetEnvironmentVariable(variable);
        if (string.IsNullOrWhiteSpace(valor))
            throw new InvalidOperationException(
                $"Falta definir la variable de entorno {variable} con la cadena de conexión de MySQL.");
        return valor;
    }

    public IDbConnection CrearConexionAdministrador() =>
        new MySqlConnection(ObtenerCadena("ADMIN_DB_CONNECTION"));

    public IDbConnection CrearConexionInvitado() =>
        new MySqlConnection(ObtenerCadena("GUEST_DB_CONNECTION"));
}
