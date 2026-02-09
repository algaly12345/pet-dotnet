using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pets.Models;
using pets.Security;
using pets.ViewModels;

namespace pets.Controllers
{
    [Authorize] // لازم يكون مسجل دخول
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: /Users
        [Authorize(Policy = Permissions.Roles_View)]
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var list = new List<dynamic>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                list.Add(new
                {
                    u.Id,
                    u.UserName,
                    u.FullName,
                    u.IsActive,
                    Role = roles.FirstOrDefault() ?? "-"
                });
            }

            return View(list);
        }

        // GET: /Users/Create
        [Authorize(Policy = Permissions.Roles_Edit)]
        public IActionResult Create()
        {
            ViewBag.Roles = _roleManager.Roles.Select(r => r.Name!).ToList();
            return View(new UserCreateVm());
        }

        // POST: /Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Roles_Edit)]
        public async Task<IActionResult> Create(UserCreateVm vm)
        {
            ViewBag.Roles = _roleManager.Roles.Select(r => r.Name!).ToList();

            if (!ModelState.IsValid) return View(vm);

            if (!await _roleManager.RoleExistsAsync(vm.RoleName))
            {
                ModelState.AddModelError("", "الدور المحدد غير موجود.");
                return View(vm);
            }

            var user = new User
            {
                UserName = vm.UserName,
                FullName = vm.FullName,
                IsActive = vm.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            var createResult = await _userManager.CreateAsync(user, vm.Password);
            if (!createResult.Succeeded)
            {
                foreach (var e in createResult.Errors)
                    ModelState.AddModelError("", e.Description);
                return View(vm);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, vm.RoleName);
            if (!roleResult.Succeeded)
            {
                foreach (var e in roleResult.Errors)
                    ModelState.AddModelError("", e.Description);
                return View(vm);
            }

            TempData["Success"] = "تم إنشاء المستخدم بنجاح ✅";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Users/Edit/{id}
        [Authorize(Policy = Permissions.Roles_Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            var currentRole = roles.FirstOrDefault() ?? "";

            ViewBag.Roles = _roleManager.Roles.Select(r => r.Name!).ToList();

            var vm = new UserEditVm
            {
                Id = user.Id,
                UserName = user.UserName ?? "",
                FullName = user.FullName,
                IsActive = user.IsActive,
                RoleName = currentRole
            };

            return View(vm);
        }

        // POST: /Users/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Roles_Edit)]
        public async Task<IActionResult> Edit(UserEditVm vm)
        {
            ViewBag.Roles = _roleManager.Roles.Select(r => r.Name!).ToList();

            if (!ModelState.IsValid) return View(vm);

            var user = await _userManager.FindByIdAsync(vm.Id);
            if (user == null) return NotFound();

            // تحديث بيانات
            user.UserName = vm.UserName;
            user.FullName = vm.FullName;
            user.IsActive = vm.IsActive;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var e in updateResult.Errors)
                    ModelState.AddModelError("", e.Description);
                return View(vm);
            }

            // تحديث الدور (Role واحد)
            if (!string.IsNullOrWhiteSpace(vm.RoleName))
            {
                if (!await _roleManager.RoleExistsAsync(vm.RoleName))
                {
                    ModelState.AddModelError("", "الدور المحدد غير موجود.");
                    return View(vm);
                }

                var oldRoles = await _userManager.GetRolesAsync(user);
                if (oldRoles.Any())
                    await _userManager.RemoveFromRolesAsync(user, oldRoles);

                var addRoleResult = await _userManager.AddToRoleAsync(user, vm.RoleName);
                if (!addRoleResult.Succeeded)
                {
                    foreach (var e in addRoleResult.Errors)
                        ModelState.AddModelError("", e.Description);
                    return View(vm);
                }
            }

            // تغيير كلمة المرور (اختياري)
            if (!string.IsNullOrWhiteSpace(vm.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var reset = await _userManager.ResetPasswordAsync(user, token, vm.NewPassword);
                if (!reset.Succeeded)
                {
                    foreach (var e in reset.Errors)
                        ModelState.AddModelError("", e.Description);
                    return View(vm);
                }
            }

            TempData["Success"] = "تم تعديل المستخدم بنجاح ✅";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Users/Delete/{id}
        [Authorize(Policy = Permissions.Roles_Edit)]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: /Users/DeleteConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Roles_Edit)]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            // (اختياري) منع حذف نفسك
            if (User?.Identity?.Name == user.UserName)
            {
                TempData["Error"] = "لا يمكنك حذف حسابك الحالي.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                TempData["Error"] = string.Join(" | ", result.Errors.Select(e => e.Description));
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "تم حذف المستخدم بنجاح ✅";
            return RedirectToAction(nameof(Index));
        }
    }
}
