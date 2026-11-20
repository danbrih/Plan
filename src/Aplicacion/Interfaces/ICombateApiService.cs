using Aplicacion.DTOs;

namespace Aplicacion.Interfaces;

public interface ICombateApiService
{
    IEnumerable<CombateResumenResponse> Listar();
    CombateDetalleResponse Obtener(int id);
    CombateDetalleResponse Crear(CrearCombateRequest request);
    CombateDetalleResponse Actualizar(int id, ActualizarParticipantesRequest request);
    void Eliminar(int id);
    IEnumerable<TurnoResponse> ObtenerTurnos(int id);
    TurnoEjecutadoResponse EjecutarTurno(int id, EjecutarTurnoRequest request);
    TurnoResponse ActualizarNota(int id, int turnoId, ActualizarNotaRequest request);
    void EliminarTurno(int id, int turnoId);
}
