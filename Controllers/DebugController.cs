using Microsoft.AspNetCore.Mvc;

namespace UserManagerAPI.Controllers;

[ApiController]
[Route("api/debug")]

public class DebugController : Controller
{
    [HttpPost("body")]
    public IActionResult TestBody([FromBody] object body) =>
        Ok(new { message = "Body recibido correctamente", body });

    [HttpGet("params/{id}")]
    public IActionResult TestParams(int id) =>
    Ok(new { message = "Params recibidos correctamente", id });

    [HttpGet("query")]
    public IActionResult TestQuery(
        [FromQuery] string? role, 
        [FromQuery] bool? isActive) =>
    Ok(new { message = "Query params recibidos", role, isActive });

    [HttpGet("headers")]
    public IActionResult TestHeaders() =>
    Ok(new { message = "Headers recibidos", authorization = Request.Headers["Authorization"].ToString() });

    [HttpPatch("users/{id}")]
    public IActionResult TestCombined(
        int id, 
        [FromQuery] bool? notify, 
        [FromBody] object changes) =>
    Ok(new
    {
        message = "Datos combinados recibidos",
        id,
        notify,
        authorization = Request.Headers["Authorization"].ToString(),
        changes
    });

    [HttpPost("request")]
    public IActionResult TestRequest([FromQuery] string? source, [FromBody] object? body) =>
    Ok(new
    {
        message = "Información completa de la petición",
        method = Request.Method,
        path = Request.Path.ToString(),
        query = source,
        authorization = Request.Headers["Authorization"].ToString(),
        clientName = Request.Headers["x-client-name"].ToString(),
        body
    });

}
