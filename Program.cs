// Crea el "constructor" de la app. Aquí registras todo lo que necesita antes de arrancar.
var builder = WebApplication.CreateBuilder(args);

// Le dice a la app que busque clases Controller para gestionar las rutas.
builder.Services.AddControllers();

// Con todo registrado, construye la aplicación.
// A partir de aquí ya no puedes añadir servicios.
var app = builder.Build();

// Conecta las rutas definidas en los Controllers con la app.
app.MapControllers();

// Arranca el servidor y se queda escuchando peticiones.
app.Run();