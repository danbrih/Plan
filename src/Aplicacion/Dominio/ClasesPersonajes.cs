namespace Aplicacion.Dominio;

public class Guerrero : Personaje
{
    public int FuerzaAdicional { get; set; } = 5;
    public override int CalcularDano() => Ataque + FuerzaAdicional;
}

public class Mago : Personaje
{
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
}

public class Arquero : Personaje
{
    public override int CalcularDano() => Ataque + 3;
}

public class Asesino : Personaje
{
    public override int CalcularDano() => Ataque * 3;
}
