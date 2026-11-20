namespace Exploracion.Configuracion;

public sealed class PersonajeRepositoryMemoria
{
    private readonly List<string> nombres = new() { "Guerrero de prueba", "Mago de prueba" };
    public IEnumerable<string> ObtenerTodos() => nombres;
}
