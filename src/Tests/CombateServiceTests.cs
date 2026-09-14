namespace Tests;

using Aplicacion.Dominio;
using Aplicacion.Servicios;
using Xunit;

public class CombateServiceTests
{
    [Fact]
    public void GuerreroAplicaFuerzaAdicionalAlAtacar()
    {
        var guerrero = new Guerrero { Nombre = "Conan", Ataque = 10, Defensa = 2, Vida = 100 };
        var mago = new Mago { Nombre = "Gandalf", Ataque = 5, Defensa = 1, Vida = 50 };
        var service = new CombateService();

        int dano = service.EjecutarTurno(guerrero, mago);

        Assert.Equal(15, dano);
        Assert.Equal(36, mago.Vida);
    }

    [Fact]
    public void MagoConsumeManaAlAtacar()
    {
        var mago = new Mago { Nombre = "Gandalf", Ataque = 10, Mana = 10, Vida = 50 };
        var guerrero = new Guerrero { Nombre = "Conan", Ataque = 10, Defensa = 0, Vida = 100 };
        var service = new CombateService();

        service.EjecutarTurno(mago, guerrero);

        Assert.Equal(5, mago.Mana);
    }

    [Fact]
    public void AsesinoCalculaDanioPolimorfico()
    {
        var asesino = new Asesino { Nombre = "Ezio", Ataque = 15, Defensa = 0, Vida = 60 };
        var guerrero = new Guerrero { Nombre = "Conan", Ataque = 10, Defensa = 5, Vida = 100 };
        var service = new CombateService();

        int dano = service.EjecutarTurno(asesino, guerrero);

        Assert.Equal(30, dano);
        Assert.Equal(75, guerrero.Vida);
    }
}
