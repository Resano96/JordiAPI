using Microsoft.AspNetCore.Mvc;

namespace UserManagerAPI.Controllers
{
    public class DebugController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
