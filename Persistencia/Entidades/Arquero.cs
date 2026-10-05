namespace Persistencia.Entidades;

public class Arquero : Personaje
{
    public int Precision { get; set; } = 3;

    public override string Tipo
    {
        get { return "Arquero"; }
    }

    public Arquero()
    {
        Habilidad = new Habilidad("Disparo preciso", 2);
    }

    public override int CalcularDanio()
    {
        return Ataque + Precision;
    }

    public override int UsarHabilidad()
    {
        return Habilidad.CalcularDanio(Ataque + Precision);
    }

    public override int CalcularDefensa()
    {
        return Defensa;
    }
}
