namespace Aplicacion.Servicios;

using Aplicacion.Dominio;

public class CombateService
{
    public int EjecutarTurno(Personaje atacante, Personaje defensor)
    {
        int dano = atacante.CalcularDano();
        defensor.RecibirDano(dano);
        return dano;
    }
}
