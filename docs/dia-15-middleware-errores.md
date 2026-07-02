# Día 15: Middleware centralizado de errores

## Qué hemos hecho

- Creado una excepción personalizada `AppException` que lleva su código HTTP
- Creado un middleware `ErrorHandlingMiddleware` que atrapa todas las excepciones
- Registrado el middleware en `Program.cs` antes de los controllers
- Refactorizado los endpoints para usar `throw new AppException(...)`

## El problema que resuelve

Antes cada endpoint construía su propia respuesta de error (`BadRequest`, `NotFound`, `Conflict`). Eso significa:

- Formato de error repetido en muchos sitios
- Fácil que se queden inconsistentes
- Lógica de negocio mezclada con formato de respuesta

Con el middleware, cualquier parte del código puede lanzar `throw new AppException("mensaje", 404)` y **un único sitio** decide cómo se ve el JSON de error.

## AppException

```csharp
public class AppException : Exception
{
    public int StatusCode { get; }

    public AppException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}
```

Es una excepción normal de C# a la que le añadimos un `StatusCode`. En Java sería una clase que hereda de `RuntimeException` con un campo extra.

## El middleware

```csharp
public async Task InvokeAsync(HttpContext context)
{
    try
    {
        await _next(context);              // deja pasar al controller
    }
    catch (AppException ex)
    {
        await WriteError(context, ex.StatusCode, ex.Message);  // error controlado
    }
    catch (Exception ex)
    {
        await WriteError(context, 500, "Error interno del servidor");  // error inesperado
        Console.WriteLine(ex);
    }
}
```

- `_next` es el siguiente paso de la cadena (el controller). Equivale a `next()` en Express.
- Todo va en un `try`, así que cualquier excepción lanzada más adelante cae aquí.
- `AppException` → usamos su código. Cualquier otra → `500` sin exponer detalles.

## Formato de error unificado

```json
{
  "error": "Usuario no encontrado",
  "statusCode": 404,
  "path": "/api/users/999",
  "method": "GET",
  "timestamp": "2026-06-21T10:00:00Z"
}
```

## Comparación Express vs ASP.NET Core

| Express | ASP.NET Core |
|---------|-------------|
| `class AppError extends Error` | `class AppException : Exception` |
| `(err, req, res, next) => {}` | clase con `InvokeAsync(HttpContext)` |
| `next(new AppError(...))` | `throw new AppException(...)` |
| `app.use(errorMiddleware)` al final | `app.UseMiddleware<...>()` antes de MapControllers |

## Por qué el orden importa

El middleware debe registrarse **antes** de `MapControllers()`. Así envuelve a los controllers y puede atrapar lo que lancen. Si se registrara después, no vería esas excepciones.

> Ojo con `dotnet watch`: los cambios en el pipeline de `Program.cs` no se aplican con hot reload — hay que reiniciar el servidor completo.

## Casos probados

| Petición | Código | Formato del middleware |
|----------|--------|----------------------|
| GET /api/users/999 | 404 | Sí |
| POST datos inválidos | 400 | Sí |
| POST email duplicado | 409 | Sí |
| PATCH /api/users/999 | 404 | Sí |
