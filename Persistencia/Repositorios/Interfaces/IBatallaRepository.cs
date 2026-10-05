namespace Persistencia.Repositorios.Interfaces;

using Persistencia.Entidades;

public interface IBatallaRepository
{
    int Crear(Batalla batalla);
    Batalla? ObtenerPorId(int id);
    IEnumerable<Batalla> ObtenerTodas();
    IEnumerable<Turno> ObtenerHistorial(int batallaId);
    void RegistrarTurno(Turno turno);
    void FinalizarBatalla(int batallaId, int ganadorId);

    IEnumerable<EstadisticaTipo> ObtenerResumenPorTipo(DateTime inicio, DateTime fin);
    IEnumerable<EstadisticaTipo> ObtenerVictoriasDerrotasPorTipo(DateTime inicio, DateTime fin);
    IEnumerable<RankingPersonaje> ObtenerRanking(DateTime inicio, DateTime fin);
    IEnumerable<EstadisticaDanio> ObtenerDanioPromedio(DateTime inicio, DateTime fin);
    IEnumerable<EstadisticaDanio> ObtenerDanioTotal(DateTime inicio, DateTime fin);
    IEnumerable<EstadisticaPeriodo> ObtenerBatallasPorPeriodo(DateTime inicio, DateTime fin);
}
