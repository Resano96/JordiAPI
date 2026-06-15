using Microsoft.AspNetCore.Mvc;

namespace UserManagerAPI.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new
    {
        message = "Listado de usuarios",
        data = Array.Empty<object>() // array vacío tipado — C# necesita saber el tipo
    });

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        return Ok(new { message = "Detalle de usuario", 
            id = id 
        });
    }

    [HttpPost]
    public IActionResult Create([FromBody] object userData)
    {
        return StatusCode(201, new { message = "Usuario recibido para crear", data = userData });
    }

    [HttpPatch("{id}")]
    public IActionResult Update(int id, [FromBody] object changes)
    {
        return Ok(new { message = "Usuario recibido para actualizar", id = id, changes = changes });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        return Ok(new { message = "Usuario recibido para eliminar o desactivar", id = id });
    }
}

