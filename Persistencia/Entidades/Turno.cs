namespace Persistencia.Entidades;

public class Turno
{
    public int Id { get; set; }
    public int BatallaId { get; set; }
    public int AtacanteId { get; set; }
    public int DefensorId { get; set; }
    public int Danio { get; set; }
    public string TipoAccion { get; set; } = string.Empty;
    public int Numero { get; set; }
    public DateTime Fecha { get; set; }
}
