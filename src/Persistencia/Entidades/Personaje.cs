namespace Persistencia.Entidades;

public abstract class Personaje
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Vida { get; set; }
    public int VidaMaxima { get; set; }
    public int Ataque { get; set; }
    public int Defensa { get; set; }

    public IHabilidad Habilidad { get; protected set; }

    protected Personaje()
    {
        Habilidad = new Habilidad("Habilidad", 1);
    }

    public abstract string Tipo { get; }

    public abstract int CalcularDanio();

    public abstract int UsarHabilidad();

    public virtual int CalcularDefensa()
    {
        return Defensa;
    }

    public virtual void RecibirDanio(int cantidad)
    {
        int danoReal = cantidad - CalcularDefensa();

        if (danoReal < 1)
            danoReal = 1;

        Vida -= danoReal;

        if (Vida < 0)
            Vida = 0;
    }

    public bool EstaVivo()
    {
        return Vida > 0;
    }

    public int Atacar(Personaje defensor)
    {
        return Atacar(defensor, false);
    }

    public int Atacar(Personaje defensor, bool usarHabilidad)
    {
        if (!EstaVivo())
            throw new InvalidOperationException("El atacante está derrotado.");

        if (!defensor.EstaVivo())
            throw new InvalidOperationException("El defensor está derrotado.");

        int dano;

        if (usarHabilidad)
            dano = UsarHabilidad();
        else
            dano = CalcularDanio();

        defensor.RecibirDanio(dano);

        return dano;
    }

    public static Personaje operator +(Personaje personaje, int vida)
    {
        personaje.Vida += vida;

        if (personaje.Vida > personaje.VidaMaxima)
            personaje.Vida = personaje.VidaMaxima;

        return personaje;
    }
}
