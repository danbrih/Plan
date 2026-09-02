using Aplicacion.Interfaces;
using Persistencia;

namespace Aplicacion.Servicios;

public class ServicioA : IServicioA
{
    private readonly IDbConnectionFactory _dbFactory;

    public ServicioA(IDbConnectionFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public void Ejecutar()
    {
        Console.WriteLine("Ejecutando Servicio A...");
        Console.WriteLine($"Cadena de conexión: {_dbFactory.GetConnectionString()}");
    }
}
