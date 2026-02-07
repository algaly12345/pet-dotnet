using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pets.Models;
using pets.ViewModels;
using Microsoft.AspNetCore.Authorization;


namespace pets.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public LoginController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GET: /Login
        [HttpGet]
        public IActionResult Index(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginVm());
        }

        // POST: /Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginVm vm, string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "رجاءً تأكد من إدخال اسم المستخدم وكلمة المرور.");
                return View(vm);
            }

            // =========================
            // DEMO LOGIN (demo / demo)
            // =========================
            if (string.Equals(vm.UserName, "demo", StringComparison.OrdinalIgnoreCase)
                && vm.Password == "demo")
            {
                var demoUser = await _userManager.FindByNameAsync("demo");

                if (demoUser == null)
                {
                    demoUser = new User
                    {
                        UserName = "demo",
                        Email = "demo@demo.com",
                        FullName = "Demo User",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    // لو Password policy تمنع "demo" غيّرها إلى "Demo@12345"
                    var createResult = await _userManager.CreateAsync(demoUser, "demo");
                    if (!createResult.Succeeded)
                    {
                        ModelState.AddModelError("", "تعذر إنشاء حساب demo بسبب سياسة كلمة المرور.");
                        foreach (var err in createResult.Errors)
                            ModelState.AddModelError("", err.Description);

                        return View(vm);
                    }
                }

                await _signInManager.SignInAsync(demoUser, isPersistent: vm.RememberMe);
                TempData["Success"] = "تم تسجيل الدخول بنجاح ✅";
                return RedirectToLocal(returnUrl);
            }

            // =========================
            // NORMAL LOGIN (DB)
            // =========================
            var user = await _userManager.FindByNameAsync(vm.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "اسم المستخدم غير موجود.");
                return View(vm);
            }

            // (اختياري) لو عندك IsActive
            if (!user.IsActive)
            {
                ModelState.AddModelError("", "هذا الحساب غير مفعل. تواصل مع الإدارة.");
                return View(vm);
            }

            var result = await _signInManager.PasswordSignInAsync(
                vm.UserName,
                vm.Password,
                vm.RememberMe,
                lockoutOnFailure: true
            );

            if (result.Succeeded)
            {
                TempData["Success"] = "تم تسجيل الدخول بنجاح ✅";
                return RedirectToLocal(returnUrl);
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "تم قفل الحساب مؤقتًا بسبب محاولات خاطئة كثيرة. حاول لاحقًا.");
                return View(vm);
            }

            if (result.IsNotAllowed)
            {
                ModelState.AddModelError("", "غير مسموح بتسجيل الدخول (قد تحتاج تأكيد البريد/الهاتف).");
                return View(vm);
            }

            if (result.RequiresTwoFactor)
            {
                ModelState.AddModelError("", "يلزم التحقق بخطوتين لتسجيل الدخول.");
                return View(vm);
            }

            ModelState.AddModelError("", "كلمة المرور غير صحيحة.");
            return View(vm);
        }

        // POST: /Login/Logout
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            // بعد الخروج يرجع لصفحة تسجيل الدخول
            return RedirectToAction("Index", "Login");
        }
        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            // ✅ Dashboard عندك هو Home/Index
            return RedirectToAction("Index", "Home");
        }
    }
}
