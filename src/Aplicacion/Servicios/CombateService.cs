namespace Aplicacion.Servicios;

using Persistencia.Entidades;

public class CombateService
{
    public int ObtenerAtacanteSiguiente(int numeroTurno, int personaje1Id, int personaje2Id)
    {
        if (numeroTurno <= 0 || personaje1Id <= 0 || personaje2Id <= 0 || personaje1Id == personaje2Id)
            throw new ArgumentException("El turno y los IDs de participantes deben ser válidos.");
        return numeroTurno % 2 == 1 ? personaje1Id : personaje2Id;
    }

    public int EjecutarTurno(Personaje atacante, Personaje defensor)
    {
        return atacante.Atacar(defensor);
    }

    public int EjecutarTurno(Personaje atacante, Personaje defensor, bool usarHabilidad)
    {
        return atacante.Atacar(defensor, usarHabilidad);
    }

    public Personaje EjecutarCombate(Personaje primero, Personaje segundo)
    {
        if (!primero.EstaVivo())
            throw new ArgumentException("El primer personaje está derrotado.");

        if (!segundo.EstaVivo())
            throw new ArgumentException("El segundo personaje está derrotado.");

        while (primero.EstaVivo() && segundo.EstaVivo())
        {
            EjecutarTurno(primero, segundo);

            if (segundo.EstaVivo())
                EjecutarTurno(segundo, primero);
        }

        if (primero.EstaVivo())
            return primero;

        return segundo;
    }
}
