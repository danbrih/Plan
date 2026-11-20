using Aplicacion.DTOs;
using Aplicacion.Interfaces;
using Persistencia.Entidades;
using Persistencia.Repositorios.Interfaces;

namespace Aplicacion.Servicios;

public sealed class PersonajeApiService : IPersonajeApiService
{
    private readonly IPersonajeApiRepository personajes;
    private readonly IBatallaApiRepository batallas;

    public PersonajeApiService(IPersonajeApiRepository personajes, IBatallaApiRepository batallas)
    {
        this.personajes = personajes;
        this.batallas = batallas;
    }

    public IEnumerable<PersonajeResponse> Listar() => personajes.ObtenerTodos().Select(Mapear).ToList();

    public PersonajeResponse Obtener(int id) => Mapear(ObtenerEntidad(id));

    public IEnumerable<CombateResumenResponse> ObtenerCombates(int id)
    {
        _ = ObtenerEntidad(id);
        return personajes.ObtenerCombates(id).Select(MapearCombate).ToList();
    }

    public PersonajeResponse Crear(CrearPersonajeRequest request)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Nombre))
            throw new ArgumentException("El nombre es obligatorio.");
        if (request.Nombre.Trim().Length > 50)
            throw new ArgumentException("El nombre no puede superar 50 caracteres.");
        if (string.IsNullOrWhiteSpace(request.Tipo))
            throw new ArgumentException("El tipo es obligatorio.");

        Personaje personaje = request.Tipo.Trim().ToLowerInvariant() switch
        {
            "guerrero" => new Guerrero(),
            "mago" => new Mago(),
            "arquero" => new Arquero(),
            "asesino" => new Asesino(),
            _ => throw new ArgumentException("Tipo inválido. Usá Guerrero, Mago, Arquero o Asesino.")
        };
        personaje.Nombre = request.Nombre.Trim();
        personaje.Vida = 100;
        personaje.VidaMaxima = 100;
        personaje.Ataque = 10;
        personaje.Defensa = 2;

        personaje.Id = personajes.Guardar(personaje);
        return Mapear(ObtenerEntidad(personaje.Id));
    }

    public PersonajeResponse Actualizar(int id, ActualizarNombreRequest request)
    {
        _ = ObtenerEntidad(id);
        if (request is null || string.IsNullOrWhiteSpace(request.Nombre) || request.Nombre.Trim().Length > 50)
            throw new ArgumentException("El nombre debe contener entre 1 y 50 caracteres.");
        if (!personajes.ActualizarNombre(id, request.Nombre.Trim()))
            throw new RecursoNoEncontradoException("El personaje no existe.");
        return Obtener(id);
    }

    public void Eliminar(int id)
    {
        _ = ObtenerEntidad(id);
        if (personajes.ParticipoEnCombate(id))
            throw new ConflictoApiException("No se puede eliminar un personaje que participó en un combate.");
        personajes.Eliminar(id);
    }

    private Personaje ObtenerEntidad(int id)
    {
        if (id <= 0) throw new ArgumentException("El ID debe ser mayor que cero.");
        return personajes.ObtenerPorId(id) ?? throw new RecursoNoEncontradoException("El personaje no existe.");
    }

    private CombateResumenResponse MapearCombate(Batalla b)
    {
        Personaje p1 = ObtenerEntidad(b.Personaje1Id);
        Personaje p2 = ObtenerEntidad(b.Personaje2Id);
        return new CombateResumenResponse(b.Id, b.Fecha, p1.Id, p1.Nombre, p2.Id, p2.Nombre, b.Estado, b.GanadorId);
    }

    internal static PersonajeResponse Mapear(Personaje p) =>
        new(p.Id, p.Nombre, p.Tipo, p.Vida, p.VidaMaxima, p.Ataque, p.Defensa);
}
