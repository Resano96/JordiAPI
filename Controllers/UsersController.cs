using Microsoft.AspNetCore.Mvc;
using UserManagerAPI.Models;
using UserManagerAPI.Errors;

namespace UserManagerAPI.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new
    {
        message = "Listado de usuarios",
        total = Users.Count,
        data = Users
    });

    // Debe ir antes de GetById para que {id} no capture "count" ni "active"
    [HttpGet("count")]
    public IActionResult Count() =>
    Ok(new { total = Users.Count });

    [HttpGet("active")]
    public IActionResult GetActive() =>
        Ok(new { message = "Usuarios activos", data = Users.Where(u => u.IsActive) });

    [HttpGet("inactive")]
    public IActionResult GetInactive() =>
        Ok(new { message = "Usuarios inactivos", data = Users.Where(u => !u.IsActive) });

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        // FirstOrDefault busca el primer usuario con ese id, o null si no existe
        var user = Users.FirstOrDefault(u => u.Id == id);

        if (user is null)
            throw new AppException("Usuario no encontrado", 404);

        return Ok(new { message = "Usuario encontrado", data = user });
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateUserDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new AppException("El nombre es obligatorio", 400);

        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new AppException("El email es obligatorio", 400);

        if (string.IsNullOrWhiteSpace(dto.Password))
            throw new AppException("La contraseña es obligatoria", 400);

        if (dto.Password.Length < 6)
            throw new AppException("La contraseña debe tener al menos 6 caracteres", 400);

        if (!IsValidBasicEmail(dto.Email))
            throw new AppException("El email no tiene un formato válido", 400);

        var email = NormalizeEmail(dto.Email);

        if (IsEmailTaken(email))
            throw new AppException("El email ya está registrado", 409);

        var newId = Users.Count > 0 ? Users.Max(u => u.Id) + 1 : 1;
        var user = new User(newId, dto.Name.Trim(), email, dto.Role ?? "USER", true);
        Users.Add(user);

        return StatusCode(201, new { message = "Usuario creado", data = user });
    }

    [HttpPatch("{id}")]
    public IActionResult Update(int id, [FromBody] UpdateUserDto dto)
    {
        var index = Users.FindIndex(u => u.Id == id);

        if (index == -1)
            throw new AppException("Usuario no encontrado", 404);

        // Body vacío: no se envió ningún campo para cambiar
        if (dto.Name is null && dto.Email is null && dto.IsActive is null)
            throw new AppException("No se enviaron campos para actualizar", 400);

        var current = Users[index];

        // Validamos el email solo si viene en la petición
        var email = current.Email;
        if (dto.Email is not null)
        {
            if (!IsValidBasicEmail(dto.Email))
                throw new AppException("El email no tiene un formato válido", 400);

            email = NormalizeEmail(dto.Email);

            // excludeId: id -> ignoramos al propio usuario que estamos editando
            if (IsEmailTaken(email, id))
                throw new AppException("El email ya está registrado", 409);
        }

        Users[index] = current with
        {
            Name = dto.Name?.Trim() ?? current.Name,
            Email = email,
            IsActive = dto.IsActive ?? current.IsActive
        };

        return Ok(new { message = "Usuario actualizado", data = Users[index] });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var index = Users.FindIndex(u => u.Id == id);

        if (index == -1)
            throw new AppException("Usuario no encontrado", 404);

        // Si ya estaba inactivo, avisamos en vez de desactivar de nuevo
        if (!Users[index].IsActive)
            return Ok(new { message = "El usuario ya estaba desactivado", data = Users[index] });

        // Borrado lógico: no eliminamos, solo desactivamos
        Users[index] = Users[index] with { IsActive = false };

        return Ok(new { message = "Usuario desactivado", data = Users[index] });
    }

    [HttpPatch("{id}/reactivate")]
    public IActionResult Reactivate(int id)
    {
        var index = Users.FindIndex(u => u.Id == id);

        if (index == -1)
            throw new AppException("Usuario no encontrado", 404);

        if (Users[index].IsActive)
            return Ok(new { message = "El usuario ya estaba activo", data = Users[index] });

        Users[index] = Users[index] with { IsActive = true };
        return Ok(new { message = "Usuario reactivado", data = Users[index] });
    }

    private static readonly List<User> Users =
    [
        new(1, "Ana García", "ana@email.com", "USER", true),
        new(2, "Carlos Pérez", "carlos@email.com", "ADMIN", true),
        new(3, "Laura Martínez", "laura@email.com", "USER", false),
        new(4, "Pedro López", "pedro@email.com", "USER", true),
        new(5, "María Sánchez", "maria@email.com", "ADMIN", false)
    ];

    // --- Métodos auxiliares de email ---

    // Quita espacios y pasa a minúsculas
    private static string NormalizeEmail(string email) => email.Trim().ToLower();

    // Formato básico: debe tener @ y .
    private static bool IsValidBasicEmail(string email) =>
        email.Contains('@') && email.Contains('.');

    // ¿El email ya lo tiene alguien? excludeId permite ignorar al propio usuario al editar
    private static bool IsEmailTaken(string email, int? excludeId = null) =>
        Users.Any(u => u.Email == email && u.Id != excludeId);
}
