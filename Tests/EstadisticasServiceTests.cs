namespace Tests;

using Aplicacion.Servicios;
using Persistencia.Entidades;
using Persistencia.Repositorios.Interfaces;
using Xunit;

public class EstadisticasServiceTests
{
    [Fact]
    public void ObtenerRankingValidaFechas()
    {
        EstadisticasService service = new EstadisticasService(
            new BatallaRepositoryFalso());

        Assert.Throws<ArgumentException>(() =>
            service.ObtenerRanking(
                new DateTime(2026, 10, 2),
                new DateTime(2026, 10, 1)));
    }

    [Fact]
    public void ObtenerResumenPorTipoDevuelveResultados()
    {
        BatallaRepositoryFalso repository = new BatallaRepositoryFalso();

        repository.Resumen.Add(new EstadisticaTipo
        {
            Tipo = "Guerrero",
            BatallasTotales = 5,
            Victorias = 3,
            Derrotas = 2
        });

        EstadisticasService service = new EstadisticasService(repository);

        IEnumerable<EstadisticaTipo> resultado =
            service.ObtenerResumenPorTipo(
                new DateTime(2026, 10, 1),
                new DateTime(2026, 10, 2));

        Assert.Single(resultado);
    }

    private class BatallaRepositoryFalso : IBatallaRepository
    {
        public List<EstadisticaTipo> Resumen { get; } = new List<EstadisticaTipo>();

        public int Crear(Batalla batalla) => 1;

        public Batalla? ObtenerPorId(int id) => null;

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
            return Resumen;
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



