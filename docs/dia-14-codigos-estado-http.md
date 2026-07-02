# Día 14: Códigos de estado HTTP

## Qué hemos hecho

- Revisado que cada endpoint devuelve el código HTTP correcto
- Identificado y razonado una posible incoherencia de diseño
- No ha hecho falta cambiar código — la API ya era coherente

## Familias de códigos

| Familia | Significado | Quién tiene la culpa |
|---------|-------------|---------------------|
| 2xx | Todo correcto | — |
| 4xx | Error del cliente (datos mal, no autorizado...) | El cliente |
| 5xx | Error del servidor | El servidor |

## Códigos usados en la API

| Código | Cuándo | Ejemplo |
|--------|--------|---------|
| 200 OK | Consulta/actualización correcta | GET, PATCH, DELETE |
| 201 Created | Recurso creado | POST /api/users |
| 400 Bad Request | Datos incorrectos o incompletos | password corta, email sin @ |
| 404 Not Found | El recurso no existe | GET /api/users/999 |
| 409 Conflict | Choca con regla de negocio | email duplicado |
| 401 / 403 | Autenticación / permisos | (fase 6, aún no) |

## Revisión de endpoints

| Endpoint | Caso | Código |
|----------|------|--------|
| GET /api/users | Listar | 200 |
| GET /api/users/{id} | Existe | 200 |
| GET /api/users/{id} | No existe | 404 |
| GET /api/users/{id} | No numérico | 400 (automático) |
| POST /api/users | Creado | 201 |
| POST /api/users | Datos mal | 400 |
| POST /api/users | Email duplicado | 409 |
| PATCH /api/users/{id} | Actualizado | 200 |
| PATCH /api/users/{id} | No existe | 404 |
| DELETE /api/users/{id} | Desactivado | 200 |

## Diferencia entre 400, 404 y 409

- **400** — la petición está mal hecha (faltan datos, formato inválido). Se arregla cambiando lo que envías.
- **404** — lo que pides no existe. La petición está bien, pero el recurso no está.
- **409** — la petición está bien, pero choca con el estado actual (email ya registrado). Es un conflicto de reglas, no de formato.

## Decisión de diseño: estado ya aplicado

En `DELETE` y `PATCH .../reactivate`, si el usuario ya estaba en el estado pedido, devolvemos **200** con un mensaje aclaratorio:

```json
{ "message": "El usuario ya estaba desactivado", "data": { ... } }
```

Lo dejamos en 200 (no 409) de forma consciente: la operación no falla, simplemente el usuario ya está en ese estado. Es una decisión defendible — algunas APIs usarían 409 aquí.

## 401 vs 403 (para la fase 6)

- **401 Unauthorized** — no sabemos quién eres (falta token o es inválido)
- **403 Forbidden** — sabemos quién eres, pero no tienes permiso (ej. un USER intentando entrar a una zona de ADMIN)
