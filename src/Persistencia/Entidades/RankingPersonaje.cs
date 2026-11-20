namespace Persistencia.Entidades;

public class RankingPersonaje
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public int PartidasJugadas { get; set; }
    public int Victorias { get; set; }
    public decimal PorcentajeEfectividad { get; set; }
}
