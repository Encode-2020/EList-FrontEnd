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
        public string baseUrl;
        public string apiKey;
        public static string? token;
        User loggedUser;
        User createdUser;
        public AccountController(IConfiguration config)
        {
            configuration = config;
            baseUrl = configuration.GetSection("ApiBaseUrl").GetSection("Baseurl").Value;
            apiKey= configuration.GetSection("ApiBaseUrl").GetSection("apikey").Value;
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
            if(fetchedUser != null)
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                string url = baseUrl + "users/" + fetchedUser.UserID + apiKey;
                if (ModelState.IsValid)
                {
                    var response = await client.GetAsync(url);
                    var userResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        loggedUser = JsonConvert.DeserializeObject<User>(userResponse);
                        HttpContext.Session.SetInt32("UserId", loggedUser.UserID);
                        return Redirect("/List/Index");
                    }
                    else
                    {
                        TempData["error"] = "Information is incorrect.";
                    }

                }
            }
            TempData["error"] = "Information is incorrect.";
            return View();
        }
        public async Task<User> GetUser(Login login)
        {
            RootResponse responseObj = new RootResponse();
             HttpClient client = new HttpClient();
                string url = baseUrl + "login"+apiKey;
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
            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("Token", responseObj.LoginResponse.Token);
                token = responseObj.LoginResponse.Token;
                return responseObj.LoginResponse.User;
            } 
            return null;  
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
                string url = baseUrl + "Users" + apiKey;
              
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
                    } else if(response.StatusCode.ToString() == "409")
                        {
                        TempData["error"] = "User already exist.";
                        }
                    else
                    {
                        TempData["error"] = "Failed to create user.";
                    }
            }
           
            return View();
        }

        public IActionResult Signout(string returnUrl = "/")
        {
            HttpContext.Session.Remove("Token");
            HttpContext.Session.Remove("UserId");
            return Redirect(returnUrl);
        }

    }
}
