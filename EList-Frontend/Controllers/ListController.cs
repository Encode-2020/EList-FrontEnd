using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class ListController : Controller
    {
        private IConfiguration configuration;
        public string baseUrl;
        public List<List> listsOfUser;
        public static string token;
        public string apiKey;
        public static int userID;

        public ListController(IConfiguration config)
        {
            configuration = config;
            baseUrl = configuration.GetSection("ApiBaseUrl").GetSection("Baseurl").Value;
            apiKey = configuration.GetSection("ApiBaseUrl").GetSection("apikey").Value;
        }

        // GET: ListController
        public async Task<IActionResult> Index()
        {
            userID = (int)HttpContext.Session.GetInt32("UserId");
            token = HttpContext.Session.GetString("Token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            string url = baseUrl + "list/ByUserId/"+userID + apiKey;
            var response = await client.GetAsync(url);
            var userResponse = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                listsOfUser = JsonConvert.DeserializeObject<List<List>>(userResponse);
                ListItemModel listItemModel = new ListItemModel();
                listItemModel.Items = new List<Item>();
                listItemModel.CompletedItems = new List<Item>();
               
                GetListColors();

                foreach (List slist in listsOfUser)
                {
                    foreach(Item i in slist.Items)
                    {
                        if (i.IsCompleted)
                        {
                            listItemModel.CompletedItems.Add(i);

                        } else
                        {
                            listItemModel.Items.Add(i);
                        }
                    }
                }


                if (listsOfUser != null)
                {
                    listItemModel.Lists = listsOfUser;
                    return View(listItemModel);
                }
            }
            return View();
        }
        public void GetListColors()
        {
            foreach (List l in listsOfUser)
            {
                if (l.ListColor == ListColors.BG_DANGER)
                {
                    l.color = "bg-danger";
                } else if (l.ListColor == ListColors.BG_INFO)
                {
                    l.color = "bg-info";
                }
                else if (l.ListColor == ListColors.BG_PRIMARY)
                {
                    l.color = "bg-primary";
                }
                else if (l.ListColor == ListColors.BG_SECONDARY)
                {
                    l.color = "bg-secondary";
                }
                else if (l.ListColor == ListColors.BG_SUCCESS)
                {
                    l.color = "bg-success";
                }
                else
                {
                    l.color = "bg-warning";
                }
            }
        }
 

        // POST: ListController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateList(List list)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    string url = baseUrl + "list"+apiKey;
                    var jsonObj = JsonConvert.SerializeObject(new
                    {
                        listname = list.ListName,
                        userid= userID,
                        items = list.Items,
                        reminderdatetime = list.ReminderDateTime,
                        listcolor = list.ListColor,

                    });
                    var content = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);
                    var userResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["message"] = "List created successfull!";
                        return Redirect("/List/Index");
                    }
                    else
                    {
                        TempData["error"] = "List cannot be created.";
                    }
                }
                return View();
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
        public async Task<ActionResult> EditList( ListItemModel listItemModel)
        {
            userID = (int)HttpContext.Session.GetInt32("UserId");
            try
            {
                if (ModelState.IsValid)
                {
                    listItemModel.List.LastEdited = DateTime.Now;
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    string url = baseUrl + "list/"+ listItemModel.List.ListId+apiKey;
                    var jsonObj = JsonConvert.SerializeObject(new
                    {
                        listname = listItemModel.List.ListName,
                        userid = listItemModel.List.UserId,
                        items = listItemModel.List.Items,
                        reminderdatetime = listItemModel.List.ReminderDateTime,
                        listcolor = listItemModel.List.ListColor,
                        listid= listItemModel.List.ListId,
                        lastedited= listItemModel.List.LastEdited

                    });
                    var content = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(url, content);
                    var userResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["message"] = "List edited successfully!";
                        return Redirect("/List/Index");
                    }
                    else
                    {
                        TempData["error"] = "List cannot be edited.";
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }


        // POST: ListController/Delete/5
        [HttpPost]
        public async Task<ActionResult> DeleteList(int id)
        {
            token = HttpContext.Session.GetString("Token");
            try
            {
                if (ModelState.IsValid)
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    string url = baseUrl + "list/"+id+apiKey;
                    var response = await client.DeleteAsync(url);
                    var userResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["message"] = "List deleted successfully!";
                        return Redirect("/List/Index");
                    }
                    else
                    {
                        TempData["error"] = "List cannot be deleted.";
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Popup()
        {
            return View();
        }
    }
}
