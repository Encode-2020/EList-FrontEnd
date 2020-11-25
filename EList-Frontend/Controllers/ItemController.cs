using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EList_Frontend.Models;
using EList_Frontend.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EList_Frontend.Controllers
{
    public class ItemController : Controller
    {

        private IConfiguration configuration;
        string baseUrl;
        public static string token;

        public ItemController(IConfiguration config)
        {
            configuration = config;
            baseUrl = configuration.GetSection("ApiBaseUrl").GetSection("Baseurl").Value;
           
        }
        // GET: ItemController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ItemController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ItemController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ItemController/Create
        [HttpPost]
        public async Task<ActionResult> CreateItem(ListItemModel listItemModel)
        {
            token = HttpContext.Session.GetString("Token");
            Item createdItem = new Item();
            try
            {
                if (ModelState.IsValid)
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    string url = baseUrl + "api/item";
                    var jsonObj = JsonConvert.SerializeObject(new
                    {
                        description = listItemModel.Item.Description,
                        iscompleted = listItemModel.Item.isCompleted,
                         listId = listItemModel.List.ListId
                    });
                    var content = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);
                    var userResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["ItemSuccess"] = "Item created successfull!";
                        return Redirect("/List/Index");
                        //return View();
                    }
                    else
                    {
                        TempData["FailedItemCreation"] = "Item cannot be created.";
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: ItemController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ItemController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Item item)
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

        // GET: ItemController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ItemController/Delete/5
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
