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
using Newtonsoft.Json.Serialization;

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

        // GET: ItemController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // POST: ItemController/Create
        [HttpPost]
        public async Task<ActionResult> CreateItem(ListItemModel listItemModel, int id)
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
                        iscompleted = listItemModel.Item.IsCompleted,
                         listId = id
                    });
                    var content = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);
                    var userResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["ItemSuccess"] = "Item created successfull!";
                      return Redirect("/List/Index");
                    }
                    else
                    {
                        TempData["FailedItemCreation"] = "Failed to add item!";
                    }
                }
                return Redirect("/List/Index");
            }
            catch
            {
                return Redirect("/List/Index");
            }
        }


        // POST: ItemController/Edit/5
        [HttpPost]
        public async Task<ActionResult> PatchIsCompleted(ListItemModel listItemModel, int id)
        {

            bool status = false;
            if(listItemModel.Item.IsCompleted == false)
            {
                status = true;
            } else if (listItemModel.Item.IsCompleted == true) {
                status = false;
               
            }

            if(listItemModel.Item.IsCompleted) {
                listItemModel.Item.Url = "/images/select.png";
            } else
            {
                listItemModel.Item.Url = "/images/blank-check-box.png";
            }
            token = HttpContext.Session.GetString("Token"); 
            try
            {
                if (ModelState.IsValid)
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    string url = baseUrl + "api/item/" + id + "/" + listItemModel.Item.ItemId + "/isCompleted?status=" + status;

                    var response = await client.PatchAsync(url, null);
                    var userResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["ItemSuccess"] = "Item edited successfully!";
                        return Redirect("/List/Index");
                    }
                    else
                    {
                        TempData["FailedItems"] = "Failed to edit item!";
                    }
                }
                return Redirect("/List/Index");
            }
            catch
            {
                return Redirect("/List/Index");
            }
        }

        // GET: ItemController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ItemController/Delete/5/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteItem(int itemId, int id)
        {
            token = HttpContext.Session.GetString("Token");
            try
            {
                if (ModelState.IsValid)
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    string url = baseUrl + "api/item/" + id + "/"+itemId;
                    var response = await client.DeleteAsync(url);
                    var userResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["ItemSuccess"] = "Item deleted successfully!";
                        return Redirect("/List/Index");
                    }
                    else
                    {
                        TempData["FailedItems"] = "Failed to edit item!";
                    }
                }
                return Redirect("/List/Index");
            }
            catch
            {
                return Redirect("/List/Index");
            }
        }
        [HttpPost]
        public async Task<ActionResult> EditItem(ListItemModel listItemModel, int id)
        {
            token = HttpContext.Session.GetString("Token");
            try
            {
                if (ModelState.IsValid)
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    string url = baseUrl + "api/item/" + listItemModel.Item.ItemId;
                    var jsonObj = JsonConvert.SerializeObject(new
                    {
                        itemId = listItemModel.Item.ItemId,
                        description = listItemModel.Item.Description,
                        iscompleted = listItemModel.Item.IsCompleted,
                        listId = id
                    });
                    var content = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(url, content);
                    var userResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["ItemSuccess"] = "Item edited successfully!";
                        return Redirect("/List/Index");
                    }
                    else
                    {
                        TempData["FailedItems"] = "Failed to edit item!";
                    }
                }
                return Redirect("/List/Index");
            }
            catch
            {
                return Redirect("/List/Index");
            }
        }

    }
}
