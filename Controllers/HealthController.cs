using Microsoft.AspNetCore.Mvc;

namespace UserManagerAPI.Controllers;

// [ApiController] activa comportamientos automáticos: validación, errores 400, etc.
// [Route] define la URL base de este controller: /api/health
[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase // ControllerBase da acceso a Ok(), NotFound(), etc.
{
    // [HttpGet] indica que este método responde a peticiones GET
    [HttpGet]
    public IActionResult Get() // IActionResult es el tipo de retorno genérico para respuestas HTTP
    {
        // Ok() devuelve un 200 con el objeto JSON como cuerpo
        return Ok(new {
            status = "ok",
            message = "UserManager API funcionando",
            timestamp = DateTime.UtcNow // fecha y hora actual en formato UTC
        });
    }
}