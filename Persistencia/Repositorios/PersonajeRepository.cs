namespace Persistencia.Repositorios;

using System.Data;
using Dapper;
using Persistencia.Entidades;
using Persistencia.Repositorios.Interfaces;

public class PersonajeRepository : IPersonajeRepository
{
    private readonly IDbConnectionFactory factory;

    public PersonajeRepository(IDbConnectionFactory factory)
    {
        this.factory = factory;
    }

    public int Guardar(Personaje personaje)
    {
        using IDbConnection db = factory.CrearConexionDesarrollo();

        string sql =
            "INSERT INTO Personaje (Nombre, Tipo, Vida, Ataque, Defensa) " +
            "VALUES (@Nombre, @Tipo, @Vida, @Ataque, @Defensa); " +
            "SELECT LAST_INSERT_ID();";

        return db.ExecuteScalar<int>(sql, new
        {
            personaje.Nombre,
            Tipo = personaje.Tipo,
            personaje.Vida,
            personaje.Ataque,
            personaje.Defensa
        });
    }

    public Personaje? ObtenerPorId(int id)
    {
        using IDbConnection db = factory.CrearConexionDesarrollo();

        string sql = "SELECT Id, Nombre, Tipo, Vida, Ataque, Defensa " +
                     "FROM Personaje WHERE Id = @Id";

        PersonajeDatos fila = db.QueryFirstOrDefault<PersonajeDatos>(sql, new { Id = id });

        if (fila == null)
            return null;

        return Convertir(fila);
    }

    public IEnumerable<Personaje> ObtenerTodos()
    {
        using IDbConnection db = factory.CrearConexionDesarrollo();

        string sql = "SELECT Id, Nombre, Tipo, Vida, Ataque, Defensa FROM Personaje";

        IEnumerable<PersonajeDatos> filas = db.Query<PersonajeDatos>(sql);

        List<Personaje> personajes = new List<Personaje>();

        foreach (PersonajeDatos fila in filas)
            personajes.Add(Convertir(fila));

        return personajes;
    }

    private Personaje Convertir(PersonajeDatos fila)
    {
        Personaje personaje;

        if (fila.Tipo == "Guerrero")
            personaje = new Guerrero();
        else if (fila.Tipo == "Mago")
            personaje = new Mago();
        else if (fila.Tipo == "Arquero")
            personaje = new Arquero();
        else
            personaje = new Asesino();

        personaje.Id = fila.Id;
        personaje.Nombre = fila.Nombre;
        personaje.Vida = fila.Vida;
        personaje.VidaMaxima = fila.Vida;
        personaje.Ataque = fila.Ataque;
        personaje.Defensa = fila.Defensa;

        return personaje;
    }

    private class PersonajeDatos
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int Vida { get; set; }
        public int Ataque { get; set; }
        public int Defensa { get; set; }
    }
}
