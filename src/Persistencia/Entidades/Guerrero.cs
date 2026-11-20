namespace Persistencia.Entidades;

public class Guerrero : Personaje
{
    public int FuerzaAdicional { get; set; } = 5;

    public override string Tipo
    {
        get { return "Guerrero"; }
    }

    public Guerrero()
    {
        Habilidad = new Habilidad("Golpe fuerte", 2);
    }

    public override int CalcularDanio()
    {
        return Ataque + FuerzaAdicional;
    }

    public override int UsarHabilidad()
    {
        return Habilidad.CalcularDanio(Ataque) + FuerzaAdicional;
    }

    public override int CalcularDefensa()
    {
        return Defensa + 2;
    }
}
