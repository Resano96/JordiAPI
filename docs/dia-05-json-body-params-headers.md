# Día 5: JSON, body, params y headers

## Qué hemos hecho

- Creado `DebugController.cs` con rutas temporales para practicar
- Probado body con `POST /api/debug/body`
- Probado params con `GET /api/debug/params/{id}`
- Probado query params con `GET /api/debug/query`
- Probado headers con `GET /api/debug/headers`
- Probado todo combinado con `PATCH /api/debug/users/{id}`

## Rutas trabajadas

```http
POST   /api/debug/body
GET    /api/debug/params/{id}
GET    /api/debug/query
GET    /api/debug/headers
PATCH  /api/debug/users/{id}
```

## Equivalencias con Express

| Express | ASP.NET Core |
|---------|-------------|
| `req.body` | `[FromBody] object body` |
| `req.params.id` | parámetro del método `int id` |
| `req.query.role` | `[FromQuery] string? role` |
| `req.headers["authorization"]` | `Request.Headers["Authorization"]` |

## Dónde viaja cada dato

| Dato | ¿Dónde viaja? | Ejemplo |
|------|--------------|---------|
| ID de usuario | Route params | `/api/users/1` |
| Email de registro | Body | `{ "email": "..." }` |
| Filtro por rol | Query params | `?role=ADMIN` |
| Token JWT | Headers | `Authorization: Bearer token` |
| Nueva contraseña | Body | `{ "newPassword": "..." }` |

## Pruebas realizadas

| Petición | Dato probado | Código esperado | Resultado |
|----------|-------------|----------------|-----------|
| POST /api/debug/body | Body | 200 | OK |
| GET /api/debug/params/25 | Params | 200 | OK |
| GET /api/debug/query?role=ADMIN&isActive=true | Query params | 200 | OK |
| GET /api/debug/headers | Headers | 200 | OK |
| PATCH /api/debug/users/7?notify=true | Combinado | 200 | OK |

## Notas importantes

- En C# los query params se leen con `[FromQuery]` — no existe `req.query`
- `string?` y `bool?` con `?` significan que el parámetro es opcional (nullable)
- Los headers se leen con `Request.Headers["nombre"]` dentro del controller
- Los route params en C# ya llegan tipados — si declaras `int id`, C# convierte el texto automáticamente
