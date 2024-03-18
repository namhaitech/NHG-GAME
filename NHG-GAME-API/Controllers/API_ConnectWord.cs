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
        [HttpGet("getword")]
        public IActionResult GetWord()
        {
            string filePath = @"Data\data_words.txt";
            var lines = System.IO.File.ReadAllLines(filePath);
            List<string> filteredWords = new List<string>();
            foreach (var line in lines)
            {
                var wordItem = JsonConvert.DeserializeObject<DataWord>(line);
                if (wordItem != null)
                {
                    string[] words = wordItem.Text.Split(' ');
                    if (words.Length == 2)
                    {
                        string word = wordItem.Text.ToLower();
                        filteredWords.Add(word);
                    }
                }
            }

            Random random = new Random();
            int randomIndex = random.Next(0, filteredWords.Count);
            string randomWord = filteredWords[randomIndex];
            return Ok(new { message = "Lấy từ thành công", word = randomWord, status = true });
        }

        [HttpGet("getnextword={word}")]
        public IActionResult GetNextWord(string word)
        {
            string filePath = @"Data\data_words.txt";
            var lines = System.IO.File.ReadAllLines(filePath);
            string nextword = "";
            foreach (var line in lines)
            {
                var wordItem = JsonConvert.DeserializeObject<DataWord>(line);
                if (wordItem != null)
                {
                    string[] words = wordItem.Text.Split(' ');
                    if (words.Length == 2 && wordItem.Text.ToLower().StartsWith(word.ToLower()))
                    {
                        nextword = wordItem.Text;
                    }
                }
            }
            return Ok(new { message = "Lấy từ thành công", word = nextword, status = true });
        }

        [HttpPost("question={question}&answer={answer}")]
        public IActionResult CheckWordIsTrue(string question, string answer)
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
                    if (wordLength == 2 && words.Equals(question.ToLower() + " " + answer.ToLower()))
                    {
                        isValidWord = true;
                        break;
                    }
                }
            }

            if (isValidWord)
            {
                return Ok(new { message = "Từ '"+answer+"' hợp lệ", word = answer, status = true });
            }
            else
            {
                return Ok(new { message = "Từ '"+answer+"' không hợp lệ", word = answer, status = false });
            }
        }
    }
}
