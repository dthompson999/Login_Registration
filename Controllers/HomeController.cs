using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Login_Registration.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Login_Registration.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("process/registration")]
        public IActionResult ProcessRegistration(UserReg newReg)
        {
            if (ModelState.IsValid)
            {
                if (dbContext.Users.Any(u => u.Email == newReg.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                else
                {
                    // Initializing a PasswordHasher object, providing our class as its type
                    PasswordHasher<UserReg> Hasher = new PasswordHasher<UserReg>();
                    newReg.Password = Hasher.HashPassword(newReg, newReg.Password);
                    //Save your user object to the database
                    dbContext.Add(newReg);
                    dbContext.SaveChanges();
                    HttpContext.Session.SetInt32("UserId", newReg.UserId);
                    return RedirectToAction("SuccessLog");
                }
            }
            else
            {
                return View("Index");
            }
        }

        // [HttpGet("success/registration")]
        // public IActionResult SuccessReg()
        // {
        //     var userInDb = dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
        //     if (userInDb == null)
        //     {
        //         return RedirectToAction("Index");
        //     }
        //     return View();
        // }

        [HttpPost("process/login")]
        public IActionResult ProcessLogin(UserLog newLog)
        {
            if (ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == newLog.LoginEmail);
                // If no user exists with provided email
                if (userInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                    return View("Index");
                }
                else
                {
                    // Initialize hasher object
                    var hasher = new PasswordHasher<UserLog>();
                    // verify provided password against hash stored in db
                    var result = hasher.VerifyHashedPassword(newLog, userInDb.Password, newLog.LoginPassword);
                    if (result == 0)
                    {
                        return View("Index");
                    }
                    else
                    {
                        //store userId in session
                        HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                        return RedirectToAction("SuccessLog");
                    }
                }
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet("success")]
        public IActionResult SuccessLog(UserLog newLog)
        {
            
            var userInDb = dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
            ViewBag.FirstName = userInDb.FirstName;
            if (userInDb == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
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
