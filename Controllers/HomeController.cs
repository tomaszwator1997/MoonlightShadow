using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoonlightShadow.Models;
using MoonlightShadow.Services;

namespace MoonlightShadow.Controllers
{
    public class HomeController : Controller
    {
        private readonly CameraService _cameraService;
        private readonly LaptopService _laptopService;
        private readonly PhoneService _phoneService;
        private readonly GameService _gameService;

        public HomeController(CameraService cameraService,
            LaptopService laptopService,
            PhoneService phoneService,
            GameService gameService)
        {
            _cameraService = cameraService;
            _laptopService = laptopService;
            _phoneService = phoneService;
            _gameService = gameService;
        }

        public List<Product> GetAllProducts()
        {
            var cameraList = _cameraService.Get();
            var laptopList = _laptopService.Get();
            var phoneList = _phoneService.Get();
            var gameList = _gameService.Get();

            var productList = new List<Product>();

            foreach (var camera in cameraList)
            {
                productList.Add(camera as Product);
            }

            foreach (var laptop in laptopList)
            {
                productList.Add(laptop as Product);
            }
            
            foreach (var phone in phoneList)
            {
                productList.Add(phone as Product);
            }

            foreach (var game in gameList)
            {
                productList.Add(game as Product);
            }

            return productList;
        }

        public IActionResult Index()
        {
            var productList = GetAllProducts();
            
            ViewBag.ProductBestsellerList = productList
                .OrderByDescending(product => product.BoughtQuantity).Take(4);
            ViewBag.ProductLastAddedList = productList
                .OrderByDescending(product => product.TimeCreated).Take(4);
            ViewBag.ProductRecomendedList = productList
                .OrderByDescending(product => product.RecomendedQuantity).Take(4);
            return View();
        }
    }
}
