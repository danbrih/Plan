namespace Aplicacion.Servicios;

using Persistencia.Entidades;
using Persistencia.Repositorios.Interfaces;

public class BatallaService
{
    private readonly IPersonajeRepository personajeRepository;
    private readonly IBatallaRepository batallaRepository;
    private readonly CombateService combateService;

    public BatallaService(
        IPersonajeRepository personajeRepository,
        IBatallaRepository batallaRepository,
        CombateService combateService)
    {
        this.personajeRepository = personajeRepository;
        this.batallaRepository = batallaRepository;
        this.combateService = combateService;
    }

    public Batalla Crear(int personaje1Id, int personaje2Id)
    {
        ValidarPersonajes(personaje1Id, personaje2Id);

        Batalla batalla = new Batalla
        {
            Fecha = DateTime.Now,
            Personaje1Id = personaje1Id,
            Personaje2Id = personaje2Id,
            Estado = "EnProgreso"
        };

        batalla.Id = batallaRepository.Crear(batalla);

        return batalla;
    }

    public Batalla Ejecutar(int personaje1Id, int personaje2Id)
    {
        Personaje primero = ObtenerPersonaje(personaje1Id);
        Personaje segundo = ObtenerPersonaje(personaje2Id);

        if (!primero.EstaVivo() || !segundo.EstaVivo())
            throw new ArgumentException("Los dos personajes deben estar vivos.");

        Batalla batalla = Crear(personaje1Id, personaje2Id);

        int turno = 1;

        while (primero.EstaVivo() && segundo.EstaVivo())
        {
            int dano = combateService.EjecutarTurno(primero, segundo);

            batallaRepository.RegistrarTurno(new Turno
            {
                BatallaId = batalla.Id,
                AtacanteId = primero.Id,
                DefensorId = segundo.Id,
                Danio = dano,
                TipoAccion = "Ataque",
                Numero = turno,
                Fecha = DateTime.Now
            });

            turno++;

            if (segundo.EstaVivo())
            {
                dano = combateService.EjecutarTurno(segundo, primero);

                batallaRepository.RegistrarTurno(new Turno
                {
                    BatallaId = batalla.Id,
                    AtacanteId = segundo.Id,
                    DefensorId = primero.Id,
                    Danio = dano,
                    TipoAccion = "Ataque",
                    Numero = turno,
                    Fecha = DateTime.Now
                });

                turno++;
            }
        }

        Personaje ganador;

        if (primero.EstaVivo())
            ganador = primero;
        else
            ganador = segundo;

        batalla.GanadorId = ganador.Id;
        batalla.Estado = "Finalizada";

        batallaRepository.FinalizarBatalla(batalla.Id, ganador.Id);

        return batalla;
    }

    public Batalla Obtener(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El id debe ser mayor que cero.");

        Batalla? batalla = batallaRepository.ObtenerPorId(id);

        if (batalla == null)
            throw new ArgumentException("La batalla no existe.");

        return batalla;
    }

    public IEnumerable<Batalla> Listar()
    {
        return batallaRepository.ObtenerTodas();
    }

    public IEnumerable<Turno> ObtenerHistorial(int batallaId)
    {
        if (batallaId <= 0)
            throw new ArgumentException("El id debe ser mayor que cero.");

        return batallaRepository.ObtenerHistorial(batallaId);
    }

    private Personaje ObtenerPersonaje(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El id debe ser mayor que cero.");

        Personaje? personaje = personajeRepository.ObtenerPorId(id);

        if (personaje == null)
            throw new ArgumentException("El personaje no existe.");

        return personaje;
    }

    private void ValidarPersonajes(int personaje1Id, int personaje2Id)
    {
        if (personaje1Id <= 0 || personaje2Id <= 0)
            throw new ArgumentException("Los ids deben ser mayores que cero.");

        if (personaje1Id == personaje2Id)
            throw new ArgumentException("Los personajes deben ser diferentes.");

        Personaje primero = ObtenerPersonaje(personaje1Id);
        Personaje segundo = ObtenerPersonaje(personaje2Id);

        if (!primero.EstaVivo() || !segundo.EstaVivo())
            throw new ArgumentException("Los personajes deben estar vivos.");
    }
}
