# Día 11: Eliminar o desactivar usuarios (borrado lógico)

## Qué hemos hecho

- Implementado `DELETE /api/users/{id}` como **borrado lógico** (no elimina, desactiva)
- Mensaje distinto si el usuario ya estaba desactivado
- Creada ruta `PATCH /api/users/{id}/reactivate` para reactivar
- Creada ruta `GET /api/users/inactive` para listar desactivados
- Separados los modelos (`record`) a `Models/UserModels.cs`

## Borrado lógico vs borrado físico

| | Borrado físico | Borrado lógico |
|--|---------------|----------------|
| Qué hace | Elimina el registro para siempre | Marca `isActive = false` |
| Historial | Se pierde | Se conserva |
| Reversible | No | Sí (reactivando) |
| Relaciones con otros datos | Se rompen | Se mantienen |
| Uso real | Casi nunca | Lo habitual |

En aplicaciones reales casi nunca se borra un usuario de verdad: se desactiva. Así puedes conservar su historial, no romper datos relacionados y reactivarlo si hace falta.

## Endpoints trabajados

```http
DELETE /api/users/{id}            # desactiva
PATCH  /api/users/{id}/reactivate # reactiva
GET    /api/users/inactive        # lista inactivos
```

## Código del borrado lógico

```csharp
[HttpDelete("{id}")]
public IActionResult Delete(int id)
{
    var index = Users.FindIndex(u => u.Id == id);

    if (index == -1)
        return NotFound(new { error = "Usuario no encontrado", id });

    // Si ya estaba inactivo, avisamos en vez de desactivar de nuevo
    if (!Users[index].IsActive)
        return Ok(new { message = "El usuario ya estaba desactivado", data = Users[index] });

    // Borrado lógico: no eliminamos, solo desactivamos
    Users[index] = Users[index] with { IsActive = false };

    return Ok(new { message = "Usuario desactivado", data = Users[index] });
}
```

## Organización del código

Los modelos se movieron a su propio archivo para mantener el controller limpio:

```
Controllers/UsersController.cs   -> endpoints + datos en memoria
Models/UserModels.cs             -> record User, CreateUserDto, UpdateUserDto
```

> Nota: esto NO usa `partial` porque los records son tipos independientes, no parte de la clase del controller. `partial` solo se usa para dividir una misma clase en varios archivos.

## Casos probados

| Petición | Resultado |
|----------|-----------|
| DELETE /api/users/2 (activo) | Usuario desactivado |
| DELETE /api/users/2 (ya inactivo) | "El usuario ya estaba desactivado" |
| GET /api/users/inactive | Aparece el usuario 2 |
| PATCH /api/users/2/reactivate | Usuario reactivado |
| PATCH /api/users/999/reactivate | 404 |
