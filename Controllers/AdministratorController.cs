using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MoonlightShadow.Services;
using Microsoft.AspNetCore.Authorization;
using MoonlightShadow.ViewModels.Administrator;
using WebApi.Services;
using Extension.ValidModel;

namespace MoonlightShadow.Controllers
{
    public class AdministratorController : Controller
    {
        private readonly TransactionService _transactionService;
        private readonly UserService _userService;
        private readonly MailSenderService _mailSenderService;
        private readonly CameraService _cameraService;
        private readonly LaptopService _laptopService;
        private readonly PhoneService _phoneService;
        private readonly GameService _gameService;
        private readonly ContactService _contactService;

        public AdministratorController(UserService userService, 
                                        TransactionService transactionService,
                                        MailSenderService mailSenderService,
                                        CameraService cameraService,
                                        LaptopService laptopService,
                                        PhoneService phoneService,
                                        GameService gameService,
                                        ContactService contactService)
        {
            _transactionService = transactionService;
            _userService = userService;
            _mailSenderService = mailSenderService;
            _cameraService = cameraService;
            _laptopService = laptopService;
            _phoneService = phoneService;
            _gameService = gameService;
            _contactService = contactService;
        }
        
        [Authorize]
        public IActionResult Index()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            var user = _userService.GetByEmail(userEmail);

            if (user.IsNull())
            {
                RedirectToAction("Index", "Login");
            }

            if(user.Privileges == false)
            {
                TempData.Remove("ShowModal");
                TempData["ShowModal"] = "NoPermissions";
                return RedirectToAction("Index", "Home");
            }

            var administratorViewModel = new AdministratorViewModel();

            administratorViewModel.UsersTransactions = _transactionService
                .Get()
                .Where(transaction => transaction.BoughtOrder.isShippment == false)
                .ToList();
            
            administratorViewModel.Contacts = _contactService.Get();
               
            return View(administratorViewModel);
        }

        [HttpGet]
        public IActionResult SetPaymentVerified(string id)
        {
            var transaction = _transactionService.GetBy(id);

            transaction.BoughtOrder.isPaymentVerified = true;

            _transactionService.Update(transaction);

            var user = _userService.GetByLogin(transaction.UserId);

            if(user != null)
            {
                var boughtOrder = user.BoughtOrders
                    .Where(boughtOrder =>  
                        boughtOrder.TitleTransaction == 
                        transaction.BoughtOrder.TitleTransaction)
                    .FirstOrDefault();

                var index = user.BoughtOrders.IndexOf(boughtOrder);

                boughtOrder.isPaymentVerified = true;

                user.BoughtOrders[index] = boughtOrder;

                _userService.Update(user);
            }

            _mailSenderService
                .SendPaymentAcceptedMail(
                    transaction.EmailShipping, transaction.BoughtOrder.TitleTransaction
                    );

            foreach (var productItemInTransaction in transaction.BoughtOrder.ProductItems)
            {
                if(productItemInTransaction.Category == "Camera")
                {
                    var camera = _cameraService
                        .Get()
                        .FirstOrDefault(camera => 
                            camera.Id == productItemInTransaction.Id);

                    if(camera != null)
                    {
                        camera.BoughtQuantity++;
                        _cameraService.Update(camera.Id, camera);
                    }
                }
                else if(productItemInTransaction.Category == "Game")
                {
                    var game = _gameService
                        .Get()
                        .FirstOrDefault(game => 
                            game.Id == productItemInTransaction.Id);

                    if(game != null)
                    { 
                        game.BoughtQuantity++;
                        _gameService.Update(game.Id, game);
                    }
                }
                else if(productItemInTransaction.Category == "Laptop")
                {
                    var laptop = _laptopService
                        .Get()
                        .FirstOrDefault(laptop => 
                            laptop.Id == productItemInTransaction.Id);

                    if(laptop != null)
                    {
                        laptop.BoughtQuantity++;
                        _laptopService.Update(laptop.Id, laptop);
                    }
                }
                else if(productItemInTransaction.Category == "Phone")
                {
                    var phone = _phoneService
                        .Get()
                        .FirstOrDefault(phone => 
                            phone.Id == productItemInTransaction.Id);

                    if(phone != null)
                    { 
                        phone.BoughtQuantity++;
                        _phoneService.Update(phone.Id, phone);
                    }
                }
            }

            return RedirectToAction("Index", "Administrator");
        }

        [HttpGet]
        public IActionResult SetShippment(string id)
        {
            var transaction = _transactionService.GetBy(id);

            transaction.BoughtOrder.isShippment = true;

            _transactionService.Update(transaction);

            var user = _userService.GetByLogin(transaction.UserId);

            if(user != null)
            {
                var boughtOrder = user.BoughtOrders
                    .Where(boughtOrder => 
                        boughtOrder.TitleTransaction == 
                        transaction.BoughtOrder.TitleTransaction)
                    .FirstOrDefault();

                var index = user.BoughtOrders.IndexOf(boughtOrder);

                boughtOrder.isShippment = true;

                user.BoughtOrders[index] = boughtOrder;

                _userService.Update(user);
            }

            _mailSenderService
                .SendShippingOnTheWayMail(
                    transaction.EmailShipping, 
                    transaction.BoughtOrder.TitleTransaction
                );

            return RedirectToAction("Index", "Administrator");
        }
    }
}