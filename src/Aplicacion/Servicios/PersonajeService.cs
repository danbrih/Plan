namespace Aplicacion.Servicios;

using Persistencia.Entidades;
using Persistencia.Repositorios.Interfaces;

public class PersonajeService
{
    private readonly IPersonajeRepository repository;

    public PersonajeService(IPersonajeRepository repository)
    {
        this.repository = repository;
    }

    public int Registrar(Personaje personaje)
    {
        if (personaje == null)
            throw new ArgumentException("El personaje es obligatorio.");

        if (string.IsNullOrWhiteSpace(personaje.Nombre))
            throw new ArgumentException("El nombre es obligatorio.");

        if (personaje.Vida <= 0)
            throw new ArgumentException("La vida debe ser mayor que cero.");

        if (personaje.Ataque <= 0)
            throw new ArgumentException("El ataque debe ser mayor que cero.");

        if (personaje.Defensa < 0)
            throw new ArgumentException("La defensa no puede ser negativa.");

        return repository.Guardar(personaje);
    }

    public Personaje Obtener(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El id debe ser mayor que cero.");

        Personaje? personaje = repository.ObtenerPorId(id);

        if (personaje == null)
            throw new ArgumentException("El personaje no existe.");

        return personaje;
    }

    public IEnumerable<Personaje> Listar()
    {
        return repository.ObtenerTodos();
    }
}
