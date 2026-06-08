using Microsoft.AspNetCore.Mvc;

namespace UserManagerAPI.Controllers;

// [ApiController] activa comportamientos automáticos: validación, errores 400, etc.
// [Route] define la URL base de este controller: /api/health
[ApiController]
[Route("/")]
public class InfoController : ControllerBase // ControllerBase da acceso a Ok(), NotFound(), etc.
{
    // [HttpGet] indica que este método responde a peticiones GET
    [HttpGet]
    public IActionResult Get() // IActionResult es el tipo de retorno genérico para respuestas HTTP
    {
        // Ok() devuelve un 200 con el objeto JSON como cuerpo
        return Ok(new {
            name = "UserManager API",
            version = "1.0.0",
            status = "running",
            author = "Ander Resano"
        });
    }
}