using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pets.Security;
using pets.ViewModels;
using System.Security.Claims;

namespace pets.Controllers
{
    [Authorize] // لازم يكون مسجل دخول
    public class RolesController : Controller
    {
        private const string PermissionClaimType = "permission";
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // GET: /Roles
        [Authorize(Policy = Permissions.Roles_View)]
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.OrderBy(r => r.Name).ToList();
            return View(roles);
        }

        // GET: /Roles/Create
        [Authorize(Policy = Permissions.Roles_Edit)]
        [HttpGet]
        public IActionResult Create()
        {
            var vm = new RoleFormVm
            {
                AllPermissions = BuildPermissionsVm(new HashSet<string>())
            };
            return View(vm);
        }

        // POST: /Roles/Create
        [Authorize(Policy = Permissions.Roles_Edit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleFormVm vm)
        {
            if (!ModelState.IsValid)
            {
                vm.AllPermissions = BuildPermissionsVm(vm.SelectedPermissions.ToHashSet());
                return View(vm);
            }

            var roleName = vm.Name.Trim();

            if (await _roleManager.RoleExistsAsync(roleName))
            {
                ModelState.AddModelError("", "الدور موجود مسبقاً.");
                vm.AllPermissions = BuildPermissionsVm(vm.SelectedPermissions.ToHashSet());
                return View(vm);
            }

            var role = new IdentityRole(roleName);
            var createResult = await _roleManager.CreateAsync(role);

            if (!createResult.Succeeded)
            {
                foreach (var e in createResult.Errors)
                    ModelState.AddModelError("", e.Description);

                vm.AllPermissions = BuildPermissionsVm(vm.SelectedPermissions.ToHashSet());
                return View(vm);
            }

            var selectedPerms = (vm.SelectedPermissions ?? new List<string>())
                .Distinct()
                .ToList();

            foreach (var perm in selectedPerms)
                await _roleManager.AddClaimAsync(role, new Claim(PermissionClaimType, perm));

            TempData["Success"] = "تم إنشاء الدور بنجاح ✅";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Roles/Edit/{id}
        [Authorize(Policy = Permissions.Roles_Edit)]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            var claims = await _roleManager.GetClaimsAsync(role);
            var selectedPerms = claims
                .Where(c => c.Type == PermissionClaimType)
                .Select(c => c.Value)
                .ToHashSet();

            var vm = new RoleFormVm
            {
                Id = role.Id,
                Name = role.Name ?? "",
                SelectedPermissions = selectedPerms.ToList(),
                AllPermissions = BuildPermissionsVm(selectedPerms)
            };

            return View(vm);
        }

        // POST: /Roles/Edit
        [Authorize(Policy = Permissions.Roles_Edit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleFormVm vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Id))
                return BadRequest();

            var role = await _roleManager.FindByIdAsync(vm.Id);
            if (role == null) return NotFound();

            if (!ModelState.IsValid)
            {
                vm.AllPermissions = BuildPermissionsVm(vm.SelectedPermissions.ToHashSet());
                return View(vm);
            }

            role.Name = vm.Name.Trim();
            var updateRes = await _roleManager.UpdateAsync(role);

            if (!updateRes.Succeeded)
            {
                foreach (var e in updateRes.Errors)
                    ModelState.AddModelError("", e.Description);

                vm.AllPermissions = BuildPermissionsVm(vm.SelectedPermissions.ToHashSet());
                return View(vm);
            }

            // sync permissions
            var currentClaims = await _roleManager.GetClaimsAsync(role);
            var currentPerms = currentClaims
                .Where(c => c.Type == PermissionClaimType)
                .Select(c => c.Value)
                .ToHashSet();

            var desiredPerms = (vm.SelectedPermissions ?? new List<string>())
                .Distinct()
                .ToHashSet();

            foreach (var perm in currentPerms.Except(desiredPerms))
                await _roleManager.RemoveClaimAsync(role, new Claim(PermissionClaimType, perm));

            foreach (var perm in desiredPerms.Except(currentPerms))
                await _roleManager.AddClaimAsync(role, new Claim(PermissionClaimType, perm));

            TempData["Success"] = "تم تحديث الدور بنجاح ✅";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Roles/Delete/{id}
        [Authorize(Policy = Permissions.Roles_Edit)]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();
            return View(role);
        }

        // POST: /Roles/DeleteConfirmed
        [Authorize(Policy = Permissions.Roles_Edit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            var res = await _roleManager.DeleteAsync(role);
            if (!res.Succeeded)
            {
                TempData["Error"] = string.Join(" | ", res.Errors.Select(e => e.Description));
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "تم حذف الدور ✅";
            return RedirectToAction(nameof(Index));
        }

        // -----------------------------
        // Helpers
        // -----------------------------
        private List<RolePermissionVm> BuildPermissionsVm(HashSet<string> selected)
        {
            return Permissions.All
                .Distinct()
                .OrderBy(p => p)
                .Select(p => new RolePermissionVm
                {
                    Value = p,
                    Display = PermissionTranslator.ToDisplay(p), // ✅ ترجمة حسب اللغة
                    Selected = selected.Contains(p)
                })
                .ToList();
        }
    }
}
