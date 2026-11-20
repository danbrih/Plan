namespace Tests;

using Aplicacion.Servicios;
using Persistencia.Entidades;
using Persistencia.Repositorios.Interfaces;
using Xunit;

public class BatallaServiceTests
{
    [Fact]
    public void CrearBatallaValidaLosPersonajes()
    {
        PersonajeRepositoryFalso personajes = new PersonajeRepositoryFalso();
        personajes.Agregar(new Guerrero { Id = 1, Vida = 100, VidaMaxima = 100, Ataque = 10 });
        personajes.Agregar(new Mago { Id = 2, Vida = 100, VidaMaxima = 100, Ataque = 10 });

        BatallaRepositoryFalso batallas = new BatallaRepositoryFalso();
        BatallaService service = new BatallaService(
            personajes,
            batallas,
            new CombateService());

        Batalla batalla = service.Crear(1, 2);

        Assert.Equal(1, batalla.Id);
        Assert.Equal("EnProgreso", batalla.Estado);
    }

    [Fact]
    public void NoPermiteLaMismaPersonaEnAmbosLados()
    {
        PersonajeRepositoryFalso personajes = new PersonajeRepositoryFalso();
        personajes.Agregar(new Guerrero { Id = 1, Vida = 100, VidaMaxima = 100, Ataque = 10 });

        BatallaService service = new BatallaService(
            personajes,
            new BatallaRepositoryFalso(),
            new CombateService());

        Assert.Throws<ArgumentException>(() => service.Crear(1, 1));
    }

    private class PersonajeRepositoryFalso : IPersonajeRepository
    {
        private readonly List<Personaje> personajes = new List<Personaje>();

        public void Agregar(Personaje personaje)
        {
            personajes.Add(personaje);
        }

        public int Guardar(Personaje personaje)
        {
            personajes.Add(personaje);
            return personaje.Id;
        }

        public Personaje? ObtenerPorId(int id)
        {
            foreach (Personaje personaje in personajes)
            {
                if (personaje.Id == id)
                    return personaje;
            }

            return null;
        }

        public IEnumerable<Personaje> ObtenerTodos()
        {
            return personajes;
        }
    }

    private class BatallaRepositoryFalso : IBatallaRepository
    {
        private int siguienteId = 1;

        public int Crear(Batalla batalla)
        {
            batalla.Id = siguienteId;
            siguienteId++;
            return batalla.Id;
        }

        public Batalla? ObtenerPorId(int id)
        {
            return null;
        }

        public IEnumerable<Batalla> ObtenerTodas()
        {
            return new List<Batalla>();
        }

        public IEnumerable<Turno> ObtenerHistorial(int batallaId)
        {
            return new List<Turno>();
        }

        public void RegistrarTurno(Turno turno)
        {
        }

        public void FinalizarBatalla(int batallaId, int ganadorId)
        {
        }

        public IEnumerable<EstadisticaTipo> ObtenerResumenPorTipo(DateTime inicio, DateTime fin)
        {
            return new List<EstadisticaTipo>();
        }

        public IEnumerable<EstadisticaTipo> ObtenerVictoriasDerrotasPorTipo(DateTime inicio, DateTime fin)
        {
            return new List<EstadisticaTipo>();
        }

        public IEnumerable<RankingPersonaje> ObtenerRanking(DateTime inicio, DateTime fin)
        {
            return new List<RankingPersonaje>();
        }

        public IEnumerable<EstadisticaDanio> ObtenerDanioPromedio(DateTime inicio, DateTime fin)
        {
            return new List<EstadisticaDanio>();
        }

        public IEnumerable<EstadisticaDanio> ObtenerDanioTotal(DateTime inicio, DateTime fin)
        {
            return new List<EstadisticaDanio>();
        }

        public IEnumerable<EstadisticaPeriodo> ObtenerBatallasPorPeriodo(DateTime inicio, DateTime fin)
        {
            return new List<EstadisticaPeriodo>();
        }
    }
}
