namespace Aplicacion.Servicios;

using Persistencia.Entidades;
using Persistencia.Repositorios.Interfaces;

public class EstadisticasService
{
    private readonly IBatallaRepository repository;

    public EstadisticasService(IBatallaRepository repository)
    {
        this.repository = repository;
    }

    public IEnumerable<EstadisticaTipo> ObtenerResumenPorTipo(DateTime inicio, DateTime fin)
    {
        ValidarFechas(inicio, fin);
        return repository.ObtenerResumenPorTipo(inicio, fin);
    }

    public IEnumerable<EstadisticaTipo> ObtenerVictoriasDerrotasPorTipo(DateTime inicio, DateTime fin)
    {
        ValidarFechas(inicio, fin);
        return repository.ObtenerVictoriasDerrotasPorTipo(inicio, fin);
    }

    public IEnumerable<RankingPersonaje> ObtenerRanking(DateTime inicio, DateTime fin)
    {
        ValidarFechas(inicio, fin);
        return repository.ObtenerRanking(inicio, fin);
    }

    public IEnumerable<EstadisticaDanio> ObtenerDanioPromedio(DateTime inicio, DateTime fin)
    {
        ValidarFechas(inicio, fin);
        return repository.ObtenerDanioPromedio(inicio, fin);
    }

    public IEnumerable<EstadisticaDanio> ObtenerDanioTotal(DateTime inicio, DateTime fin)
    {
        ValidarFechas(inicio, fin);
        return repository.ObtenerDanioTotal(inicio, fin);
    }

    public IEnumerable<EstadisticaPeriodo> ObtenerBatallasPorPeriodo(DateTime inicio, DateTime fin)
    {
        ValidarFechas(inicio, fin);
        return repository.ObtenerBatallasPorPeriodo(inicio, fin);
    }

    private static void ValidarFechas(DateTime inicio, DateTime fin)
    {
        if (fin < inicio)
            throw new ArgumentException("La fecha final no puede ser menor que la fecha inicial.");
    }
}
