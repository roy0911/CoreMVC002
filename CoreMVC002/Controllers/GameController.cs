using CoreMVC002.Models;
using CoreMVC002.Models.CoreMVC002.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

namespace CoreMVC002.Controllers
{
    public class GameController : Controller
    {
        private const string AnswerKey = "SecretAnswer";
        private const string CountKey = "GuessCount";
        private const string HistoryKey = "GuessHistory";

        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(AnswerKey)))
            {
                HttpContext.Session.SetString(AnswerKey, XAXBEngine.GenerateAnswer());
                HttpContext.Session.SetInt32(CountKey, 0);
                HttpContext.Session.SetString(HistoryKey, JsonSerializer.Serialize(new List<string>()));
            }

            LoadSessionData();
            return View();
        }

        [HttpPost]
        public IActionResult Guess(string userGuess)
        {
            string answer = HttpContext.Session.GetString(AnswerKey);
            if (string.IsNullOrEmpty(answer))
            {
                answer = XAXBEngine.GenerateAnswer();
                HttpContext.Session.SetString(AnswerKey, answer);
                HttpContext.Session.SetInt32(CountKey, 0);
                HttpContext.Session.SetString(HistoryKey, JsonSerializer.Serialize(new List<string>()));
            }

            int count = (HttpContext.Session.GetInt32(CountKey) ?? 0) + 1;
            HttpContext.Session.SetInt32(CountKey, count);

            var history = GetHistory();

            if (string.IsNullOrEmpty(userGuess) || userGuess.Length != 4)
            {
                ViewBag.Message = "請輸入 4 位數數字！";
                LoadSessionData();
                return View("Index");
            }

            string result = XAXBEngine.Check(answer, userGuess);
            string record = $"第 {count} 次: {userGuess} ➔ {result}";

            history.Add(record);
            HttpContext.Session.SetString(HistoryKey, JsonSerializer.Serialize(history));

            ViewBag.Result = record;
            LoadSessionData();

            if (result == "4A0B")
            {
                ViewBag.Message = $"🎉 恭喜你猜對了！總共猜了 {count} 次。";
                HttpContext.Session.Remove(AnswerKey);
            }

            return View("Index");
        }

        public IActionResult Reset()
        {
            HttpContext.Session.Remove(AnswerKey);
            HttpContext.Session.Remove(CountKey);
            HttpContext.Session.Remove(HistoryKey);
            return RedirectToAction("Index");
        }

        private void LoadSessionData()
        {
            ViewBag.Count = HttpContext.Session.GetInt32(CountKey) ?? 0;
            ViewBag.History = GetHistory();
        }

        private List<string> GetHistory()
        {
            var historyJson = HttpContext.Session.GetString(HistoryKey);
            return string.IsNullOrEmpty(historyJson) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(historyJson);
        }
    }
}