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
using Newtonsoft.Json.Linq;

namespace EList_Frontend.Controllers
{
    public class ListController : Controller
    {
        private IConfiguration configuration;
        public string baseUrl;
        public List<List> listsOfUser;
        public List<Item> sortedItems;
        public List<List> sortedList;
        public static string token;
        public string apiKey;
        public static int userID;
        public static string userName;

        public ListController(IConfiguration config)
        {
            configuration = config;
            baseUrl = configuration.GetSection("ApiBaseUrl").GetSection("Baseurl").Value;
            apiKey = configuration.GetSection("ApiBaseUrl").GetSection("apikey").Value;
        }

        // GET: ListController
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            userID = (int)HttpContext.Session.GetInt32("UserId");
            userName = HttpContext.Session.GetString("UserName");
            token = HttpContext.Session.GetString("Token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            //string url = baseUrl + "list/ByUserId/"+userID + apiKey;
            string url = baseUrl + "list"+ apiKey;
            var response = await client.GetAsync(url);
            var userResponse = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                listsOfUser = JsonConvert.DeserializeObject<List<List>>(userResponse);
                sortedList = new List<List>();
                sortedItems = new List<Item>();
                ListItemModel listItemModel = new ListItemModel();
                listItemModel.Items = new List<Item>();
                listItemModel.CompletedItems = new List<Item>();
               
                GetListColors();

                foreach (List list in listsOfUser)
                {
                    if(list.UserId == userID)
                    {
                        sortedList.Add(list);
                    }
                   
                }
                for(int i = 0; i< sortedList.Count; i++)
                {
                    for(int j= i; j < sortedList[i].Items.Count; j++)
                    {
                        if(sortedList[i].ListId == sortedList[i].Items[j].ListId  && sortedList[i].Items[j].IsCompleted)
                        {
                            listItemModel.CompletedItems.Add(sortedList[i].Items[j]);
                        }
                        else
                        {
                            listItemModel.Items.Add(sortedList[i].Items[j]);
                        }
                       
                    }

                }
                //foreach (List l in sortedList)
                //{
                //    foreach(Item i in l.Items)
                //    {
                //        if (l.ListId == i.ListId)
                //        {
                //            sortedItems.Add(i);
                //        }
                //    }
                //}

                //foreach (Item i in sortedItems)
                //{
                //        if (i.IsCompleted)
                //        {
                //            listItemModel.CompletedItems.Add(i);

                //        }
                //        else
                //        {
                //            listItemModel.Items.Add(i);
                //        }
                //}


                if (listsOfUser != null)
                {
                    TempData["UserName"] = userName;
                    listItemModel.Lists = sortedList;
                    return View(listItemModel);
                }
            }
            return View();
        }
    
        public void GetListColors()
        {
            if(listsOfUser != null)
            {
                foreach (List l in listsOfUser)
                {
                    if (l.ListColor == ListColors.BG_DANGER)
                    {
                        l.color = "bg-danger";
                    }
                    else if (l.ListColor == ListColors.BG_INFO)
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
           
        }
 

        // POST: ListController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateList(List list)
        {
            userID = (int)HttpContext.Session.GetInt32("UserId");
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
                return Redirect("/List/Index");
            }
            catch
            {
                return Redirect("/List/Index");
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
                return Redirect("/List/Index");
            }
            catch
            {
                return Redirect("/List/Index");
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
                return Redirect("/List/Index");
            }
            catch
            {
                return Redirect("/List/Index");
            }
        }
        // POST: ListController/SearchList/math
        [HttpPost]
        public async Task<ActionResult> SearchList(ListItemModel listItemModel)
        {
            token = HttpContext.Session.GetString("Token");
            try
            {
                if (ModelState.IsValid)
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    string url = baseUrl + "list/" + listItemModel.List.ListName + apiKey;
                    var response = await client.PostAsync(url,null);
                    var userResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        listsOfUser = JsonConvert.DeserializeObject<List<List>>(userResponse);
                        TempData["message"] = "List found successfully!";
                        return RedirectToAction("/List/Index", listsOfUser);
                    }
                    else
                    {
                        TempData["error"] = "Can't find the list.";
                    }
                }
                return Redirect("/List/Index");
            }
            catch
            {
                return Redirect("/List/Index");
            }
        }
        public ActionResult Popup()
        {
            return View();
        }
    }
}
