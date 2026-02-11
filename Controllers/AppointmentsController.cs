using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pets.Data;
using pets.Models;
using pets.ViewModels;

namespace pets.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AppointmentsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /Appointments/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new AppointmentFormVm
            {
                StartAt = DateTime.Now
            };

            await FillDropdowns(vm);
            return View(vm);
        }

        // POST: /Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentFormVm vm)
        {
            if (!ModelState.IsValid)
            {
                await FillDropdowns(vm);
                return View(vm);
            }

            // نجيب أسماء للحقول (اختياري لكن انت تستخدمها)
            var owner = await _db.Owners.FirstOrDefaultAsync(x => x.Id == vm.OwnerId);
            var pet = await _db.Pets.FirstOrDefaultAsync(x => x.Id == vm.PetId);
            var doctor = string.IsNullOrWhiteSpace(vm.DoctorUserId)
                ? null
                : await _db.Users.FirstOrDefaultAsync(u => u.Id == vm.DoctorUserId);

            var entity = new Appointment
            {
                OwnerId = vm.OwnerId,
                PetId = vm.PetId,
                DoctorUserId = vm.DoctorUserId,
                StartAt = vm.StartAt,
                EndAt = vm.EndAt,
                Reason = vm.Reason,
                Notes = vm.Notes,
                Status = vm.Status,

                OwnerName = owner?.FullName ?? "",
                PetName = pet?.Name ?? "",
                DoctorName = doctor?.FullName ?? ""
            };

            _db.Appointments.Add(entity);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Appointments/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var a = await _db.Appointments.FirstOrDefaultAsync(x => x.Id == id);
            if (a == null) return NotFound();

            var vm = new AppointmentFormVm
            {
                Id = a.Id,
                OwnerId = a.OwnerId ?? 0,
                PetId = a.PetId ?? 0,
                DoctorUserId = a.DoctorUserId,
                StartAt = a.StartAt,
                EndAt = a.EndAt,
                Reason = a.Reason,
                Notes = a.Notes,
                Status = a.Status
            };

            await FillDropdowns(vm);
            return View(vm);
        }

        // POST: /Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AppointmentFormVm vm)
        {
            if (id != vm.Id) return BadRequest();

            var a = await _db.Appointments.FirstOrDefaultAsync(x => x.Id == id);
            if (a == null) return NotFound();

            if (!ModelState.IsValid)
            {
                await FillDropdowns(vm);
                return View(vm);
            }

            var owner = await _db.Owners.FirstOrDefaultAsync(x => x.Id == vm.OwnerId);
            var pet = await _db.Pets.FirstOrDefaultAsync(x => x.Id == vm.PetId);
            var doctor = string.IsNullOrWhiteSpace(vm.DoctorUserId)
                ? null
                : await _db.Users.FirstOrDefaultAsync(u => u.Id == vm.DoctorUserId);

            a.OwnerId = vm.OwnerId;
            a.PetId = vm.PetId;
            a.DoctorUserId = vm.DoctorUserId;
            a.StartAt = vm.StartAt;
            a.EndAt = vm.EndAt;
            a.Reason = vm.Reason;
            a.Notes = vm.Notes;
            a.Status = vm.Status;

            a.OwnerName = owner?.FullName ?? "";
            a.PetName = pet?.Name ?? "";
            a.DoctorName = doctor?.FullName ?? "";

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Index (مبدئي)
        public async Task<IActionResult> Index()
        {
            var list = await _db.Appointments
                .OrderByDescending(x => x.StartAt)
                .ToListAsync();

            return View(list); // إذا View عندك AppointmentsIndexVm قولي عشان أضبطها
        }

        private async Task FillDropdowns(AppointmentFormVm vm)
        {
            vm.Owners = await _db.Owners
                .OrderBy(x => x.FullName)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.FullName })
                .ToListAsync();

            vm.Pets = await _db.Pets
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
                .ToListAsync();

            // الدكتور = users
            vm.Doctors = await _db.Users
                .OrderBy(x => x.FullName)
                .Select(x => new SelectListItem { Value = x.Id, Text = x.FullName })
                .ToListAsync();
        }
    }
}
