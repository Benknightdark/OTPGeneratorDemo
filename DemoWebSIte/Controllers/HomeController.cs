using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DemoWebSIte.Models;
using OTPGenerator;
namespace DemoWebSIte.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private OTPCodeGeneratorService _otpService;
        public HomeController(ILogger<HomeController> logger,OTPCodeGeneratorService otpService)
        {
            _logger = logger;
            _otpService=otpService;
        }

        public IActionResult Index()
        {
            ViewBag.code=_otpService.getGACode();
 
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
