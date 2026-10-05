namespace Persistencia.Entidades;

public class Mago : Personaje
{
    public int Mana { get; set; } = 20;

    public override string Tipo
    {
        get { return "Mago"; }
    }

    public Mago()
    {
        Habilidad = new Habilidad("Hechizo", 3);
    }

    public override int CalcularDanio()
    {
        if (Mana >= 5)
        {
            Mana -= 5;
            return Ataque * 2;
        }

        return Ataque;
    }

    public override int UsarHabilidad()
    {
        if (Mana < 10)
            return CalcularDanio();

        Mana -= 10;
        return Habilidad.CalcularDanio(Ataque);
    }

    public override int CalcularDefensa()
    {
        return Defensa;
    }
}
