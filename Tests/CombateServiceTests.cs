namespace Tests;

using Aplicacion.Servicios;
using Persistencia.Entidades;
using Xunit;

public class CombateServiceTests
{
    [Fact]
    public void GuerreroHaceDanioCorrecto()
    {
        Guerrero guerrero = new Guerrero
        {
            Nombre = "Conan",
            Vida = 100,
            VidaMaxima = 100,
            Ataque = 10,
            Defensa = 5
        };

        Arquero arquero = new Arquero
        {
            Nombre = "Robin",
            Vida = 50,
            VidaMaxima = 50,
            Ataque = 10,
            Defensa = 0
        };

        CombateService service = new CombateService();

        int dano = service.EjecutarTurno(guerrero, arquero);

        Assert.Equal(15, dano);
        Assert.Equal(35, arquero.Vida);
    }

    [Fact]
    public void DefensaReduceElDanio()
    {
        Guerrero guerrero = new Guerrero
        {
            Vida = 100,
            VidaMaxima = 100,
            Ataque = 10,
            Defensa = 5
        };

        Asesino asesino = new Asesino
        {
            Vida = 50,
            VidaMaxima = 50,
            Ataque = 15,
            Defensa = 2
        };

        CombateService service = new CombateService();

        service.EjecutarTurno(asesino, guerrero);

        Assert.Equal(77, guerrero.Vida);
    }

    [Fact]
    public void MagoUsaManaParaAtacar()
    {
        Mago mago = new Mago
        {
            Vida = 100,
            VidaMaxima = 100,
            Ataque = 10,
            Defensa = 2,
            Mana = 10
        };

        Guerrero guerrero = new Guerrero
        {
            Vida = 100,
            VidaMaxima = 100,
            Ataque = 10,
            Defensa = 0
        };

        CombateService service = new CombateService();

        service.EjecutarTurno(mago, guerrero);

        Assert.Equal(5, mago.Mana);
    }

    [Fact]
    public void HabilidadDelGuerreroHaceDanioDiferente()
    {
        Guerrero guerrero = new Guerrero
        {
            Vida = 100,
            VidaMaxima = 100,
            Ataque = 10,
            Defensa = 0
        };

        Guerrero defensor = new Guerrero
        {
            Vida = 100,
            VidaMaxima = 100,
            Ataque = 10,
            Defensa = 0
        };

        CombateService service = new CombateService();

        int dano = service.EjecutarTurno(guerrero, defensor, true);

        Assert.Equal(25, dano);
    }

    [Fact]
    public void PersonajeDerrotadoNoPuedeAtacar()
    {
        Guerrero guerrero = new Guerrero
        {
            Vida = 0,
            VidaMaxima = 100,
            Ataque = 10,
            Defensa = 0
        };

        Guerrero defensor = new Guerrero
        {
            Vida = 100,
            VidaMaxima = 100,
            Ataque = 10,
            Defensa = 0
        };

        CombateService service = new CombateService();

        Assert.Throws<InvalidOperationException>(
            () => service.EjecutarTurno(guerrero, defensor));
    }

    [Fact]
    public void PolimorfismoPermiteUsarPersonajeSinSaberSuTipo()
    {
        Personaje atacante = new Asesino
        {
            Vida = 100,
            VidaMaxima = 100,
            Ataque = 15,
            Defensa = 0
        };

        Personaje defensor = new Mago
        {
            Vida = 100,
            VidaMaxima = 100,
            Ataque = 10,
            Defensa = 0
        };

        CombateService service = new CombateService();

        int dano = service.EjecutarTurno(atacante, defensor);

        Assert.Equal(30, dano);
    }

    [Fact]
    public void CombateCompletoDevuelveUnGanador()
    {
        Guerrero primero = new Guerrero
        {
            Vida = 100,
            VidaMaxima = 100,
            Ataque = 20,
            Defensa = 0
        };

        Arquero segundo = new Arquero
        {
            Vida = 40,
            VidaMaxima = 40,
            Ataque = 5,
            Defensa = 0
        };

        CombateService service = new CombateService();

        Personaje ganador = service.EjecutarCombate(primero, segundo);

        Assert.True(ganador.EstaVivo());
        Assert.Equal(primero, ganador);
    }

    [Fact]
    public void SobrecargaDeAtacarFunciona()
    {
        Guerrero guerrero = new Guerrero
        {
            Vida = 100,
            VidaMaxima = 100,
            Ataque = 10,
            Defensa = 0
        };

        Guerrero defensor = new Guerrero
        {
            Vida = 100,
            VidaMaxima = 100,
            Ataque = 10,
            Defensa = 0
        };

        int dano1 = guerrero.Atacar(defensor);
        int dano2 = guerrero.Atacar(defensor, false);

        Assert.Equal(15, dano1);
        Assert.Equal(15, dano2);
    }

    [Fact]
    public void SobrecargaDeOperadorAumentaLaVidaSinSuperarLaMaxima()
    {
        Personaje guerrero = new Guerrero
        {
            Vida = 80,
            VidaMaxima = 100,
            Ataque = 10,
            Defensa = 0
        };

        guerrero = guerrero + 30;

        Assert.Equal(100, guerrero.Vida);
    }
}






