using Microsoft.AspNetCore.Mvc;
using MoonlightShadow.Models;
using MoonlightShadow.ViewModels;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using MoonlightShadow.Services;
using Extension.Security;
using Extension.ValidModel;
using WebApi.Services;

namespace MoonlightShadow.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserService _userService;
        private readonly MailSenderService _mailSenderService;
        private readonly SessionService _sessionService;
        private readonly TokenService _tokenService;

        public LoginController(UserService userService,
            MailSenderService mailSenderService,
            SessionService sessionService,
            TokenService tokenService)
        {
            _userService = userService;
            _sessionService = sessionService;
            _mailSenderService = mailSenderService;
            _tokenService = tokenService;
        }

        public IActionResult Index(string returnUrl = null)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            if (userEmail.IsNotNull()) 
            {
                return RedirectToAction("Index", "Account");
            }

            ViewBag.ReturnUrl = returnUrl;

            ViewData["MailRegisterVerified"] = true;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LoginViewModel loginViewModel, string returnUrl = null)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            if (userEmail.IsNotNull()) 
            {
                return RedirectToAction("Index", "Account");
            }

            ViewBag.ReturnUrl = returnUrl;

            ViewData["MailRegisterVerified"] = true;

            if (loginViewModel.TypeOfModel == "LoginFormViewModel")
            {
                if (TryValidateModel(loginViewModel.LoginFormViewModel) == false)
                {
                    return View(loginViewModel);
                }

                var user = _userService
                    .GetByEmail(loginViewModel.LoginFormViewModel.Email);

                if (user == null)
                {
                    ModelState.AddModelError("LoginFormViewModel.Email",
                        "Użytkownik nie istnieje w bazie");

                    return View(loginViewModel);
                }

                if (user.IsMailRegisterVerified == false)
                {
                    ViewData["MailRegisterVerified"] = false;

                    ModelState.AddModelError("LoginFormViewModel.Email", 
                        "Mail nie został zweryfikowany");

                    return View(loginViewModel);
                }

                if (user.PasswordHash != Hasher
                    .Encrypt(loginViewModel.LoginFormViewModel.Password))
                {
                    ModelState.AddModelError("LoginFormViewModel.Password", 
                        "Hasło niepoprawne");

                    return View(loginViewModel);
                }

                HttpContext.SignInAsync(Claimer.CreateClaimPrincipal(ClaimTypes.Email, user.Email));
            
                if (returnUrl.IsNotNullOrEmptyOrWhiteSpace())
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else if (loginViewModel.TypeOfModel == "ResetPasswordViewModel")
            {
                if (TryValidateModel(loginViewModel.ResetPasswordViewModel) == false)
                {
                    TempData.Remove("ShowModal");
                    TempData["ShowModal"] = "ResetPassword";

                    return View(loginViewModel);
                }

                var user = _userService
                    .GetByEmail(loginViewModel.ResetPasswordViewModel.Email);

                if (user == null)
                {
                    ModelState.AddModelError("ResetPasswordViewModel.Email", 
                        "Użytkownik nie istnieje w bazie");

                    TempData.Remove("ShowModal");
                    TempData["ShowModal"] = "ResetPassword";

                    return View(loginViewModel);
                }

                var newPassword = Hasher.GetRandomAlphaNumericString(8);

                var acceptNewPasswordToken = Hasher
                    .GetRandomAlphaNumericString(30).ToLower();

                _mailSenderService
                    .SendNewPassword(loginViewModel.ResetPasswordViewModel.Email, 
                    user.Login, newPassword, acceptNewPasswordToken);

                _tokenService.Create(new Token() { name = "acceptNewPasswordToken", 
                    value = acceptNewPasswordToken, 
                    email = loginViewModel.ResetPasswordViewModel.Email});

                user.NewPasswordHash = Hasher.Encrypt(newPassword);

                _userService.Update(user);

                TempData.Remove("ShowModal");
                TempData["ShowModal"] = "ConfirmResetPasswordMailSend";

                return View();
            }

            return View(loginViewModel);
        }

        public IActionResult AcceptResetPassword(string email, string token)
        {
            var tokenFromDb = _tokenService.GetByValue(token);

            if (tokenFromDb.IsNotNull() && tokenFromDb.email == email)
            {
                _tokenService.Remove(tokenFromDb);
                
                var user = _userService.GetByEmail(email);

                if (user != null)
                {
                    user.PasswordHash = user.NewPasswordHash;

                    _userService.Update(user);

                    TempData.Remove("ShowModal");
                    TempData["ShowModal"] = "ConfirmResetPassword";

                    return RedirectToAction("Index", "Login");
                }
            }

            TempData.Remove("ShowModal");
            TempData["ShowModal"] = "AbortResetPassword";

            return RedirectToAction("Index", "Login");
        }
    }
}