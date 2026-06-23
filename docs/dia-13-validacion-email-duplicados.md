# Día 13: Validación de email y duplicados

## Qué hemos hecho

- Refactorizado la lógica de email en tres métodos auxiliares reutilizables
- Reforzado la validación de formato: ahora exige `@` **y** `.`
- Eliminado código duplicado entre `POST` y `PATCH`

## Métodos auxiliares

```csharp
// Quita espacios y pasa a minúsculas
private static string NormalizeEmail(string email) => email.Trim().ToLower();

// Formato básico: debe tener @ y .
private static bool IsValidBasicEmail(string email) =>
    email.Contains('@') && email.Contains('.');

// ¿El email ya lo tiene alguien? excludeId ignora al propio usuario al editar
private static bool IsEmailTaken(string email, int? excludeId = null) =>
    Users.Any(u => u.Email == email && u.Id != excludeId);
```

## Por qué extraer funciones auxiliares

Antes la misma lógica de email estaba escrita dos veces (en `POST` y en `PATCH`). Eso es un problema:

- Si cambias una regla, tienes que acordarte de cambiarla en los dos sitios
- Es más fácil que se queden inconsistentes
- El código del endpoint es más difícil de leer

Sacándola a métodos con nombre (`IsValidBasicEmail`, `IsEmailTaken`) el endpoint se lee casi como una frase: "si el email no es válido, error; si está cogido, conflicto". Esto es el principio **DRY** (Don't Repeat Yourself).

## El parámetro excludeId

```csharp
private static bool IsEmailTaken(string email, int? excludeId = null)
```

- En el **POST** lo llamamos sin `excludeId` → comprueba contra todos los usuarios
- En el **PATCH** lo llamamos con `excludeId: id` → ignora al usuario que estamos editando, para que pueda guardar su propio email sin chocar consigo mismo

`int?` significa que el parámetro es opcional y puede ser nulo. El `= null` le da un valor por defecto, así no es obligatorio pasarlo.

## Normalización

Normalizar es transformar el dato a un formato estándar antes de guardarlo o compararlo:

| Entrada | Normalizado |
|---------|-------------|
| `"  Ana@Email.COM "` | `"ana@email.com"` |

Así `Ana@email.com` y `ana@email.com` se consideran el mismo email y no se duplican.

## Qué es el 409 Conflict

El `409` indica que la petición es válida en formato, pero choca con una **regla de negocio** del sistema. Aquí: el email ya existe. No es culpa del formato (eso sería 400), sino del estado actual de los datos.

## Casos probados

| Petición | Código |
|----------|--------|
| POST email sin `.` (ej. `test@email`) | 400 |
| POST email duplicado | 409 |
| POST correcto | 201 |
| PATCH email de otro usuario | 409 |
| PATCH mismo email del propio usuario | 200 |
