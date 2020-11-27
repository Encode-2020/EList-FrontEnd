using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EList_Frontend.Models;
using EList_Frontend.Models.ViewModel;
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
        User loggedUser;
        User createdUser;
        public AccountController(IConfiguration config)
        {
            configuration = config;
            baseUrl = configuration.GetSection("ApiBaseUrl").GetSection("Baseurl").Value;
            loggedUser = new User();
            createdUser = new User();
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
        public async Task<IActionResult> Signin(Login login)
        {
            User fetchedUser = new User();
            fetchedUser =await GetUser(login);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            string url = baseUrl + "api/Users/"+fetchedUser.UserID;
            if (ModelState.IsValid)
            {
                var response = await client.GetAsync(url);
                var userResponse = await response.Content.ReadAsStringAsync();
                // LoginResponse responseObj = JsonConvert.DeserializeObject<LoginResponse>(userResponse);
                if (response.IsSuccessStatusCode)
                {
                    loggedUser = JsonConvert.DeserializeObject<User>(userResponse);
                    HttpContext.Session.SetInt32("UserId", loggedUser.UserID);
                    return Redirect("/List/Index");
                }
                else
                {
                    TempData["FailedToLogin"] = "Information is incorrect.";
                }

            }
            return View();
        }
        public async Task<User> GetUser(Login login)
        {
            RootResponse responseObj = new RootResponse();
             HttpClient client = new HttpClient();
                string url = baseUrl + "api/login/";
                //
                var jsonObj = JsonConvert.SerializeObject(new
                {
                    email = login.Email,
                    password = login.Password
                });
                var content = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                var userResponse = await response.Content.ReadAsStringAsync();
                responseObj = JsonConvert.DeserializeObject<RootResponse>(userResponse);

                HttpContext.Session.SetString("Token", responseObj.LoginResponse.Token);
                token = responseObj.LoginResponse.Token;
            return responseObj.LoginResponse.User;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Signup()
        {
            return View(new User());
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Signup(User user)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = new HttpClient();
                string url = baseUrl + "api/Users";
                var jsonObj = JsonConvert.SerializeObject(new
                {
                    username = user.Username,
                    email = user.Email,
                    password = user.Password
                });
                var content = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);
                    var userResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                    createdUser = JsonConvert.DeserializeObject<User>(userResponse);
                    HttpContext.Session.SetInt32("UserId", createdUser.UserID);
                    TempData["SignupSuccess"] = "User created successfull!";
                    return Redirect("/Account/Signin");
                    }
                    else
                    {
                        TempData["FailedToLogin"] = "Information is incorrect.";
                    }
            }
           
            return View();
        }

        public IActionResult Signout()
        {
            HttpContext.Session.Remove("Token");
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Login");
        }

    }
}
