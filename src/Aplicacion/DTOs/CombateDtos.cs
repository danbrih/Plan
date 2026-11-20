namespace Aplicacion.DTOs;

public sealed record CrearPersonajeRequest(string Nombre, string Tipo);
public sealed record ActualizarNombreRequest(string Nombre);
public sealed record CrearCombateRequest(int Personaje1Id, int Personaje2Id);
public sealed record ActualizarParticipantesRequest(int Personaje1Id, int Personaje2Id);
public sealed record EjecutarTurnoRequest(int AtacanteId, string Accion);
public sealed record ActualizarNotaRequest(string Nota);

public sealed record PersonajeResponse(
    int Id, string Nombre, string Tipo, int Vida, int VidaMaxima, int Ataque, int Defensa);

public sealed record CombateResumenResponse(
    int Id, DateTime Fecha,
    int Personaje1Id, string Personaje1,
    int Personaje2Id, string Personaje2,
    string Estado, int? GanadorId);

public sealed record TurnoResponse(
    int Id, int BatallaId, int AtacanteId, int DefensorId,
    int Danio, string TipoAccion, int Numero, DateTime Fecha, string Nota);

public sealed record CombateDetalleResponse(
    int Id, DateTime Fecha, string Estado,
    PersonajeResponse Personaje1, PersonajeResponse Personaje2,
    IEnumerable<TurnoResponse> Turnos, PersonajeResponse? Ganador);

public sealed record TurnoEjecutadoResponse(
    TurnoResponse Turno,
    CombateDetalleResponse Combate);
