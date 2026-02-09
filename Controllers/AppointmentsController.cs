using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pets.Security;

[Authorize(Policy = Permissions.Appointments_View)]
public class AppointmentsController : Controller
{
    public IActionResult Index() => View();

    [Authorize(Policy = Permissions.Appointments_Create)]
    public IActionResult Create() => View();

    [HttpPost]
    [Authorize(Policy = Permissions.Appointments_Create)]
    public IActionResult Create(object vm) => RedirectToAction("Index");

    [Authorize(Policy = Permissions.Appointments_Edit)]
    public IActionResult Edit(int id) => View();

    [Authorize(Policy = Permissions.Appointments_Delete)]
    public IActionResult Delete(int id) => View();
}
