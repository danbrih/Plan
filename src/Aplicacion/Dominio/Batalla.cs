namespace Aplicacion.Dominio;

public class Batalla
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public int Personaje1Id { get; set; }
    public int Personaje2Id { get; set; }
    public int? GanadorId { get; set; }
    public string Estado { get; set; } = "EnProgreso";
}
