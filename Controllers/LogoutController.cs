using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;


namespace MoonlightShadow.Controllers
{
    public class LogoutController : Controller
    {
        public LogoutController()
        {}

        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}