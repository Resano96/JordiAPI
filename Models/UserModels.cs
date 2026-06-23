namespace UserManagerAPI.Models;

// Modelo principal de usuario
record User(int Id, string Name, string Email, string Role, bool IsActive)
{
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; } = DateTime.UtcNow;
}

// DTO para crear un usuario — Role es opcional
public record CreateUserDto(string? Name, string? Email, string? Password, string? Role);

// DTO para actualizar — todos los campos opcionales (PATCH)
public record UpdateUserDto(string? Name, string? Email, bool? IsActive);
