namespace Persistencia.Entidades;

public class EstadisticaDanio
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string TipoAccion { get; set; } = string.Empty;
    public decimal DanioPromedio { get; set; }
    public int DanioRealizado { get; set; }
    public int DanioRecibido { get; set; }
}
