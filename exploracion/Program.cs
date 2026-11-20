// Prueba de concepto mínima de Minimal API: una ruta GET fija.
// Se ejecuta como parte de la exploración conceptual; el proyecto definitivo
// está en src/API.
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/exploracion", () => Results.Ok(new { mensaje = "Minimal API funcionando" }));
app.Run();
