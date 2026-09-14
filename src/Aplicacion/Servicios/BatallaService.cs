namespace Aplicacion.Servicios;

using Aplicacion.Dominio;
using Aplicacion.Interfaces;

public class BatallaService
{
    private readonly IBatallaRepository _batallaRepo;
    private readonly IPersonajeRepository _personajeRepo;
    private readonly CombateService _combateService;

    public BatallaService(IBatallaRepository batallaRepo, IPersonajeRepository personajeRepo, CombateService combateService)
    {
        _batallaRepo = batallaRepo;
        _personajeRepo = personajeRepo;
        _combateService = combateService;
    }

    public Batalla IniciarYResolverBatalla(int p1Id, int p2Id)
    {
        var p1 = _personajeRepo.ObtenerPorId(p1Id) ?? throw new ArgumentException("Personaje 1 no existe.");
        var p2 = _personajeRepo.ObtenerPorId(p2Id) ?? throw new ArgumentException("Personaje 2 no existe.");

        var batalla = new Batalla
        {
            Personaje1Id = p1Id,
            Personaje2Id = p2Id,
            Fecha = DateTime.Now,
            Estado = "EnProgreso"
        };

        batalla.Id = _batallaRepo.Crear(batalla);

        var ganador = _combateService.SimularCombateCompleto(p1, p2);
        batalla.GanadorId = ganador.Id;
        batalla.Estado = "Finalizada";

        _batallaRepo.FinalizarBatalla(batalla.Id, ganador.Id);
        return batalla;
    }
}
