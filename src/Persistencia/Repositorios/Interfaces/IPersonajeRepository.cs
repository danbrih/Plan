namespace Persistencia.Repositorios.Interfaces;

using Persistencia.Entidades;

public interface IPersonajeRepository
{
    int Guardar(Personaje personaje);
    Personaje? ObtenerPorId(int id);
    IEnumerable<Personaje> ObtenerTodos();
}
