namespace Tests;

using Aplicacion.Servicios;
using Persistencia.Entidades;
using Persistencia.Repositorios.Interfaces;
using Xunit;

public class PersonajeServiceTests
{
    [Fact]
    public void RegistrarGuerreroDevuelveId()
    {
        PersonajeRepositoryFalso repository = new PersonajeRepositoryFalso();
        PersonajeService service = new PersonajeService(repository);

        Guerrero guerrero = new Guerrero
        {
            Nombre = "Conan",
            Vida = 100,
            VidaMaxima = 100,
            Ataque = 10,
            Defensa = 5
        };

        int id = service.Registrar(guerrero);

        Assert.Equal(1, id);
    }

    [Fact]
    public void RegistrarPersonajeSinNombreLanzaError()
    {
        PersonajeRepositoryFalso repository = new PersonajeRepositoryFalso();
        PersonajeService service = new PersonajeService(repository);

        Guerrero guerrero = new Guerrero
        {
            Vida = 100,
            Ataque = 10,
            Defensa = 5
        };

        Assert.Throws<ArgumentException>(() => service.Registrar(guerrero));
    }

    [Fact]
    public void ObtenerPersonajeExistenteFunciona()
    {
        PersonajeRepositoryFalso repository = new PersonajeRepositoryFalso();
        repository.Personaje = new Guerrero
        {
            Id = 1,
            Nombre = "Conan",
            Vida = 100,
            VidaMaxima = 100,
            Ataque = 10,
            Defensa = 5
        };

        PersonajeService service = new PersonajeService(repository);

        Personaje personaje = service.Obtener(1);

        Assert.Equal("Conan", personaje.Nombre);
    }

    private class PersonajeRepositoryFalso : IPersonajeRepository
    {
        public Personaje? Personaje { get; set; }

        public int Guardar(Personaje personaje)
        {
            personaje.Id = 1;
            return 1;
        }

        public Personaje? ObtenerPorId(int id)
        {
            return Personaje;
        }

        public IEnumerable<Personaje> ObtenerTodos()
        {
            if (Personaje == null)
                return new List<Personaje>();

            return new List<Personaje> { Personaje };
        }
    }
}
