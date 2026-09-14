namespace Aplicacion.Interfaces;

using Aplicacion.Dominio;

public interface IPersonajeRepository
{
    Personaje? ObtenerPorId(int id);
    IEnumerable<Personaje> ObtenerTodos();
    int Guardar(Personaje personaje);
}
