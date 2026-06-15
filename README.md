# UserManager API

API REST de gestión de usuarios construida en C# con ASP.NET Core.
Proyecto del reto de 35 días para 2º DAM.

## Requisitos

- .NET 10 SDK
- Docker y Docker Compose (fases posteriores)

## Instalación

```bash
git clone https://github.com/Resano96/JordiAPI.git
cd UserManagerAPI
dotnet run
```

## Desarrollo con recarga automática

```bash
dotnet watch
```

## URL base

```
http://localhost:5207
```

## Endpoints disponibles

| Método | Ruta | Descripción | Acceso |
|--------|------|-------------|--------|
| GET | `/` | Información de la API | Público |
| GET | `/api/health` | Estado de la API | Público |

## Variables de entorno

Próximamente en fases posteriores del reto.

## Usuario administrador inicial

Próximamente en la fase de base de datos.

## Documentación del reto

- [Día 2 - Preparación del proyecto](docs/dia-02-preparacion-proyecto.md)
- [Día 3 - Primer endpoint](docs/dia-03-primer-endpoint.md)
- [Día 4 - Métodos HTTP](docs/dia-04-metodos-http.md)
- [Día 5 - JSON, body, params y headers](docs/dia-05-json-body-params-headers.md)
