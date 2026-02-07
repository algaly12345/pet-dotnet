using Microsoft.AspNetCore.Mvc;

namespace pets.Controllers
{
    public class AppointmentsController : Controller
    {
        public IActionResult Index() => View();
    }
}
