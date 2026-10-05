namespace Persistencia.Entidades;

public class EstadisticaTipo
{
    public string Tipo { get; set; } = string.Empty;
    public int BatallasTotales { get; set; }
    public int Victorias { get; set; }
    public int Derrotas { get; set; }
}
