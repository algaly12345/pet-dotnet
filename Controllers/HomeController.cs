using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace pets.Controllers
{
    [Authorize] // ✅ يمنع الوصول بدون تسجيل دخول
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        // ✅ لا نريد منع تغيير اللغة حتى لو المستخدم غير مسجل
        [AllowAnonymous]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            // لو ما فيه returnUrl رجعه على نفس الصفحة حسب حالته
            return RedirectToAction("Index", "Login");
        }
    }
}
