using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EList_Frontend.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EList_Frontend.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration configuration;
        string baseUrl;
        List<List> listsOfUser;
        string token;
        int userID;

        public HomeController(IConfiguration config)
        {
            configuration = config;
            baseUrl = configuration.GetSection("ApiBaseUrl").GetSection("Baseurl").Value;
            token= HttpContext.Session.GetString("Token");
            userID = (int)HttpContext.Session.GetInt32("UserId");
        }

        public async Task<IActionResult> Index()
        {
          
            
            Debug.WriteLine("User id is: " + userID);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            string url = baseUrl + "api/list";
            var response = await client.GetAsync(url);
            var userResponse = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                listsOfUser = JsonConvert.DeserializeObject<List<List>>(userResponse);
                Debug.WriteLine("Total number of lists: " + listsOfUser.Count());
                List<List> sortedLists = new List<List>();
                foreach (List list in sortedLists)
                {
                    if (list.UserId ==userID)
                    {
                        sortedLists.Add(list);
                    }
                }
                if (sortedLists != null)
                {
                    return View(sortedLists);
                }
            }
            return View();
        }

        public Task<IActionResult> GotoList(int? id)
        {
            return null;
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
