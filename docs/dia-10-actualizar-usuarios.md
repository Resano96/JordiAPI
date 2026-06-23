# Día 10: Actualizar usuarios

## Qué hemos hecho

- Implementado `PATCH /api/users/{id}` para modificar usuarios reales
- Creado el DTO `UpdateUserDto` con todos los campos opcionales
- Actualizado solo los campos enviados, manteniendo los demás
- Devuelto `404` si el usuario no existe

## Endpoint trabajado

```http
PATCH /api/users/1
Content-Type: application/json

{
  "name": "Ana Actualizada"
}
```

## Código implementado

```csharp
[HttpPatch("{id}")]
public IActionResult Update(int id, [FromBody] UpdateUserDto dto)
{
    var index = Users.FindIndex(u => u.Id == id);

    if (index == -1)
        return NotFound(new { error = "Usuario no encontrado", id });

    // Actualizamos solo los campos enviados, manteniendo los demás
    var current = Users[index];
    Users[index] = current with
    {
        Name = dto.Name ?? current.Name,
        Email = dto.Email ?? current.Email,
        IsActive = dto.IsActive ?? current.IsActive
    };

    return Ok(new { message = "Usuario actualizado", data = Users[index] });
}

public record UpdateUserDto(string? Name, string? Email, bool? IsActive);
```

## El operador `with` de los records

`with` crea una **copia** del record cambiando solo los campos indicados. Es la forma idiomática de C# para el "spread operator" de JavaScript.

| JavaScript | C# |
|------------|-----|
| `{ ...user, name: "Nuevo" }` | `user with { Name = "Nuevo" }` |
| `users.findIndex(u => u.id === id)` | `Users.FindIndex(u => u.Id == id)` |
| `dto.name ?? current.name` | `dto.Name ?? current.Name` |

## Campos modificables

| Campo | ¿Modificable aquí? |
|-------|-------------------|
| name | Sí |
| email | Sí |
| isActive | Sí |
| id | No (es la identidad del usuario) |
| role | No (se cambia desde ruta de ADMIN) |
| createdAt | No (fecha fija) |

## Por qué PATCH y no PUT

- **PATCH** modifica solo una parte del recurso — por eso los campos del DTO son opcionales (`string?`, `bool?`)
- **PUT** reemplazaría el recurso completo, obligando a enviar todos los campos

Como solo enviamos los campos que queremos cambiar, PATCH es el método correcto.

## Casos probados

| Petición | Código esperado | Resultado |
|----------|----------------|-----------|
| PATCH /api/users/1 con name | 200 | OK |
| PATCH /api/users/999 | 404 | OK |
