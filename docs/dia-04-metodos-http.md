# Día 4: Métodos HTTP

## Qué hemos hecho

- Creado `UsersController.cs` con las rutas simuladas de usuarios
- Probado `GET /api/users` — listado vacío
- Probado `GET /api/users/:id` — detalle por ID
- Probado `POST /api/users` — recibir body JSON
- Probado `PATCH /api/users/:id` — recibir cambios parciales
- Probado `DELETE /api/users/:id` — recibir ID a eliminar

## Endpoints trabajados

```http
GET    /api/users
GET    /api/users/{id}
POST   /api/users
PATCH  /api/users/{id}
DELETE /api/users/{id}
```

## Código del controller

```csharp
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new
    {
        message = "Listado de usuarios",
        data = Array.Empty<object>()
    });

    [HttpGet("{id}")]
    public IActionResult GetById(int id) =>
        Ok(new { message = "Detalle de usuario", id = id });

    [HttpPost]
    public IActionResult Create([FromBody] object userData) =>
        StatusCode(201, new { message = "Usuario recibido para crear", data = userData });

    [HttpPatch("{id}")]
    public IActionResult Update(int id, [FromBody] object changes) =>
        Ok(new { message = "Usuario recibido para actualizar", id = id, changes = changes });

    [HttpDelete("{id}")]
    public IActionResult Delete(int id) =>
        Ok(new { message = "Usuario recibido para eliminar o desactivar", id = id });
}
```

## Equivalencias con Express

| Express | ASP.NET Core |
|---------|-------------|
| `app.get("/api/users", ...)` | `[HttpGet]` |
| `app.post("/api/users", ...)` | `[HttpPost]` |
| `app.patch("/api/users/:id", ...)` | `[HttpPatch("{id}")]` |
| `app.delete("/api/users/:id", ...)` | `[HttpDelete("{id}")]` |
| `req.params.id` | parámetro del método `int id` |
| `req.body` | `[FromBody] object userData` |
| `res.status(201).json(...)` | `StatusCode(201, ...)` |

## Métodos HTTP

| Método | Para qué sirve | Ejemplo en UserManager API |
|--------|---------------|---------------------------|
| GET | Consultar información sin modificar nada | Listar usuarios, ver perfil |
| POST | Crear un nuevo recurso | Crear usuario, registrarse |
| PATCH | Modificar parte de un recurso existente | Cambiar nombre, cambiar rol |
| DELETE | Eliminar o desactivar un recurso | Eliminar usuario |

## Pruebas realizadas

| Petición | Código esperado | Resultado |
|----------|----------------|-----------|
| GET /api/users | 200 | OK |
| GET /api/users/1 | 200 | OK |
| POST /api/users | 201 | OK |
| PATCH /api/users/1 | 200 | OK |
| DELETE /api/users/1 | 200 | OK |

## Notas importantes

- De momento las rutas son simuladas — no hay base de datos
- `Array.Empty<object>()` es necesario porque C# necesita conocer el tipo del array
- Los parámetros de ruta en C# se declaran directamente en el método, no con `req.params`
- El body se recibe con `[FromBody]` — equivale a `req.body` en Express
