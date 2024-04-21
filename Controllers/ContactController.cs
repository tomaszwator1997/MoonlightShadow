using Microsoft.AspNetCore.Mvc;
using MoonlightShadow.Models;
using MoonlightShadow.Services;
using MoonlightShadow.ViewModels;

namespace MoonlightShadow.Controllers
{
    public class ContactController : Controller
    {
        private readonly ContactService _contactService;

        public ContactController(ContactService contactService)
        {
            _contactService = contactService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ContactViewModel contactForm)
        {
            if(ModelState.IsValid == false)
            {
                return View(contactForm);
            }

            _contactService.Create(new Contact(contactForm));

            TempData.Remove("ShowModal");
            TempData["ShowModal"] = "ConfirmSendContactMessage";

            return RedirectToAction("Index", "Contact");
        }
    }
}