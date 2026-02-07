using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace pets.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        public IActionResult Index() => View();
    }
}
