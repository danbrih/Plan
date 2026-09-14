namespace Aplicacion.Dominio;

public abstract class Personaje
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Vida { get; set; }
    public int Ataque { get; set; }
    public int Defensa { get; set; }

    public abstract int CalcularDano();

    public virtual void RecibirDano(int cantidad)
    {
        int danoReal = cantidad - Defensa;
        if (danoReal < 0) danoReal = 0;
        Vida -= danoReal;
        if (Vida < 0) Vida = 0;
    }
}
