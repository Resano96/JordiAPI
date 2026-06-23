# Día 12: Validación manual básica

## Qué hemos hecho

- Añadido `Password` al `CreateUserDto`
- Validado en `POST /api/users`: campos obligatorios, longitud de contraseña, formato de email
- Detectado emails duplicados con `409 Conflict`
- Normalizado datos (trim del nombre, email en minúsculas)
- Añadido las mismas validaciones al `PATCH /api/users/{id}`

## Reglas de validación

| Regla | Código si falla |
|-------|----------------|
| name obligatorio | 400 |
| email obligatorio | 400 |
| password obligatoria | 400 |
| password mínimo 6 caracteres | 400 |
| email contiene `@` | 400 |
| email único | 409 |

## Validación en el POST

```csharp
// string.IsNullOrWhiteSpace cubre null, "" y "   " (solo espacios)
if (string.IsNullOrWhiteSpace(dto.Name))
    return BadRequest(new { error = "El nombre es obligatorio" });

if (dto.Password.Length < 6)
    return BadRequest(new { error = "La contraseña debe tener al menos 6 caracteres" });

if (!dto.Email.Contains('@'))
    return BadRequest(new { error = "El email no tiene un formato válido" });

// Normalización
var email = dto.Email.Trim().ToLower();

// Duplicado
if (Users.Any(u => u.Email == email))
    return Conflict(new { error = "El email ya está registrado" });
```

## Validación de duplicado en el PATCH

La diferencia con el POST: al actualizar, el email no puede pertenecer a **otro** usuario distinto del que editamos.

```csharp
if (Users.Any(u => u.Id != id && u.Email == email))
    return Conflict(new { error = "El email ya está registrado" });
```

Sin el `u.Id != id`, un usuario no podría guardar su propio email sin cambiarlo (chocaría consigo mismo).

## Equivalencias con JavaScript

| JavaScript | C# |
|------------|-----|
| `!name \|\| name.trim() === ""` | `string.IsNullOrWhiteSpace(name)` |
| `email.includes("@")` | `email.Contains('@')` |
| `email.trim().toLowerCase()` | `email.Trim().ToLower()` |
| `users.some(u => u.email === email)` | `Users.Any(u => u.Email == email)` |

## Por qué validar en el backend

Aunque el frontend valide los datos, el backend **siempre** debe volver a validar. El cliente se puede manipular (Postman, scripts, navegador modificado), así que el backend es la última barrera antes de tocar los datos.

## Casos probados

| Petición | Código |
|----------|--------|
| POST body vacío | 400 |
| POST password corta | 400 |
| POST email sin @ | 400 |
| POST email duplicado | 409 |
| POST datos correctos | 201 |
| PATCH body vacío | 400 |
| PATCH email de otro usuario | 409 |
| PATCH correcto | 200 |
