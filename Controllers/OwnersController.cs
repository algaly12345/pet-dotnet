using Microsoft.AspNetCore.Mvc;

namespace pets.Controllers
{
    public class OwnersController : Controller
    {
        public IActionResult Index() => View();
    }
}
