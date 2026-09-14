namespace Aplicacion.Servicios;

using Aplicacion.Dominio;

public class CombateService
{
    public int EjecutarTurno(Personaje atacante, Personaje defensor, bool usarHabilidad = false)
    {
        if (!atacante.EstaVivo() || !defensor.EstaVivo()) return 0;

        int danoBase = usarHabilidad ? atacante.UsarHabilidad() : atacante.CalcularDano();
        defensor.RecibirDano(danoBase);
        return danoBase;
    }

    public Personaje SimularCombateCompleto(Personaje p1, Personaje p2)
    {
        int turno = 1;
        while (p1.EstaVivo() && p2.EstaVivo())
        {
            if (turno % 2 != 0)
                EjecutarTurno(p1, p2, turno % 3 == 0);
            else
                EjecutarTurno(p2, p1, turno % 3 == 0);
            turno++;
        }
        return p1.EstaVivo() ? p1 : p2;
    }
}
