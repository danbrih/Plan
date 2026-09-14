namespace Aplicacion.Dominio;

public class Guerrero : Personaje
{
    public override string Tipo => "Guerrero";
    public int FuerzaAdicional { get; set; } = 5;

    public override int CalcularDano() => Ataque + FuerzaAdicional;
    public override int UsarHabilidad() => (Ataque + FuerzaAdicional) * 2;
}

public class Mago : Personaje
{
    public override string Tipo => "Mago";
    public int Mana { get; set; } = 20;

    public override int CalcularDano()
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
        if (Mana >= 10)
        {
            Mana -= 10;
            return Ataque * 3;
        }
        return CalcularDano();
    }
}

public class Arquero : Personaje
{
    public override string Tipo => "Arquero";
    public int Precision { get; set; } = 3;

    public override int CalcularDano() => Ataque + Precision;
    public override int UsarHabilidad() => (Ataque + Precision) * 2;
}

public class Asesino : Personaje
{
    public override string Tipo => "Asesino";

    public override int CalcularDano() => Ataque * 2;
    public override int UsarHabilidad() => Ataque * 4;
}
