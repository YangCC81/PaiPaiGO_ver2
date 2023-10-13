using PaiPaiGO.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Data.SqlClient;

namespace Lab1006_Time.Controllers {

    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly PaiPaiGoContext _context;


        public HomeController(ILogger<HomeController> logger, PaiPaiGoContext context) {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index() {
            //測試是否可以從資料庫帶出資料
            var temp = from x in _context.Missions
                       where x.MissionId == 2023092801
                       select new {
                           x.MissionId,
                           x.DeadlineDate
                       };

            var mission = temp.FirstOrDefault();  // 取得第一個符合條件的任務

            if (mission != null) {
                DateTime deadlineDateFromDatabase = mission.DeadlineDate;
                DateTime currentTime = DateTime.Now;

                TimeSpan remainingTime = deadlineDateFromDatabase - currentTime;


                //進行剩餘時間的處理，比如輸出或者其他操作
                ViewBag.remainingtime = $"{remainingTime.Days} 天 {remainingTime.Hours} 小時 {remainingTime.Minutes} 分鐘 {remainingTime.Seconds} 秒";
            }

            ViewBag.Miss = temp.SingleOrDefault();
            return View();
        }

        public IActionResult RemainingTimePartial() {
            //從資料庫帶出資料後，呈現在前端且時間的秒數會跟著動
            var temp = from x in _context.Missions
                       where x.MissionId == 2023092801
                       select new {
                           x.MissionId,
                           x.DeadlineDate
                       };

            var mission = temp.FirstOrDefault();  // 取得第一個符合條件的任務

            if (mission != null) {
                DateTime deadlineDateFromDatabase = mission.DeadlineDate;
                DateTime currentTime = DateTime.Now;

                TimeSpan remainingTime = deadlineDateFromDatabase - currentTime;

                // 將剩餘時間以日時分秒的形式返回
                var remainingTimeString = $"{remainingTime.Days} 天 {remainingTime.Hours} 小時 {remainingTime.Minutes} 分鐘 {remainingTime.Seconds} 秒";

                // 將剩餘時間作為部分視圖的模型返回
                return PartialView("_RemainingTimePartial", remainingTimeString);
            }

            return PartialView("_RemainingTimePartial", null);
        }



        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}