using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaiPaiGO.Models;

namespace PaiPaiGo.Controllers
{
    public class WS_OpinionsController : Controller
    {
        private readonly PaiPaiGoContext _context;

        public WS_OpinionsController(PaiPaiGoContext context)
        {
            _context = context;
        }


		//改狀態
		[HttpPost]
		public IActionResult UpdateOpinionStatus(List<OpinionStatusUpdate> changedStatusList)
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            if (ModelState.IsValid)
			{
				// 遍历changedStatusList并更新数据库中Opinion的状态
				foreach (var statusUpdate in changedStatusList)
				{
					var opinion = _context.Opinions.Find(statusUpdate.Ratingnumber);
					if (opinion != null)
					{
						opinion.State = statusUpdate.NewStatus;
					}
				}
				// 保存更改
				_context.SaveChanges();

				// 返回成功的响应
				return Ok(new { message = "状态更新成功" });
			}

			// 如果出现错误，返回错误的响应
			return BadRequest(new { message = "状态更新失败" });
		}

		//新增警告
		// GET: WS_Opinions/AddOpinion
		public IActionResult AddOpinion()
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            // 在這裡處理新增警告到資料庫的邏輯
            // 使用你的Opinion模型來執行新增操作
            string warningMessage = Request.Form["warningMessage"]; // 從請求中獲取警告消息

			// 在這裡執行將警告消息新增到資料庫的操作
			using (var dbContext = new PaiPaiGoContext())
			{
				var opinion = new Opinion { Warn = warningMessage };
				dbContext.Opinions.Add(opinion);
				dbContext.SaveChanges();
			}

			// 返回JSON回應，表示成功
			return Json(new { success = true });
		}

		// GET: WS_Opinions
		public async Task<IActionResult> AdmOpinion()
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            var paiPaiGoContext = _context.Opinions.Include(o => o.Mission).Include(o => o.ReportMember).Include(o => o.ReportedMember);
            return View(await paiPaiGoContext.ToListAsync());
        }



        // GET: WS_Opinions/Edit/5
        public async Task<IActionResult> Edit(string id)
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            if (id == null || _context.Opinions == null)
            {
                return NotFound();
            }

            var opinion = await _context.Opinions.FindAsync(id);
            if (opinion == null)
            {
                return NotFound();
            }
            ViewData["MissionId"] = new SelectList(_context.Missions, "MissionId", "MissionId", opinion.MissionId);
            ViewData["ReportMemberId"] = new SelectList(_context.Members, "MemberId", "MemberId", opinion.ReportMemberId);
            ViewData["ReportedMemberId"] = new SelectList(_context.Members, "MemberId", "MemberId", opinion.ReportedMemberId);
            return View(opinion);
        }



        private bool OpinionExists(string id)
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            return (_context.Opinions?.Any(e => e.Type == id)).GetValueOrDefault();
        }
    }
}
