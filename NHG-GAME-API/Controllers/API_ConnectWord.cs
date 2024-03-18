using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using DataAccess;
using System.Linq;

namespace NHG_GAME_API.Controllers
{
    [Route("api/connectword")]
    [ApiController]
    public class API_ConnectWord : Controller
    {
        [HttpPost]
        public IActionResult CheckWordIsTrue(string word)
        {
            string filePath = @"Data\data_words.txt";
            var lines = System.IO.File.ReadAllLines(filePath);
            var isValidWord = false;

            foreach (var line in lines)
            {
                var wordItem = JsonConvert.DeserializeObject<DataWord>(line);
                if (wordItem != null)
                {
                    int wordLength = wordItem.Text.Split(' ').Count();
                    string words = wordItem.Text.ToLower();
                    if (wordLength == 2 && words.Contains(word.ToLower()))
                    {
                        isValidWord = true;
                        break;
                    }
                }
            }

            if (isValidWord)
            {
                return Ok(new { message = "Từ '"+word+"' hợp lệ", word = word, status = true });
            }
            else
            {
                return Ok(new { message = "Từ '"+word+"' không hợp lệ", word = word, status = false });
            }
        }
    }
}
