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
        string baseUrl;
        List<List> listsOfUser;
        public static string token;
        public static int userID;
        public List<List> sortedLists;

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
                 sortedLists = new List<List>();
                ListItemModel listItemModel = new ListItemModel();
                listItemModel.Items = new List<Item>();
                listItemModel.CompletedItems = new List<Item>();
                foreach (List list in listsOfUser)
                {
                    if (list.UserId == userID)
                    {
                        sortedLists.Add(list);
                    }
                }

                GetListColors();

                foreach (List slist in sortedLists)
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


                if (sortedLists != null)
                {
                    listItemModel.List = sortedLists;
                    return View(listItemModel);
                }
            }
            return View();
        }
        public void GetListColors()
        {
            foreach (List l in sortedLists)
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
        // GET: ListController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
                    string url = baseUrl + "api/list";
                    var jsonObj = JsonConvert.SerializeObject(new
                    {
                        listname = list.ListName,
                        userid= userID,
                        items = list.Items,
                        reminderdatetime = list.ReminderDateTime

                    });
                    var content = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);
                    var userResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["ListSuccess"] = "List created successfull!";
                        return Redirect("/List/Index");
                    }
                    else
                    {
                        TempData["FailedListCreation"] = "List cannot be created.";
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
            try
            {
                if (ModelState.IsValid)
                {
                    listItemModel.List.LastEdited = DateTime.Now;
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    string url = baseUrl + "api/list/"+ listItemModel.List.ListId;
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
                    string url = baseUrl + "api/list/"+id;
                    var response = await client.DeleteAsync(url);
                    var userResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["ListSuccess"] = "List deleted successfully!";
                        return Redirect("/List/Index");
                    }
                    else
                    {
                        TempData["FailedList"] = "List cannot be deleted.";
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
