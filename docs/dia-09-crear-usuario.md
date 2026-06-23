# Día 9: Crear usuarios

## Qué hemos hecho

- Implementado `POST /api/users` para añadir usuarios reales a la lista en memoria
- Creado el DTO `CreateUserDto` para recibir los datos del body
- Generado IDs automáticamente basados en el máximo existente
- Devuelto `201 Created` con el usuario creado

## Endpoint trabajado

```http
POST /api/users
Content-Type: application/json

{
  "name": "Carlos Nuevo",
  "email": "carlos.nuevo@email.com",
  "role": "USER"
}
```

## Código implementado

```csharp
[HttpPost]
public IActionResult Create([FromBody] CreateUserDto dto)
{
    // Generamos un ID nuevo basado en el máximo actual
    var newId = Users.Count > 0 ? Users.Max(u => u.Id) + 1 : 1;

    var user = new User(newId, dto.Name, dto.Email, dto.Role ?? "USER", true);
    Users.Add(user);

    return StatusCode(201, new { message = "Usuario creado", data = user });
}

public record CreateUserDto(string Name, string Email, string? Role);
```

## Qué es un DTO

DTO significa Data Transfer Object — es un objeto que define qué datos se esperan recibir.

| Sin DTO | Con DTO |
|---------|---------|
| `[FromBody] object userData` | `[FromBody] CreateUserDto dto` |
| Sin validación de campos | C# sabe exactamente qué esperar |
| Acceso por diccionario | Acceso por propiedades tipadas |

## Equivalencia con Express

| Express | C# |
|---------|-----|
| `const userData = req.body` | `[FromBody] CreateUserDto dto` |
| `users.push(newUser)` | `Users.Add(user)` |
| `res.status(201).json(...)` | `StatusCode(201, ...)` |
| `dto.role \|\| "USER"` | `dto.Role ?? "USER"` |

## Respuesta obtenida

```json
{
  "message": "Usuario creado",
  "data": {
    "id": 6,
    "name": "Carlos Nuevo",
    "email": "carlos.nuevo@email.com",
    "role": "USER",
    "isActive": true
  }
}
```

## Notas importantes

- El `role` es opcional en el DTO (`string?`) — si no se envía, se asigna `"USER"` por defecto
- `??` es el operador null-coalescing: devuelve el valor de la derecha si el de la izquierda es null
- Los datos se pierden al reiniciar el servidor — esto se resolverá en la fase de base de datos
