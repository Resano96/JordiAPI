using System.Text.Json;
using UserManagerAPI.Errors;

namespace UserManagerAPI.Middleware;

// Middleware que envuelve toda la petición en un try/catch.
// Si algo lanza una excepción, aquí se convierte en una respuesta JSON uniforme.
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Deja pasar la petición al siguiente paso (equivale a next() en Express)
            await _next(context);
        }
        catch (AppException ex)
        {
            // Error controlado por nosotros: usamos su código
            await WriteError(context, ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            // Error inesperado: 500 y no exponemos detalles internos
            await WriteError(context, 500, "Error interno del servidor");
            Console.WriteLine(ex); // log para depurar en la terminal
        }
    }

    private static async Task WriteError(HttpContext context, int statusCode, string error)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var body = JsonSerializer.Serialize(new
        {
            error,
            statusCode,
            path = context.Request.Path.Value,
            method = context.Request.Method,
            timestamp = DateTime.UtcNow
        });

        await context.Response.WriteAsync(body);
    }
}