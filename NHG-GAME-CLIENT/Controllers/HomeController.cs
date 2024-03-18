using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHG_GAME_CLIENT.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace NHG_GAME_CLIENT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient client = new HttpClient();
        private string AccountAPIUrl = "https://localhost:44372/api/connectword";
        

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;        
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);           
        }

        public async Task<IActionResult> Index(string answer)
        {           
            if (answer != null)
            {
                HttpResponseMessage response = await client.GetAsync($"{AccountAPIUrl}/getnextword={answer}");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    string word = responseObject.word;
                    ViewData["Question"] = word;
                }
            } else
            {
                HttpResponseMessage response = await client.GetAsync($"{AccountAPIUrl}/getword");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    string word = responseObject.word;
                    ViewData["Question"] = word;
                }
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Answer(string question, string answer)
        {
            HttpResponseMessage response = await client.PostAsync($"{AccountAPIUrl}/question={question}&answer={answer}", null);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
                bool status = responseObject.status;
                if (status)
                {
                   return RedirectToAction("Index", new { answer = answer });
                }
            }
            return Ok("Quá ngu, đề nghị chơi lại!!!");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
