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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PaiPaiGO.Controllers {
	public class Yu_CalendarController : Controller {

		private readonly PaiPaiGoContext _context;

		public Yu_CalendarController(PaiPaiGoContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Yu_Calendar()
		{
			ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
			ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");

			//要帶入登入會員的ID，待現在測試中，先設001
			var memberID = HttpContext.Session.GetString("MemberID");
			//var memberID = "001";


			//行事曆
			var OrderEventList = _context.Missions
				.Where(mission => mission.OrderMemberId == memberID && (mission.MissionStatus == "發布中"|| mission.MissionStatus == "進行中"))
				.Select(mission => new {
					id = mission.MissionId,
					title = mission.MissionName,
					start = mission.OrderTime,
					end = mission.DeliveryDate + mission.DeliveryTime,
					color = "#AFD9E4",
					type = mission.Category
				})
				.ToList();


			var AcceptEventList = _context.Missions
			.Where(mission => mission.AcceptMemberId == memberID && (mission.MissionStatus == "發布中" || mission.MissionStatus == "進行中"))
			.Select(mission => new {
				id = mission.MissionId,
				title = mission.MissionName,
				start = mission.AcceptTime,
				end = mission.DeliveryDate + mission.DeliveryTime,
				color = "#ABD9C5",
				type = mission.Category
			})
			.ToList();

			ViewBag.OrderEventList = OrderEventList;
			ViewBag.AcceptEventList = AcceptEventList;
			//行事曆


			//任務篩選下拉選單的內容
			var MissionStatus_Droplist = (from p in _context.Missions										  
										  select p.MissionStatus).Distinct();
			ViewBag.MissionStatus_Droplist = MissionStatus_Droplist.ToList();


			return _context.Missions != null ?
						View(await _context.Missions.ToListAsync()) :
						Problem("Entity set 'PaiPaiGoContext.Missions'  is null.");
		}


		//任務篩選
		[HttpPost]
		public ActionResult Filter_Order(string missionStatus)
		{
			//要帶入登入會員的ID，待現在測試中，先設001
			var memberID = HttpContext.Session.GetString("MemberID");
			//var memberID = "001";

			var query = _context.Missions.Include(x => x.CategoryNavigation).AsQueryable();
			query = query.Where(m => m.OrderMemberId == memberID);
			query = query.OrderByDescending(m => m.DeadlineDate);

			if (!string.IsNullOrEmpty(missionStatus)) // 如果選擇了狀態，則進行篩選
			{
				if(missionStatus== "待確認") {
				query = query.Where(m => m.MissionStatus == missionStatus|| m.MissionStatus== "等待接案人完成任務" || m.MissionStatus == "等待發布人完成任務");
				}
				else { query = query.Where(m => m.MissionStatus == missionStatus); }
				
			}
			var filteredData = query.ToList();
			// 根据筛选条件执行筛选操作
			// 将筛选结果返回到部分视图
			return PartialView("_OrderTable", filteredData);
		}


		[HttpPost]
		public ActionResult Filter_Accept(string missionStatus)
		{
			//要帶入登入會員的ID，待現在測試中，先設001
			var memberID = HttpContext.Session.GetString("MemberID");
			//var memberID = "001";

			var query = _context.Missions.Include(x => x.CategoryNavigation).AsQueryable();
			query = query.Where(m => m.AcceptMemberId == memberID);
			query = query.OrderByDescending(m => m.DeadlineDate);

			if (!string.IsNullOrEmpty(missionStatus)) // 如果選擇了狀態，則進行篩選
			{
				if (missionStatus == "待確認") {
					query = query.Where(m => m.MissionStatus == missionStatus || m.MissionStatus == "等待接案人完成任務" || m.MissionStatus == "等待發布人完成任務" );
				}
				else { query = query.Where(m => m.MissionStatus == missionStatus); }
			}
			var filteredData = query.ToList();
			// 根据筛选条件执行筛选操作
			// 将筛选结果返回到部分视图
			return PartialView("_AcceptTable", filteredData);
		}


		//取消任務
		[HttpPost]
		public IActionResult cancelMission(List<MissionStatusChangeModel> changes)
		{
			if (_context.Missions == null) {
				return Problem("Entity set 'PaiPaiGoContext.Missions' is null.");
			}

			foreach (var change in changes) {
				var mission = _context.Missions.FirstOrDefault(m => m.MissionId == change.MissionId);

				if (mission != null) {
					mission.MissionStatus = change.NewStatus;
				}
				if (change.NewStatus == "發布中") {
					mission.AcceptMemberId = "";
				}
			}
			try {
				_context.SaveChanges(); // 保存更改到SQL
				return Json(new { success = true }); // 丟回JSON
			}
			catch (Exception ex) {
				// 處理更新失敗
				return Json(new { success = false, error = ex.Message });
			}
		}

		[HttpPost]
		public IActionResult FinishMission(List<MissionFinishModel> changes)
		{
			if (_context.Missions == null) {
				return Problem("Entity set 'PaiPaiGoContext.Missions' is null.");
			}




			foreach (var change in changes) {
				//抓一個變數，用來確認有這個任務
				var mission = _context.Missions.FirstOrDefault(m => m.MissionId == change.MissionId);

				//確認有無任務
				if (mission != null) {

 if ((mission.MissionStatus == "等待接案人完成任務"&& change.MemberId != mission.OrderMemberId) ||
						(mission.MissionStatus == "等待發布人完成任務" && change.MemberId != mission.AcceptMemberId)) {
						//如果任務狀態是這兩個，代表已經有人完成了任務，那就再把任務
						mission.MissionStatus = change.NewStatus;
					}else if (mission.MissionStatus == "進行中") {
						//進行中代表還沒有人填，那就判斷是誰按下完成

						if (change.MemberId == mission.OrderMemberId) {
							//按下完成的是發布人
							mission.MissionStatus = "等待接案人完成任務";
						}
						if (change.MemberId == mission.AcceptMemberId) {
							//按下完成的是接單人
							mission.MissionStatus = "等待發布人完成任務";
						}
					}
				}
			}
			try {
				_context.SaveChanges(); // 保存更改到SQL
				return Json(new { success = true }); // 丟回JSON
			}
			catch (Exception ex) {
				// 處理更新失敗
				return Json(new { success = false, error = ex.Message });
			}
		}





		//評分星星的頁面
		public ActionResult Yu_Star(int missionId, string memberId, string othersId)
		{
			//登入狀態顯示
			ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
			ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");


			// 这里可以根据 missionId 和 memberId 从 A 资料表中获取相关数据
			// 然后将数据传递给评分页面
			ViewData["MissionId"] = missionId;
			ViewData["MemberId"] = memberId;
			ViewData["OthersId"] = othersId;
			DateTime currentTime = DateTime.Now;
			ViewData["Date"] = currentTime;

			int rowCount = _context.Opinions.Count() + 1;
			ViewData["Ratingnumber"] = rowCount.ToString("D2");

			return View();
		}


		[HttpPost]
		public async Task<IActionResult> Yu_Star([Bind("Ratingnumber,Type,Date,,MissionId,ReportMemberId,ReportedMemberId,Content,State,Score,Mission,ReportMember,ReportedMember")] Opinion_Star model)
		{
			if (ModelState.IsValid) {
				// 从 model 中获取评分数据，并将其保存到 B 资料表中
				// 你可以使用 Entity Framework 或其他数据访问技术来进行数据库操作
				// 保存成功后，重定向到之前的页面或其他适当的操作

				var ratingData = new Opinion
				{
					Ratingnumber = model.Ratingnumber,
					Type = model.Type,
					Date = model.Date,
					MissionId = model.MissionId,
					ReportMemberId = model.ReportMemberId,
					ReportedMemberId = model.ReportedMemberId,
					Content = model.Content,
					State = model.State,
					Score = model.Score
				};

				_context.Add(ratingData);
				await _context.SaveChangesAsync();

				return RedirectToAction(nameof(Yu_Calendar));
			}
			// 返回视图并传递参数			
			return View("Yu_Star", model);
			//return View("Yu_Star", model); // 如果评分数据验证失败，返回评分页面并显示错误信息
		}

		//來做檢舉頁面，應該會和星星頁面差不多(只是把星星拿掉、選項換掉，還有登進資料庫時，要把類別設定為檢舉)
		public ActionResult Yu_Report(int missionId, string memberId, string othersId)
		{
			//登入狀態顯示
			ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
			ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
			// 这里可以根据 missionId 和 memberId 从 A 资料表中获取相关数据
			// 然后将数据传递给评分页面
			ViewData["MissionId"] = missionId;
			ViewData["MemberId"] = memberId;
			ViewData["OthersId"] = othersId;
			DateTime currentTime = DateTime.Now;
			ViewData["Date"] = currentTime;

			int rowCount = _context.Opinions.Count() + 1;
			ViewData["Ratingnumber"] = rowCount.ToString("D2");

			return View();
		}
	}
}
