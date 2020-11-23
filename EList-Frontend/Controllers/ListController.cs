using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EList_Frontend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EList_Frontend.Controllers
{
    public class ListController : Controller
    {
        private IConfiguration configuration;
        string baseUrl;
        List<List> listsOfUser;
        string token;
        int userID;

        public ListController(IConfiguration config)
        {
            configuration = config;
            baseUrl = configuration.GetSection("ApiBaseUrl").GetSection("Baseurl").Value;
           
           
        }

        // GET: ListController
        public async Task<IActionResult> Index()
        {
            userID = (int)HttpContext.Session.GetInt32("UserId");
            token = HttpContext.Session.GetString("Token");
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
                    if (list.UserId == userID)
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

        // GET: ListController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ListController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ListController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ListController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ListController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ListController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ListController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
