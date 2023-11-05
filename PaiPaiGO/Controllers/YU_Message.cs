using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PaiPaiGO.Models;
using System;
using System.Globalization;
using static PaiPaiGO.Models.Abandoned;


namespace PaiPaiGO.Controllers
{

	public class YU_Message : Controller
	{

		private readonly PaiPaiGoContext _context;
		public DateTime Date { get; set; }

		public YU_Message(PaiPaiGoContext context)
		{
			_context = context;
		}
		// 測試
		public string Index()
		{
			return "apple123456";
		}
		#region 客服回寫到資料庫
		[HttpPost]
		public ActionResult SubmitChat(string yu, string mi, int missionId)
		{
			// _Layout連線
			ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
			ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
			DateTime currentTime = DateTime.Now;

			// 確保該資料的欄位，進行客服的時間回傳
			using (var mmcontext = new PaiPaiGoContext())
			{
				var Abandoned = new Abandoned
				{
					MissionId = missionId,
					MemberId = ViewBag.YU_ID,
					Date = currentTime,
					Chat = mi,
					
				};
				mmcontext.Abandoneds.Add(Abandoned);
				mmcontext.SaveChanges();
			}
			//return Json(new { success = true, message = "您好，已收到訊息，謝謝您的回饋，將盡速為您處理。" });
			return Ok("");
		}
		#endregion

		#region 確認資料庫有資料可以取得

		public ActionResult GetMemberMissions(object JsonRequestBehavior)
		{
			// 假設你已經有一個使用者登入的Session，這個Session中包含了會員的資訊，例如會員的ID
			var memberId = HttpContext.Session.GetString("MemberID");
		
			using (var mmcontext = new PaiPaiGoContext())
			{
				// 根據會員的ID，檢索該會員所掛的MissionID列表
				var memberMissions = mmcontext.Missions
					.Where(a => a.AcceptMemberId == memberId && a.MissionStatus == "進行中")
					.Select(a => a.MissionId)
					.ToList();

				// 將MissionID列表傳送到前端
				return Json(memberMissions);
			}
		}
		#endregion
	}
}