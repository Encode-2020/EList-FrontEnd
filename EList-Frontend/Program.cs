using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EList_Frontend
{
    public class Program
    {
        static HttpClient client = new HttpClient();
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
           // RunAsync().GetAwaiter().GetResult();

        }

        //public static Task RunAsync()
        //{
        //    client.BaseAddress = new Uri("https://srashi25-eval-prod.apigee.net/elist/users");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
        //}

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
