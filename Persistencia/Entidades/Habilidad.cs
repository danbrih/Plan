namespace Persistencia.Entidades;

public class Habilidad : IHabilidad
{
    public string Nombre { get; set; } = string.Empty;
    public int Multiplicador { get; set; }

    public Habilidad()
    {
    }

    public Habilidad(string nombre, int multiplicador)
    {
        Nombre = nombre;
        Multiplicador = multiplicador;
    }

    public int CalcularDanio(int ataque)
    {
        return ataque * Multiplicador;
    }
}
