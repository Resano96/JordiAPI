# Día 7: Listado de usuarios en memoria

## Qué hemos hecho

- Creado el modelo `User` como `record` de C#
- Creado una lista estática de 5 usuarios en memoria dentro de `UsersController`
- Actualizado `GET /api/users` para devolver los usuarios reales
- Añadido el campo `total` en la respuesta
- Creado `GET /api/users/count` para obtener solo el total
- Creado `GET /api/users/active` para listar solo usuarios activos

## Endpoint trabajado

```http
GET /api/users
```

## Respuesta obtenida

```json
{
  "message": "Listado de usuarios",
  "total": 3,
  "data": [
    { "id": 1, "name": "Ana García", "email": "ana@email.com", "role": "USER", "isActive": true },
    { "id": 2, "name": "Carlos Pérez", "email": "carlos@email.com", "role": "ADMIN", "isActive": true },
    { "id": 3, "name": "Laura Martínez", "email": "laura@email.com", "role": "USER", "isActive": false }
  ]
}
```

## Modelo User en C#

```csharp
// record es la forma más compacta de definir un modelo de datos en C#
// equivale a una clase con propiedades, constructor y equals automáticos
record User(int Id, string Name, string Email, string Role, bool IsActive)
{
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; } = DateTime.UtcNow;
}
```

## Equivalencia con TypeScript

| TypeScript | C# |
|------------|-----|
| `type User = { id: number; ... }` | `record User(int Id, ...)` |
| `const users: User[] = [...]` | `private static readonly List<User> Users = [...]` |
| `users.length` | `Users.Count` |

## Memoria vs base de datos

| | Memoria | Base de datos |
|--|---------|--------------|
| Persistencia | Se pierde al reiniciar | Se guarda siempre |
| Configuración | Ninguna | Docker, migraciones, ORM |
| Uso | Aprender la lógica | Producción real |

Los datos en memoria son temporales — si se para el servidor, los cambios se pierden y vuelve el array inicial. En la fase 4 sustituiremos esto por PostgreSQL con Entity Framework Core.

## Tareas libres completadas

### Usuarios en memoria (5 usuarios)

```csharp
new(1, "Ana García", "ana@email.com", "USER", true),
new(2, "Carlos Pérez", "carlos@email.com", "ADMIN", true),
new(3, "Laura Martínez", "laura@email.com", "USER", false),
new(4, "Pedro López", "pedro@email.com", "USER", true),
new(5, "María Sánchez", "maria@email.com", "ADMIN", false)
```

### GET /api/users/count

```json
{ "total": 5 }
```

### GET /api/users/active

Devuelve solo los usuarios con `isActive: true`.
Usa `Users.Where(u => u.IsActive)` — equivale a `users.filter(u => u.isActive)` en JS.

> Estas rutas van **antes** de `GET /api/users/{id}` para que ASP.NET no interprete `count` o `active` como un ID.

## Comprobaciones

| Comprobación | Resultado |
|-------------|-----------|
| GET /api/users responde | OK |
| Status code 200 | OK |
| Respuesta contiene `total` | OK |
| Respuesta contiene `data` | OK |
| `data` es un array | OK |
| No se devuelve contraseña | OK |
