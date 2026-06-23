# Día 8: Consultar usuario por ID

## Qué hemos hecho

- Actualizado `GET /api/users/{id}` para buscar en la lista real
- Devuelto `404` cuando el usuario no existe
- Probado casos válidos, inexistentes e inválidos

## Endpoint trabajado

```http
GET /api/users/{id}
```

## Código implementado

```csharp
[HttpGet("{id}")]
public IActionResult GetById(int id)
{
    // FirstOrDefault busca el primer usuario con ese id, o null si no existe
    var user = Users.FirstOrDefault(u => u.Id == id);

    if (user is null)
        return NotFound(new { error = "Usuario no encontrado", id });

    return Ok(new { message = "Usuario encontrado", data = user });
}
```

## Ventaja de C# sobre Express

En Express el ID llega como string y hay que convertirlo manualmente:
```js
const id = Number(req.params.id);
if (Number.isNaN(id)) return res.status(400).json({ error: "..." });
```

En C# declaramos `int id` y ASP.NET Core convierte y valida automáticamente.
Si llega `abc`, el framework devuelve `400` solo, sin código extra.

## Casos probados

| Petición | Código esperado | Resultado |
|----------|----------------|-----------|
| GET /api/users/1 | 200 | OK |
| GET /api/users/2 | 200 | OK |
| GET /api/users/999 | 404 | OK |
| GET /api/users/abc | 400 | OK (automático) |

## Tareas libres completadas

### Error 404 con ID incluido

```json
{ "error": "Usuario no encontrado", "id": 999 }
```

### GET /api/users/active

Ruta colocada antes de `GET /api/users/{id}` para evitar que `active` sea interpretado como ID.

```csharp
[HttpGet("active")]
public IActionResult GetActive() =>
    Ok(new { message = "Usuarios activos", data = Users.Where(u => u.IsActive) });
```

### Orden de rutas en ASP.NET Core

ASP.NET Core usa atributos en lugar de registrar rutas en orden, pero el principio es el mismo: las rutas específicas como `count` o `active` deben definirse antes de las dinámicas `{id}`, ya que de lo contrario el framework podría enrutar `active` como si fuera un ID numérico y devolver `400`.

## Equivalencias

| Express | C# |
|---------|-----|
| `users.find(u => u.id === id)` | `Users.FirstOrDefault(u => u.Id == id)` |
| `if (!user)` | `if (user is null)` |
| `res.status(404).json(...)` | `NotFound(...)` |
| `res.status(200).json(...)` | `Ok(...)` |
