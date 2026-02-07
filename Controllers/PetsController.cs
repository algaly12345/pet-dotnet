using Microsoft.AspNetCore.Mvc;

namespace pets.Controllers
{
    public class PetsController : Controller
    {
        public IActionResult Index() => View();
    }
}
