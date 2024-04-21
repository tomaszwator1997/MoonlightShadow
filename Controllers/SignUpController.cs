using Microsoft.AspNetCore.Mvc;
using MoonlightShadow.Models;
using MoonlightShadow.ViewModels;
using MoonlightShadow.Services;
using Extension.ValidModel;
using WebApi.Services;
using Extension.Security;
using System.Security.Claims;

namespace MoonlightShadow.Controllers
{
    public class SignUpController : Controller
    {
        private readonly UserService _userService;
        private readonly MailSenderService _mailSenderService;
        private readonly SessionService _sessionService;
        private readonly TokenService _tokenService;

        public SignUpController(UserService userService,
            MailSenderService mailSenderService,
            SessionService sessionService,
            TokenService tokenService)
        {
            _userService = userService;
            _mailSenderService = mailSenderService;
            _sessionService = sessionService;
            _tokenService = tokenService;
        }

        public IActionResult Index()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            if (userEmail.IsNotNull()) 
            {
                return RedirectToAction("Index", "Account");
            }
            
            return View();
        }

        [HttpPost]
        public IActionResult Index(SignUpViewModel SignUpForm)
        {
            if (ModelState.IsValid == false)
            {
                return View(SignUpForm);
            }

            if(SignUpForm.Password != SignUpForm.RepetedPassword)
            {
                ModelState.AddModelError("RepetedPassword", "Hasła nie są identyczne");

                SignUpForm.Password = "";
                SignUpForm.RepetedPassword = "";

                return View(SignUpForm);
            }

            var user = _userService.GetByLogin(SignUpForm.Login);

            if (user.IsNotNull())
            {
                ModelState.AddModelError("Login", "Użytkownik już istnieje w bazie");

                return View(SignUpForm);
            }

            user = _userService.GetByEmail(SignUpForm.Email);

            if (user.IsNotNull())
            {
                ModelState.AddModelError("Email", "Istnieje konto na podanego maila");

                return View(SignUpForm);
            }

            _userService.Create(new Models.User(SignUpForm));

            var registrationToken = Hasher.GetToken();
            
            _tokenService.Create(new Token() 
                { name = "registrationToken", 
                value = registrationToken, 
                email = SignUpForm.Email });

            _mailSenderService.SendRegistrationMail(SignUpForm.Email, 
                SignUpForm.Login, 
                registrationToken);

            TempData.Remove("ShowModal");
            TempData["ShowModal"] = "ConfirmSignUpMailSend";

            return RedirectToAction("Index", "SignUp");
        }

        [HttpGet]
        public IActionResult VerifyEmail(string email, string token)
        {
            var tokenFromDb = _tokenService.GetByValue(token);

            if (tokenFromDb.IsNotNull() && tokenFromDb.email == email)
            {
                var user = _userService.GetByEmail(email);

                if(user.IsNull())
                {
                    TempData.Remove("ShowModal");
                    TempData["ShowModal"] = "AbortVerificationMail";

                    return RedirectToAction("Index", "Login");
                }

                user.IsMailRegisterVerified = true;

                _userService.Update(user);

                TempData.Remove("ShowModal");
                TempData["ShowModal"] = "ConfirmVerificationMail";

                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData.Remove("ShowModal");
                TempData["ShowModal"] = "AbortVerificationMail";

                return RedirectToAction("Index", "Login");
            }
        }
    }
}