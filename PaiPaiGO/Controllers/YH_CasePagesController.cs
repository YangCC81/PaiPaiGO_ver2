using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using PaiPaiGO.Models;
using System.Reflection;

namespace PaiPaiGo.Controllers
{

    public class YH_CasePagesController : Controller
    {

        private readonly PaiPaiGoContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public YH_CasePagesController(PaiPaiGoContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }


		// GET: YH_CasePages
		public async Task<IActionResult> YH_CasePage(string sortOrder, string image, string searchString, int? page, string category)
		{

			//layout用
			ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
			ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
			if (!string.IsNullOrEmpty(category))
			{
				page = 1; // Reset page number if category is changed
			}
			//搜尋框
			if (_context.Missions == null)
            {
                return Problem("Entity set 'MIsson'  is null.");
            }
            var missions = from m in _context.Missions
                           select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                missions = missions.Where(s => s.MissionName!.Contains(searchString));
            }
            //圖片
            //byte[] Imgdata = null;
            
            //HttpContext.Session.TryGetValue(image, out Imgdata);
            //if (Imgdata != null)
            //{
            //    string base64Image = Convert.ToBase64String(Imgdata);
            //    ViewBag.mission64Image = "data:image/png;base64,"+base64Image; ;
            //}

            //return View(await missions.ToListAsync());
            var paiPaiGoContext = _context.Missions.Include(m => m.CategoryNavigation);

            //分頁,一頁8個card
            var pageNumber = page ?? 1;
            return View(await missions.ToPagedListAsync(pageNumber, 8));
        }
        [HttpGet]
        public async Task<IActionResult> GetMissionImage(int id)
        {
			//layout用
			ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
			ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
			var mission = await _context.Missions.FindAsync(id);

            if (mission == null || mission.ImagePath == null || mission.ImagePath.Length == 0)
            {
                // Return default image
                var defaultImagePath = Path.Combine(_hostingEnvironment.WebRootPath, "img", "Home1.png");
                return PhysicalFile(defaultImagePath, "image/jpeg");
            }

            return File(mission.ImagePath, "image/jpeg"); // Assuming the image type is jpeg. Modify MIME type if different.
        }

        [HttpGet]

        public IActionResult GetMissionsByCategory(string category)
        {
			//layout用
			ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
			ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
			var missions = _context.Missions
                        .Where(m => m.MissionStatus == "發布中")
                        .ToList();
			
			return PartialView("_MissionsPartial", missions);
        }
		[HttpPost]
		public async Task<IActionResult> GetFilterData(int? selectedCategory, string selectedStatus, string selectedZipcode, int? page)
		{
			//layout用
			ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
			ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
			// 從資料庫中取得資料，預先加載相關的Category
			var query = _context.Missions.Include(x => x.CategoryNavigation).AsQueryable();

			// 過濾：根據Category
			if (selectedCategory> 0)
			{
				query = query.Where(m => m.Category == selectedCategory);
			}

			// 過濾：根據Status
			if (!string.IsNullOrEmpty(selectedStatus))
			{
				query = query.Where(m => m.MissionStatus == selectedStatus);
			}

			// 過濾：根據Zipcode
			if (!string.IsNullOrEmpty(selectedZipcode))
			{
				query = query.Where(m => m.Postcode == selectedZipcode);
			}

			// 執行查詢和分頁
			var filterData = await query.ToListAsync();
			var pageNumber = page ?? 1;

			// 返回PartialView結果
			return PartialView("_MissionsPartial", await filterData.ToPagedListAsync(pageNumber, 8));
		}


		[HttpPost]
		public IActionResult Map(string city, string district)
		{
			//layout用
			ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
			ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
			var orders = _context.Missions
								 .Where(o => o.LocationCity == city && o.LocationDistrict == district)
								 .Select(o => new { o.LocationCity, o.LocationDistrict, o.Address })
								 .ToList();

			return Json(new { orders = orders });
		}


		




		// GET: YH_CasePages/Details/5
		public async Task<IActionResult> Details(int? id)
        {
			//layout用
			ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
			ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
			if (id == null || _context.Missions == null)
            {
                return NotFound();
            }

            var mission = await _context.Missions
                .Include(m => m.CategoryNavigation)
                .FirstOrDefaultAsync(m => m.MissionId == id);
            if (mission == null)
            {
                return NotFound();
            }

            return View(mission);
        }

        // GET: YH_CasePages/Create
        public IActionResult Create()
        {
			//layout用
			ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
			ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
			ViewData["Category"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
            return View();
        }

        // POST: YH_CasePages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MissionId,Category,Tags,OrderMemberId,AcceptMemberId,MissionName,MissionAmount,Postcode,FormattedMissionAmount,LocationCity,LocationDistrict,Address,DeliveryDate,DeliveryTime,DeadlineDate,DeadlineTime,MissionDescription,DeliveryMethod,ExecutionLocation,MissionStatus,OrderTime,AcceptTime,ImagePath")] Mission mission)
        {
			//layout用
			ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
			ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
			if (ModelState.IsValid)
            {
                _context.Add(mission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Category"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", mission.Category);
            return View(mission);
        }

        // GET: YH_CasePages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
			//layout用
			ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
			ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
			if (id == null || _context.Missions == null)
            {
                return NotFound();
            }

            var mission = await _context.Missions.FindAsync(id);
            if (mission == null)
            {
                return NotFound();
            }
            ViewData["Category"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", mission.Category);
            return View(mission);
        }

        // POST: YH_CasePages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MissionId,Category,Tags,OrderMemberId,AcceptMemberId,MissionName,MissionAmount,Postcode,FormattedMissionAmount,LocationCity,LocationDistrict,Address,DeliveryDate,DeliveryTime,DeadlineDate,DeadlineTime,MissionDescription,DeliveryMethod,ExecutionLocation,MissionStatus,OrderTime,AcceptTime,ImagePath")] Mission mission)
        {
			//layout用
			ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
			ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
			if (id != mission.MissionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MissionExists(mission.MissionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Category"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", mission.Category);
            return View(mission);
        }

        // GET: YH_CasePages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
			//layout用
			ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
			ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
			if (id == null || _context.Missions == null)
            {
                return NotFound();
            }

            var mission = await _context.Missions
                .Include(m => m.CategoryNavigation)
                .FirstOrDefaultAsync(m => m.MissionId == id);
            if (mission == null)
            {
                return NotFound();
            }

            return View(mission);
        }

        // POST: YH_CasePages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
			//layout用
			ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
			ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
			if (_context.Missions == null)
            {
                return Problem("Entity set 'PaiPaiGoContext.Missions'  is null.");
            }
            var mission = await _context.Missions.FindAsync(id);
            if (mission != null)
            {
                _context.Missions.Remove(mission);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MissionExists(int id)
        {
          return (_context.Missions?.Any(e => e.MissionId == id)).GetValueOrDefault();
        }
    }
}

	