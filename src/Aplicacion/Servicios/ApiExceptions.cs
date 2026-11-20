namespace Aplicacion.Servicios;

public sealed class RecursoNoEncontradoException : Exception
{
    public RecursoNoEncontradoException(string message) : base(message) { }
}

public sealed class ConflictoApiException : Exception
{
    public ConflictoApiException(string message) : base(message) { }
}
