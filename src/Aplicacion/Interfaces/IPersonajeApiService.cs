using Aplicacion.DTOs;

namespace Aplicacion.Interfaces;

public interface IPersonajeApiService
{
    IEnumerable<PersonajeResponse> Listar();
    PersonajeResponse Obtener(int id);
    IEnumerable<CombateResumenResponse> ObtenerCombates(int id);
    PersonajeResponse Crear(CrearPersonajeRequest request);
    PersonajeResponse Actualizar(int id, ActualizarNombreRequest request);
    void Eliminar(int id);
}
