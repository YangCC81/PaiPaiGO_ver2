using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaiPaiGO.Models;
using static System.Formats.Asn1.AsnWriter;

namespace PaiPaiGO.Controllers {
	public class Yu_CalendarController : Controller {

		private readonly PaiPaiGoContext _context;

		public Yu_CalendarController(PaiPaiGoContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Yu_Calendar()
		{
			var myOrderMission = from p in _context.Missions
								 orderby p.DeadlineDate descending
								 where p.OrderMemberId == "001" //這裡之後要代入登入會員的ID
								 select p;
			var myAcceptMission = from p in _context.Missions
								  orderby p.DeliveryDate descending
								  where p.AcceptMemberId == "001"
								  select p;

			var MissionStatus_Droplist = (from p in _context.Missions
										  select p.MissionStatus).Distinct();



			//if (!String.IsNullOrEmpty(searchString)) {
			//	students = students.Where(s => s.LastName.Contains(searchString)
			//						   || s.FirstMidName.Contains(searchString));
			//}

			ViewBag.myOrderMission = myOrderMission.ToArray();

			ViewBag.myAcceptMission = myAcceptMission.ToArray();

			ViewBag.MissionStatus_Droplist = MissionStatus_Droplist.ToList();



			return _context.Missions != null ?
						View(await _context.Missions.ToListAsync()) :
						Problem("Entity set 'PaiPaiGoContext.Missions'  is null.");
		}



		//評分頁面
		public ActionResult Yu_Star(int missionId, int memberId, int othersId)
		{
			// 这里可以根据 missionId 和 memberId 从 A 资料表中获取相关数据
			// 然后将数据传递给评分页面
			ViewData["MissionId"] = missionId;
			ViewData["MemberId"] = memberId;
			ViewData["OthersId"] = othersId;
			DateTime currentTime = DateTime.Now;
			ViewData["Date"] = currentTime;

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Yu_Star([Bind("Ratingnumber,Type,Date,,MissionId,ReportMemberId,ReportedMemberId,Content,State,Score,Mission,ReportMember,ReportedMember")] Opinion model)
		{
			if (ModelState.IsValid) {
				// 从 model 中获取评分数据，并将其保存到 B 资料表中
				// 你可以使用 Entity Framework 或其他数据访问技术来进行数据库操作
				// 保存成功后，重定向到之前的页面或其他适当的操作


				//var ratingData = new Opinion
				//{
				//	Type=model.Type,
				//	Date=model.Date,
				//	MissionId=model.MissionId,
				//	ReportMemberId=model.ReportMemberId,
				//	ReportedMemberId=model.ReportedMemberId,
				//	Content=model.Content,
				//	State=model.State,
				//	Score = model.Score
				//};

				_context.Add(model);
				await _context.SaveChangesAsync();

				return RedirectToAction(nameof(Yu_Calendar));
			}
			return View(model);
			//return View("Yu_Star", model); // 如果评分数据验证失败，返回评分页面并显示错误信息
		}


		//要拉行事曆的JSON
		public class eventList {
			public int id { get; set; }
			public string title { get; set; }
			public string start { get; set; }
			public string end { get; set; }

		}


		//我想把資料庫裡的資料變成JSON檔案讓行事曆取用
		//但不知為何雖然成功弄成了JSON，但行事曆跑不出來
		public JsonResult GetEvents()
		{
			var myOrderMission = from p in _context.Missions
								 orderby p.DeadlineDate ascending
								 where p.OrderMemberId == "001"
								 select p;

			// 从数据库中检索事件数据
			//https://localhost:7277/Yu_Calendar/GetEvents
			// 将数据库数据转换为事件对象
			List<eventList> myOrderMissionList = new List<eventList>();


			foreach (var item in myOrderMission.ToArray()) {
				eventList customMission = new eventList
				{
					id = item.MissionId,
					title = item.MissionName,
					start = "2023-09-20 10:00:00",
					end = "2023-09-21 10:00:00"

				};
				myOrderMissionList.Add(customMission);
			}

			var options1 = new JsonSerializerOptions
			{
				Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs),
				WriteIndented = true
			};


			return Json(myOrderMissionList);
		}

	}
}
