using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EList_Frontend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EList_Frontend.Controllers
{
    public class AccountController : Controller
    {
        private IConfiguration? configuration;
        string? baseUrl;
        public string? token;
        User sessionUser;
        public AccountController(IConfiguration config)
        {
            configuration = config;
            baseUrl = configuration.GetSection("ApiBaseUrl").GetSection("Baseurl").Value;
            sessionUser = new User();
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Signin()
        {
            return View(new User());
        }

        // Signining in the user
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signin(User user)
        {
            HttpClient client = new HttpClient();
            string url = baseUrl + "api/Users/";
            //if (ModelState.IsValid)
            //{
                //
                var jsonObj = JsonConvert.SerializeObject(new
                {
                    email = user.Email,
                    password = user.Password
                });
                var content = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                var userResponse = await response.Content.ReadAsStringAsync();
                // LoginResponse responseObj = JsonConvert.DeserializeObject<LoginResponse>(userResponse);
                if (response.IsSuccessStatusCode)
                {
                    sessionUser = JsonConvert.DeserializeObject<User>(userResponse);
                   // HttpContext.Session.SetString("UserId", sessionUser.UserID);
                        return Redirect("/Home/Index");
                }
                else
                {
                    TempData["FailedToLogin"] = "Information is incorrect.";
                }

            //}
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

    }
}
