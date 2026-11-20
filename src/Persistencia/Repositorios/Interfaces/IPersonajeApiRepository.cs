using Persistencia.Entidades;

namespace Persistencia.Repositorios.Interfaces;

public interface IPersonajeApiRepository : IPersonajeRepository
{
    IEnumerable<Batalla> ObtenerCombates(int personajeId);
    bool ParticipoEnCombate(int personajeId);
    bool ActualizarNombre(int id, string nombre);
    void Eliminar(int id);
}
