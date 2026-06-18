# Día 6: Cliente HTTP y depuración

## Qué hemos hecho

- Añadida ruta `POST /api/debug/request` que muestra toda la información de la petición
- Probado errores controlados: ruta inexistente, método incorrecto
- Usado el archivo `.http` para organizar y reutilizar las pruebas

## Ruta de depuración completa

```http
POST http://localhost:5207/api/debug/request?source=thunder
Content-Type: application/json
Authorization: Bearer token-de-prueba
x-client-name: thunder-client

{
  "example": "datos de prueba"
}
```

## Cómo depurar cuando algo falla

1. ¿Está arrancado el servidor? (`dotnet watch`)
2. ¿El puerto es correcto? (5207)
3. ¿La ruta existe?
4. ¿El método HTTP es correcto?
5. ¿El body tiene `Content-Type: application/json`?
6. ¿Qué código de estado devuelve?
7. ¿Qué dice la terminal?

## Errores provocados

| Petición | Qué probamos | Resultado |
|----------|-------------|-----------|
| GET /api/ruta-inventada | Ruta inexistente | 404 |
| POST /api/health | Método incorrecto | 405 |
| JSON con coma extra | Body mal formado | 400 |

## Comparación de herramientas

| Herramienta | Ventajas | Limitaciones |
|-------------|----------|-------------|
| Navegador | Rápido para GET simples | No permite POST, headers ni body |
| Archivo .http (VS) | Integrado en el editor, sin instalar nada | Menos visual |
| Postman | Muy completo, colecciones, entornos | Aplicación externa |
