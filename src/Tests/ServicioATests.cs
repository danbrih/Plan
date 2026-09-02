using Xunit;
using Aplicacion.Servicios;
using Persistencia;

namespace Tests;

public class ServicioATests
{
    [Fact]
    public void Test_ServicioA_EjecutaCorrectamente()
    {
        var factory = new MySqlConnectionFactory("Server=localhost;");
        var servicio = new ServicioA(factory);

        var exception = Record.Exception(() => servicio.Ejecutar());
        Assert.Null(exception);
    }
}
