using Persistencia.Entidades;

namespace Persistencia.Repositorios.Interfaces;

public interface IBatallaApiRepository : IBatallaRepository
{
    bool TieneTurnos(int batallaId);
    int ContarTurnos(int batallaId);
    bool ActualizarParticipantes(int batallaId, int personaje1Id, int personaje2Id);
    void Eliminar(int batallaId);
    Turno? ObtenerTurno(int batallaId, int turnoId);
    bool ActualizarNota(int batallaId, int turnoId, string nota);
    int RegistrarTurnoTransaccional(
        Turno turno,
        int vidaPersonaje1Despues,
        int vidaPersonaje2Despues,
        string estadoDespues,
        int? ganadorDespues);
    // Devuelve 1 si borró, 0 si el turno no existe y -1 si no es el último.
    int EliminarUltimoTurno(int batallaId, int turnoId);
}
