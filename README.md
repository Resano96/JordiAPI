# UserManager API

API REST de gestión de usuarios construida en C# con ASP.NET Core.
Proyecto del reto de 35 días para 2º DAM.

## Stack tecnológico

- **Lenguaje:** C#
- **Framework:** ASP.NET Core Web API (.NET 10)
- **Base de datos:** PostgreSQL (fase 4)
- **ORM:** Entity Framework Core (fase 4)
- **Autenticación:** JWT (fase 6)
- **Contenedores:** Docker Compose (fase 4)
- **Frontend:** Next.js entregado por el profesor (fase 7)

## Requisitos

- .NET 10 SDK
- Docker y Docker Compose (a partir de la fase 4)

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
| GET | `/api/users` | Listado de usuarios | ADMIN |
| GET | `/api/users/{id}` | Usuario por ID | ADMIN o propio usuario |
| GET | `/api/users/me` | Mi perfil | Autenticado |
| POST | `/api/users` | Crear usuario | ADMIN |
| PATCH | `/api/users/{id}` | Modificar usuario | ADMIN o propio usuario |
| DELETE | `/api/users/{id}` | Eliminar usuario | ADMIN |
| POST | `/api/auth/register` | Registro | Público |
| POST | `/api/auth/login` | Login | Público |
| PATCH | `/api/users/me/password` | Cambiar contraseña | Autenticado |
| PATCH | `/api/users/{id}/role` | Cambiar rol | ADMIN |
| PATCH | `/api/users/{id}/status` | Activar/desactivar | ADMIN |

> Los endpoints marcados con acceso restringido están pendientes de implementar en la fase 6.

## Modelo de datos

| Campo | Tipo | Descripción |
|-------|------|-------------|
| id | int | Identificador único |
| name | string | Nombre del usuario |
| email | string | Email único, usado para login |
| passwordHash | string | Contraseña cifrada, nunca se devuelve |
| role | string | USER o ADMIN |
| isActive | bool | Permite activar o desactivar la cuenta |
| createdAt | DateTime | Fecha de creación |
| updatedAt | DateTime | Fecha de última modificación |

## Reglas de negocio

- El email no puede repetirse
- `passwordHash` nunca se devuelve en ninguna respuesta
- Un USER solo puede ver y modificar sus propios datos
- Un usuario desactivado no puede iniciar sesión

## Variables de entorno

Pendiente de configurar en la fase 4. Se usarán en `appsettings.json`:

```
PORT=5207
DATABASE_URL=...
JWT_SECRET=...
```

## Usuario administrador inicial

Pendiente de configurar en la fase 4 (seed de base de datos).

```
email: admin@email.com
password: admin123
```

## Estructura del proyecto

```
UserManagerAPI/
  Controllers/
    HealthController.cs
    InfoController.cs
    UsersController.cs
    DebugController.cs
  docs/
  Program.cs
  UserManagerAPI.csproj
  appsettings.json
```

## Documentación del reto

- [Día 2 - Preparación del proyecto](docs/dia-02-preparacion-proyecto.md)
- [Día 3 - Primer endpoint](docs/dia-03-primer-endpoint.md)
- [Día 4 - Métodos HTTP](docs/dia-04-metodos-http.md)
- [Día 5 - JSON, body, params y headers](docs/dia-05-json-body-params-headers.md)
- [Día 6 - Cliente HTTP y depuración](docs/dia-06-cliente-http-depuracion.md)
- [Día 7 - Listado de usuarios en memoria](docs/dia-07-listado-usuarios.md)
- [Día 8 - Consultar usuario por ID](docs/dia-08-consultar-usuario-id.md)
- [Día 9 - Crear usuario](docs/dia-09-crear-usuario.md)
- [Día 10 - Actualizar usuarios](docs/dia-10-actualizar-usuarios.md)
- [Día 11 - Eliminar o desactivar usuarios](docs/dia-11-eliminar-desactivar-usuarios.md)
- [Día 12 - Validación manual básica](docs/dia-12-validacion-manual.md)
- [Día 13 - Validación de email y duplicados](docs/dia-13-validacion-email-duplicados.md)
- [Día 14 - Códigos de estado HTTP](docs/dia-14-codigos-estado-http.md)
- [Día 15 - Middleware centralizado de errores](docs/dia-15-middleware-errores.md)
