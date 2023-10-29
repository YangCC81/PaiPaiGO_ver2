using PaiPaiGO.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;

namespace PaiPaiGO.Controllers
{

    public class HomeController : Controller
    {
       
        private readonly PaiPaiGoContext _context;


        public HomeController(PaiPaiGoContext context)
        {
            _context = context;
        }
        #region 倒數時間測試
        public IActionResult Index()
        {
            //測試是否可以從資料庫帶出資料
            var temp = from x in _context.Missions
                       where x.MissionId == 2023092801
                       select new
                       {
                           x.MissionId,
                           x.DeadlineDate
                       };

            var mission = temp.FirstOrDefault();  // 取得第一個符合條件的任務

            if (mission != null)
            {
                DateTime deadlineDateFromDatabase = mission.DeadlineDate;
                DateTime currentTime = DateTime.Now;

                TimeSpan remainingTime = deadlineDateFromDatabase - currentTime;


                //進行剩餘時間的處理，比如輸出或者其他操作
                ViewBag.remainingtime = $"{remainingTime.Days} 天 {remainingTime.Hours} 小時 {remainingTime.Minutes} 分鐘 {remainingTime.Seconds} 秒";
            }

            ViewBag.Miss = temp.SingleOrDefault();
            return View();
        }

        #endregion

        #region 倒數時間(取得)
        [HttpGet]
        public IActionResult RemainingTimePartial()
        {
            //保持連線
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");

            var memberId = HttpContext.Session.GetString("MemberID");
            // 取得 Mission 資料
            var mission = _context.Missions
                                .Where(x => x.AcceptMemberId == memberId && x.DeliveryDate >= DateTime.Now && x.MissionStatus == "進行中")
                                .OrderBy(x => x.DeliveryDate)
                                .FirstOrDefault();

            

			//(mission != null && member != null)
			if (mission != null)
            {
				
				// 將我資料的日期與時間一起帶入
				DateTime deliveryDateTime = mission.DeliveryDate + mission.DeliveryTime;
                DateTime currentTime = DateTime.Now;
                TimeSpan remainingTime = deliveryDateTime - currentTime;

                //將剩餘時間以日時分秒的形式返回
                var remainingTimeString = $"{remainingTime.Days} 天 {remainingTime.Hours} 小時 {remainingTime.Minutes} 分鐘 {remainingTime.Seconds} 秒";
				
				var viewModel = new MissionViewModel
				{
					MissionName = mission.MissionName,
					RemainingTime = remainingTimeString
				};
				//將剩餘時間作為部分視圖的模型返回
				return PartialView("_RemainingTimePartial", viewModel);
			}

		        //如果 mission 或 member 為 null，表示沒有相應的數據
		        return PartialView("_RemainingTimePartial", "");
            
			
        }


        #endregion




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
