using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Login_Registration.Models;

namespace Login_Registration.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("success/registration")]
        public IActionResult SuccessReg(UserReg newReg)
        {
            if (ModelState.IsValid)
            {
                return View();
            }
            else
            {
                return View("Index");
            }
            
        }

        [HttpPost("success/login")]
        public IActionResult SuccessLog(UserLog newLog)
        {
            if (ModelState.IsValid)
            {
                return View();
            }
            else
            {
                return View("Index");
            }
            
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
