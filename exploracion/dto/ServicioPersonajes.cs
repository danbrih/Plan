namespace Exploracion.Dto;

public sealed class ServicioPersonajes
{
    private int siguienteId = 1;
    public PersonajeCreadoDto Crear(CrearPersonajeDto entrada)
    {
        if (string.IsNullOrWhiteSpace(entrada.Nombre) || string.IsNullOrWhiteSpace(entrada.Tipo))
            throw new ArgumentException("Nombre y tipo son obligatorios.");
        return new PersonajeCreadoDto(siguienteId++, entrada.Nombre.Trim());
    }
}
