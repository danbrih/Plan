using Aplicacion.Servicios;
using Persistencia;

Console.WriteLine("=== Aplicación Iniciada ===");

var dbFactory = new MySqlConnectionFactory("Server=localhost;Database=mi_db;User=root;");
var servicioA = new ServicioA(dbFactory);

servicioA.Ejecutar();

Console.WriteLine("=== Proceso Finalizado ===");
