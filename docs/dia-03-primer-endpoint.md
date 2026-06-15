# Día 3: Primer endpoint

## Qué hemos hecho

- Creado `HealthController.cs` con el endpoint `GET /api/health`
- Devuelto una respuesta JSON con estado 200
- Añadido `timestamp` con la fecha y hora actual
- Probado la ruta desde el navegador

## Endpoint creado

```http
GET /api/health
```

## Respuesta obtenida

```json
{
  "status": "ok",
  "message": "UserManager API funcionando",
  "timestamp": "2026-06-15T10:00:00Z"
}
```

## Equivalencia con Node.js/Express

```js
// Express
app.get("/api/health", (req, res) => {
  res.status(200).json({ status: "ok", timestamp: new Date().toISOString() });
});
```

```csharp
// ASP.NET Core
[HttpGet]
public IActionResult Get()
{
    return Ok(new { status = "ok", timestamp = DateTime.UtcNow });
}
```

## Tabla comparativa de rutas

| Ruta | Método | Para qué sirve |
|------|--------|----------------|
| `/` | GET | Información general de la API |
| `/api/health` | GET | Comprobar el estado de la API |

## Pruebas realizadas

| Petición | Código esperado | Resultado |
|----------|----------------|-----------|
| GET / | 200 | OK |
| GET /api/health | 200 | OK |
| GET /api/no-existe | 404 | Not Found |
