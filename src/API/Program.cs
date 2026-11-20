using Aplicacion.Interfaces;
using Aplicacion.Servicios;
using Persistencia;
using Persistencia.Repositorios;
using Persistencia.Repositorios.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDbConnectionFactory, MySqlConnectionFactory>();
builder.Services.AddScoped<IPersonajeApiRepository, PersonajeRepository>();
builder.Services.AddScoped<IBatallaApiRepository, BatallaRepository>();
builder.Services.AddScoped<CombateService>();
builder.Services.AddScoped<IPersonajeApiService, PersonajeApiService>();
builder.Services.AddScoped<ICombateApiService, CombateApiService>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();


var personajes = app.MapGroup("/api/personajes").WithTags("Personajes");
personajes.MapGet("", (IPersonajeApiService s) => ApiResult.Ejecutar(() => Results.Ok(s.Listar())))
    .WithName("ListarPersonajes").Produces<IEnumerable<Aplicacion.DTOs.PersonajeResponse>>(200);
personajes.MapGet("/{id:int}", (int id, IPersonajeApiService s) => ApiResult.Ejecutar(() => Results.Ok(s.Obtener(id))))
    .WithName("ObtenerPersonaje").Produces<Aplicacion.DTOs.PersonajeResponse>(200).Produces(404);
personajes.MapGet("/{id:int}/combates", (int id, IPersonajeApiService s) => ApiResult.Ejecutar(() => Results.Ok(s.ObtenerCombates(id))))
    .WithName("ObtenerCombatesDePersonaje").Produces<IEnumerable<Aplicacion.DTOs.CombateResumenResponse>>(200).Produces(404);
personajes.MapPost("", (Aplicacion.DTOs.CrearPersonajeRequest body, IPersonajeApiService s) =>
        ApiResult.Ejecutar(() => { var creado = s.Crear(body); return Results.Created($"/api/personajes/{creado.Id}", creado); }))
    .WithName("CrearPersonaje").Produces<Aplicacion.DTOs.PersonajeResponse>(201).Produces(400);
personajes.MapPut("/{id:int}", (int id, Aplicacion.DTOs.ActualizarNombreRequest body, IPersonajeApiService s) =>
        ApiResult.Ejecutar(() => Results.Ok(s.Actualizar(id, body))))
    .WithName("ActualizarNombrePersonaje").Produces<Aplicacion.DTOs.PersonajeResponse>(200).Produces(400).Produces(404);
personajes.MapDelete("/{id:int}", (int id, IPersonajeApiService s) =>
        ApiResult.Ejecutar(() => { s.Eliminar(id); return Results.NoContent(); }))
    .WithName("EliminarPersonaje").Produces(204).Produces(404).Produces(409);

var combates = app.MapGroup("/api/combates").WithTags("Combates y turnos");
combates.MapGet("", (ICombateApiService s) => ApiResult.Ejecutar(() => Results.Ok(s.Listar())))
    .WithName("ListarCombates").Produces<IEnumerable<Aplicacion.DTOs.CombateResumenResponse>>(200);
combates.MapPost("", (Aplicacion.DTOs.CrearCombateRequest body, ICombateApiService s) =>
        ApiResult.Ejecutar(() => { var creado = s.Crear(body); return Results.Created($"/api/combates/{creado.Id}", creado); }))
    .WithName("CrearCombate").Produces<Aplicacion.DTOs.CombateDetalleResponse>(201).Produces(400).Produces(404);
combates.MapGet("/{id:int}", (int id, ICombateApiService s) => ApiResult.Ejecutar(() => Results.Ok(s.Obtener(id))))
    .WithName("ObtenerCombate").Produces<Aplicacion.DTOs.CombateDetalleResponse>(200).Produces(404);
combates.MapPut("/{id:int}", (int id, Aplicacion.DTOs.ActualizarParticipantesRequest body, ICombateApiService s) =>
        ApiResult.Ejecutar(() => Results.Ok(s.Actualizar(id, body))))
    .WithName("ActualizarParticipantesCombate").Produces<Aplicacion.DTOs.CombateDetalleResponse>(200).Produces(400).Produces(404).Produces(409);
combates.MapDelete("/{id:int}", (int id, ICombateApiService s) =>
        ApiResult.Ejecutar(() => { s.Eliminar(id); return Results.NoContent(); }))
    .WithName("EliminarCombate").Produces(204).Produces(404).Produces(409);
combates.MapGet("/{id:int}/turnos", (int id, ICombateApiService s) => ApiResult.Ejecutar(() => Results.Ok(s.ObtenerTurnos(id))))
    .WithName("ListarTurnos").Produces<IEnumerable<Aplicacion.DTOs.TurnoResponse>>(200).Produces(404);
combates.MapPost("/{id:int}/turnos", (int id, Aplicacion.DTOs.EjecutarTurnoRequest body, ICombateApiService s) =>
        ApiResult.Ejecutar(() => Results.Ok(s.EjecutarTurno(id, body))))
    .WithName("EjecutarTurno").Produces<Aplicacion.DTOs.TurnoEjecutadoResponse>(200).Produces(400).Produces(404);
combates.MapPut("/{id:int}/turnos/{turnoId:int}", (int id, int turnoId, Aplicacion.DTOs.ActualizarNotaRequest body, ICombateApiService s) =>
        ApiResult.Ejecutar(() => Results.Ok(s.ActualizarNota(id, turnoId, body))))
    .WithName("ActualizarNotaTurno").Produces<Aplicacion.DTOs.TurnoResponse>(200).Produces(400).Produces(404);
combates.MapDelete("/{id:int}/turnos/{turnoId:int}", (int id, int turnoId, ICombateApiService s) =>
        ApiResult.Ejecutar(() => { s.EliminarTurno(id, turnoId); return Results.NoContent(); }))
    .WithName("EliminarUltimoTurno").Produces(204).Produces(404).Produces(409);

app.Run();

public partial class Program { }

internal static class ApiResult
{
    public static IResult Ejecutar(Func<IResult> accion)
    {
        try { return accion(); }
        catch (RecursoNoEncontradoException ex) { return Results.NotFound(new { error = ex.Message }); }
        catch (KeyNotFoundException ex) { return Results.NotFound(new { error = ex.Message }); }
        catch (ConflictoApiException ex) { return Results.Conflict(new { error = ex.Message }); }
        catch (ArgumentException ex) { return Results.BadRequest(new { error = ex.Message }); }
        catch (InvalidOperationException ex) { return Results.BadRequest(new { error = ex.Message }); }
    }
}
