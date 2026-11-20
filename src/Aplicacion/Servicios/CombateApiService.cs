using Aplicacion.DTOs;
using Aplicacion.Interfaces;
using Persistencia.Entidades;
using Persistencia.Repositorios.Interfaces;

namespace Aplicacion.Servicios;

public sealed class CombateApiService : ICombateApiService
{
    private readonly IPersonajeApiRepository personajes;
    private readonly IBatallaApiRepository batallas;
    private readonly CombateService combate;

    public CombateApiService(IPersonajeApiRepository personajes, IBatallaApiRepository batallas, CombateService combate)
    {
        this.personajes = personajes;
        this.batallas = batallas;
        this.combate = combate;
    }

    public IEnumerable<CombateResumenResponse> Listar() => batallas.ObtenerTodas().Select(MapearResumen).ToList();

    public CombateDetalleResponse Obtener(int id)
    {
        Batalla b = ObtenerBatalla(id);
        return MapearDetalle(b);
    }

    public CombateDetalleResponse Crear(CrearCombateRequest request)
    {
        if (request is null) throw new ArgumentException("Los dos IDs son obligatorios.");
        ValidarParticipantes(request.Personaje1Id, request.Personaje2Id);
        var b = new Batalla
        {
            Fecha = DateTime.Now,
            Personaje1Id = request.Personaje1Id,
            Personaje2Id = request.Personaje2Id,
            Estado = "EnProgreso"
        };
        b.Id = batallas.Crear(b);
        return MapearDetalle(ObtenerBatalla(b.Id));
    }

    public CombateDetalleResponse Actualizar(int id, ActualizarParticipantesRequest request)
    {
        Batalla b = ObtenerBatalla(id);
        if (request is null) throw new ArgumentException("Los IDs de participantes son obligatorios.");
        if (batallas.TieneTurnos(id))
            throw new ConflictoApiException("No se pueden cambiar los participantes después del primer turno.");
        ValidarParticipantes(request.Personaje1Id, request.Personaje2Id);
        if (!batallas.ActualizarParticipantes(id, request.Personaje1Id, request.Personaje2Id))
            throw new ConflictoApiException("El combate ya tiene turnos o cambió mientras se actualizaba.");
        return MapearDetalle(ObtenerBatalla(b.Id));
    }

    public void Eliminar(int id)
    {
        _ = ObtenerBatalla(id);
        if (batallas.TieneTurnos(id))
            throw new ConflictoApiException("No se puede eliminar un combate que ya tiene turnos.");
        batallas.Eliminar(id);
    }

    public IEnumerable<TurnoResponse> ObtenerTurnos(int id)
    {
        _ = ObtenerBatalla(id);
        return batallas.ObtenerHistorial(id).OrderBy(t => t.Numero).Select(MapearTurno).ToList();
    }

    public TurnoEjecutadoResponse EjecutarTurno(int id, EjecutarTurnoRequest request)
    {
        Batalla b = ObtenerBatalla(id);
        if (request is null) throw new ArgumentException("El atacante y la acción son obligatorios.");
        if (b.Estado != "EnProgreso") throw new ArgumentException("El combate ya finalizó.");

        string accion = (request.Accion ?? string.Empty).Trim().ToLowerInvariant();
        if (accion is not ("atacar" or "habilidad"))
            throw new ArgumentException("La acción debe ser 'atacar' o 'habilidad'.");

        Personaje atacante = ObtenerPersonaje(request.AtacanteId);
        int cantidad = batallas.ContarTurnos(id);
        int atacanteEsperado = combate.ObtenerAtacanteSiguiente(cantidad + 1, b.Personaje1Id, b.Personaje2Id);
        if (request.AtacanteId != atacanteEsperado)
            throw new ArgumentException("El atacante está fuera de turno.");
        int defensorId = atacante.Id == b.Personaje1Id ? b.Personaje2Id : b.Personaje1Id;
        Personaje defensor = ObtenerPersonaje(defensorId);
        int vidaP1Antes = b.Personaje1Id == atacante.Id ? atacante.Vida : defensor.Vida;
        int vidaP2Antes = b.Personaje2Id == atacante.Id ? atacante.Vida : defensor.Vida;

        int dano = combate.EjecutarTurno(atacante, defensor, accion == "habilidad");
        int vidaP1Despues = b.Personaje1Id == atacante.Id ? atacante.Vida : defensor.Vida;
        int vidaP2Despues = b.Personaje2Id == atacante.Id ? atacante.Vida : defensor.Vida;
        int? ganador = defensor.EstaVivo() ? null : atacante.Id;

        var turno = new Turno
        {
            BatallaId = id,
            AtacanteId = atacante.Id,
            DefensorId = defensor.Id,
            Danio = dano,
            TipoAccion = accion,
            Numero = cantidad + 1,
            Fecha = DateTime.Now,
            Nota = string.Empty,
            VidaPersonaje1Antes = vidaP1Antes,
            VidaPersonaje2Antes = vidaP2Antes,
            EstadoAntes = b.Estado,
            GanadorAntes = b.GanadorId
        };
        turno.Id = batallas.RegistrarTurnoTransaccional(
            turno, vidaP1Despues, vidaP2Despues,
            ganador.HasValue ? "Finalizada" : "EnProgreso", ganador);
        return new TurnoEjecutadoResponse(MapearTurno(turno), MapearDetalle(ObtenerBatalla(id)));
    }

    public TurnoResponse ActualizarNota(int id, int turnoId, ActualizarNotaRequest request)
    {
        _ = ObtenerBatalla(id);
        if (turnoId <= 0) throw new ArgumentException("El ID del turno debe ser mayor que cero.");
        if (request is null || string.IsNullOrWhiteSpace(request.Nota) || request.Nota.Trim().Length > 255)
            throw new ArgumentException("La nota debe contener entre 1 y 255 caracteres.");
        if (batallas.ObtenerTurno(id, turnoId) is null)
            throw new RecursoNoEncontradoException("El turno no existe en ese combate.");
        string nota = request.Nota.Trim();
        if (!batallas.ActualizarNota(id, turnoId, nota))
            throw new RecursoNoEncontradoException("El turno no existe en ese combate.");
        Turno turno = batallas.ObtenerTurno(id, turnoId)!;
        return MapearTurno(turno);
    }

    public void EliminarTurno(int id, int turnoId)
    {
        _ = ObtenerBatalla(id);
        if (turnoId <= 0) throw new ArgumentException("El ID del turno debe ser mayor que cero.");
        int resultado = batallas.EliminarUltimoTurno(id, turnoId);
        if (resultado == 0) throw new RecursoNoEncontradoException("El turno no existe en ese combate.");
        if (resultado < 0) throw new ConflictoApiException("Sólo se puede eliminar el último turno del combate.");
    }

    private Batalla ObtenerBatalla(int id)
    {
        if (id <= 0) throw new ArgumentException("El ID del combate debe ser mayor que cero.");
        return batallas.ObtenerPorId(id) ?? throw new RecursoNoEncontradoException("El combate no existe.");
    }

    private Personaje ObtenerPersonaje(int id) =>
        personajes.ObtenerPorId(id) ?? throw new RecursoNoEncontradoException("El personaje no existe.");

    private void ValidarParticipantes(int id1, int id2)
    {
        if (id1 <= 0 || id2 <= 0 || id1 == id2)
            throw new ArgumentException("Se necesitan dos IDs positivos y distintos.");
        Personaje p1 = ObtenerPersonaje(id1);
        Personaje p2 = ObtenerPersonaje(id2);
        if (!p1.EstaVivo() || !p2.EstaVivo())
            throw new ArgumentException("Los dos personajes deben estar vivos.");
    }

    private CombateDetalleResponse MapearDetalle(Batalla b)
    {
        Personaje p1 = ObtenerPersonaje(b.Personaje1Id);
        Personaje p2 = ObtenerPersonaje(b.Personaje2Id);
        PersonajeResponse? ganador = b.GanadorId.HasValue
            ? PersonajeApiService.Mapear(ObtenerPersonaje(b.GanadorId.Value)) : null;
        return new CombateDetalleResponse(
            b.Id, b.Fecha, b.Estado,
            PersonajeApiService.Mapear(p1), PersonajeApiService.Mapear(p2),
            batallas.ObtenerHistorial(b.Id).OrderBy(t => t.Numero).Select(MapearTurno).ToList(),
            ganador);
    }

    private CombateResumenResponse MapearResumen(Batalla b)
    {
        Personaje p1 = ObtenerPersonaje(b.Personaje1Id);
        Personaje p2 = ObtenerPersonaje(b.Personaje2Id);
        return new CombateResumenResponse(b.Id, b.Fecha, p1.Id, p1.Nombre, p2.Id, p2.Nombre, b.Estado, b.GanadorId);
    }

    private static TurnoResponse MapearTurno(Turno t) =>
        new(t.Id, t.BatallaId, t.AtacanteId, t.DefensorId, t.Danio, t.TipoAccion, t.Numero, t.Fecha, t.Nota);
}
