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
    public string Nota { get; set; } = string.Empty;
    public int VidaPersonaje1Antes { get; set; }
    public int VidaPersonaje2Antes { get; set; }
    public string EstadoAntes { get; set; } = "EnProgreso";
    public int? GanadorAntes { get; set; }
}
