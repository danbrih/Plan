using Exploracion.Configuracion;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<PersonajeRepositoryMemoria>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/exploracion/personajes", (PersonajeRepositoryMemoria repo) => Results.Ok(repo.ObtenerTodos()));
app.Run();
