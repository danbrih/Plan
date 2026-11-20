namespace Persistencia.Entidades;

public class Asesino : Personaje
{
    public override string Tipo
    {
        get { return "Asesino"; }
    }

    public Asesino()
    {
        Habilidad = new Habilidad("Ataque mortal", 4);
    }

    public override int CalcularDanio()
    {
        return Ataque * 2;
    }

    public override int UsarHabilidad()
    {
        return Habilidad.CalcularDanio(Ataque);
    }

    public override int CalcularDefensa()
    {
        return Defensa + 1;
    }
}
