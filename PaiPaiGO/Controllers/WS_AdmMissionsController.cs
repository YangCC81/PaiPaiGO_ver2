using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using PaiPaiGO.Models;
using System.Reflection;

namespace PaiPaiGo.Controllers
{
   
    public class WS_AdmMissionsController : Controller
    {
        private readonly PaiPaiGoContext _context;


        public WS_AdmMissionsController(PaiPaiGoContext context)
        {
            _context = context;
        }



		// GET: Missions
		public async Task<IActionResult> WS_AdmMission(string sortOrder, string currentFilter, string searchString, int? page)
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");

            if (_context.Missions != null)
			{
                var result = _context.Missions.Include(x => x.CategoryNavigation).ToList();
                //排隊或代購
                MissionCategoyViewModel MissionCatagoryViewModel = new MissionCategoyViewModel();
                MissionCatagoryViewModel.MissionCategory = result.Select(x => x.CategoryNavigation).Distinct().Select(x => new SelectListItem() { Value = x.CategoryId.ToString(), Text = x.CategoryName.ToString() }).ToList();
				//MissionCatagoryViewModel.MissionCategory = result.Select(x => x.Category).Distinct().ToList().Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() }).ToList();
                ViewData["MissionCatagoryViewModel"] = MissionCatagoryViewModel;
				//任務狀態
				MissionStatusViewModel MissionStatusViewModel = new MissionStatusViewModel();
				MissionStatusViewModel.MissionStatus = result.Select(y => y.MissionStatus).Distinct().ToList().Select(y => new SelectListItem() { Value = y, Text = y}).ToList();
				ViewData["MissionStatusViewModel"] = MissionStatusViewModel;



				//搜尋框
				if (_context.Missions == null)
				{
					return Problem("Entity set 'PaiPaiGoContext.Mission'  is null.");
				}
				var missions = from m in _context.Missions
							   select m;
				if (!String.IsNullOrEmpty(searchString))
				{
					missions = missions.Where(s => s.MissionName!.Contains(searchString));
				}
				//return View(await missions.ToListAsync());
				var PaiPaiGoContext = _context.Missions.Include(m => m.CategoryNavigation);


				//頁面分頁
				var pageNumber = page ?? 1;
				return View(await missions.ToPagedListAsync(pageNumber, 5));

			}
			return Problem("Entity set 'MissionContext.Tasks'  is null.");
		}
        //改上下架
		[HttpPost]
		public IActionResult SaveMissionStatusChanges(List<MissionStatusChangeModel> changes)
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            if (_context.Missions == null)
			{
				return Problem("Entity set 'PaiPaiGoContext.Missions' is null.");
			}

			foreach (var change in changes)
			{
				var mission = _context.Missions.FirstOrDefault(m => m.MissionId == change.MissionId);

				if (mission != null)
				{
					mission.MissionStatus = change.NewStatus;
				}
			}

			try
			{
				_context.SaveChanges(); // 保存更改到SQL
				return Json(new { success = true }); // 丟回JSON
			}
			catch (Exception ex)
			{
				// 處理更新失敗
				return Json(new { success = false, error = ex.Message });
			}
		}
		//篩選
		[HttpPost]
		public async Task<IActionResult> GetFilteredData(int selectedCategory, string selectedStatus, string selectedZipcode, int? page)
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            // 使用 Entity Framework Core 建立查詢，篩選選擇的條件
            var query = _context.Missions.Include(x => x.CategoryNavigation).AsQueryable();

			if (selectedCategory > 0) // 如果選擇了種類，則進行篩選
			{
				query = query.Where(m => m.Category == selectedCategory);
			}

			if (!string.IsNullOrEmpty(selectedStatus)) // 如果選擇了狀態，則進行篩選
			{
				query = query.Where(m => m.MissionStatus == selectedStatus);
			}

			if (!string.IsNullOrEmpty(selectedZipcode)) // 如果選擇了Zipcode，則進行篩選
			{
				query = query.Where(m => m.Postcode == selectedZipcode);
			}

			// 執行查詢，獲取過濾後的數據
			var filteredData = await query.ToListAsync();


			// 使用 PartialView 返回過濾後的數據
			
			var pageNumber = page ?? 1;
			return PartialView("_MissionTable", await filteredData.ToPagedListAsync(pageNumber, 5));
		}





        // GET: Missions/Details/5
        public async Task<IActionResult> Details(int? id)
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            if (id == null || _context.Missions == null)
            {
                return NotFound();
            }

            var mission = await _context.Missions
                .FirstOrDefaultAsync(m => m.MissionId == id);
            if (mission == null)
            {
                return NotFound();
            }

            return View(mission);
        }

        // GET: Missions/Create
        public IActionResult Create()
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            return View();
        }

        

        private bool MissionExists(int id)
        {
          return (_context.Missions?.Any(e => e.MissionId == id)).GetValueOrDefault();
        }


    }
	
}
