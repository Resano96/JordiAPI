# Día 2: Preparación del proyecto

## Qué hemos hecho

- Creado el proyecto ASP.NET Core Web API con `dotnet new webapi`
- Limpiado `Program.cs` eliminando el ejemplo de WeatherForecast
- Configurado el servidor para usar Controllers
- Creado la carpeta `Controllers`
- Creado `HealthController.cs` como primera ruta de prueba
- Servidor arrancando en local

## Equivalencias con Node.js

| Node.js | C# |
|---|---|
| `npm init` + `package.json` | `dotnet new webapi` + `.csproj` |
| `npm install express` | ASP.NET Core viene incluido |
| `tsconfig.json` | No necesario, C# compila directamente |
| `src/server.ts` | `Program.cs` |
| `npm run dev` | `dotnet watch` |
| `app.listen(3000)` | `app.Run()` en Program.cs |

## Comando para arrancar el proyecto

```bash
dotnet watch
```

## URL de prueba

```
http://localhost:5207/api/health
```

## Respuesta obtenida

```json
{
  "status": "ok",
  "message": "UserManager API funcionando"
}
```

## Estructura actual del proyecto

```
UserManagerAPI/
  Controllers/
    HealthController.cs
  docs/
    dia-02-preparacion-proyecto.md
  Program.cs
  UserManagerAPI.csproj
  appsettings.json
```

## Explicación de Program.cs

```csharp
// Crea el "constructor" de la app. Aquí registras todo lo que necesita antes de arrancar.
var builder = WebApplication.CreateBuilder(args);

// Le dice a la app que busque clases Controller para gestionar las rutas.
builder.Services.AddControllers();

// Con todo registrado, construye la aplicación.
var app = builder.Build();

// Conecta las rutas definidas en los Controllers con la app.
app.MapControllers();

// Arranca el servidor y se queda escuchando peticiones.
app.Run();
```

## Explicación de HealthController.cs

```csharp
[ApiController]  // Activa validaciones automáticas — equivale a @RestController en Java
[Route("api/health")]  // Define la URL base del controller
public class HealthController : ControllerBase  // ControllerBase da acceso a Ok(), NotFound(), etc.
{
    [HttpGet]  // Este método responde a GET /api/health
    public IActionResult Get()
    {
        return Ok(new { status = "ok", message = "UserManager API funcionando" });
    }
}
```
