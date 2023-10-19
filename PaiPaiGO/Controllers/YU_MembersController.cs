using PaiPaiGO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Lab1006_Time.Controllers {
    public class YU_MembersController : Controller {
        private readonly PaiPaiGoContext _context;

        public YU_MembersController(PaiPaiGoContext context) {
            _context = context;
        }
        // GET: Mission/UnfinishedMissions
        //public IActionResult UnfinishedMissions()
        //{

        //    // 獲取當前會員的 ID
        //    string memberId = HttpContext.Session.GetString("MemberID")!;

        //    // 檢索未完成任務
        //    var unfinishedMissions = _context.Missions
        //        .Where(m => m.AcceptMemberId == memberId && m.MissionStatus == "已接單未完成")
        //        .ToList();

        //    ViewBag.UnfinishedMissions = unfinishedMissions; // Or use a ViewModel to pass data to the view

        //    return View(unfinishedMissions);
        //}




        public IActionResult Index() {
            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            var status = from y in _context.Missions
                         where y.MissionStatus == "已接單未完成"
                         select new {
                             y.AcceptMemberId,
                             y.MissionStatus
                         };
            var str = status.FirstOrDefault(); // 取得第一個符合條件的任務
            ViewBag.Status = status.SingleOrDefault();

            return View();

        }

    }
}


