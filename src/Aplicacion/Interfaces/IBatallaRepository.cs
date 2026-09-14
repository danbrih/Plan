namespace Aplicacion.Interfaces;

using Aplicacion.Dominio;

public interface IBatallaRepository
{
    int Crear(Batalla batalla);
    Batalla? ObtenerPorId(int id);
    IEnumerable<Batalla> ObtenerTodas();
    void FinalizarBatalla(int batallaId, int ganadorId);
}
