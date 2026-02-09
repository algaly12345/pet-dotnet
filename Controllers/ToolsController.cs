using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pets.Data;
using pets.Models;
using pets.ViewModels;

namespace pets.Controllers
{
    [Authorize] // عشان غير المسجل ما يدخل
    public class ToolsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ToolsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /Tools
        public async Task<IActionResult> Index()
        {
            var list = await _db.Tools
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return View(list); // هنا View يستقبل List<Tool>
        }

        // GET: /Tools/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new ToolCreateVm()); // ✅ نفس نوع الموديل في Create.cshtml
        }

        // POST: /Tools/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ToolCreateVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm); // ✅ يرجع نفس vm

            var entity = new Tool
            {
                Name = vm.Name,
                Description = vm.Description,
                IsActive = vm.IsActive
            };

            _db.Tools.Add(entity);
            await _db.SaveChangesAsync();

            TempData["Success"] = "تمت الإضافة بنجاح ✅";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Tools/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _db.Tools.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            var vm = new ToolCreateVm
            {
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            ViewBag.Id = id; // عشان نستخدمه في الفورم
            return View(vm); // ✅ Edit.cshtml لازم يكون ToolCreateVm
        }

        // POST: /Tools/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ToolCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Id = id;
                return View(vm);
            }

            var entity = await _db.Tools.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            entity.Name = vm.Name;
            entity.Description = vm.Description;
            entity.IsActive = vm.IsActive;

            await _db.SaveChangesAsync();

            TempData["Success"] = "تم التعديل بنجاح ✅";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Tools/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.Tools.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            _db.Tools.Remove(entity);
            await _db.SaveChangesAsync();

            TempData["Success"] = "تم الحذف بنجاح ✅";
            return RedirectToAction(nameof(Index));
        }
    }
}
