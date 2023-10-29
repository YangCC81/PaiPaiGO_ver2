using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaiPaiGO.Models;
using X.PagedList;

namespace PaiPaiGo.Controllers
{
    public class WS_AdmMembersController : Controller
    {
        private readonly PaiPaiGoContext _context;

        public WS_AdmMembersController(PaiPaiGoContext context)
        {
            _context = context;
        }


        //改封鎖狀況
        [HttpPost]
        public IActionResult UpdateStatus(List<MemberStatusChangeModel> changes)
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");

            if (_context.Missions == null)
            {
                return Problem("Entity set 'PaiPaiGoContext.Members' is null.");
            }

            foreach (var change in changes)
            {
                var member = _context.Members.FirstOrDefault(m => Convert.ToInt32(m.MemberId) == change.MemberId);

                if (member != null)
                {
                    member.MemberStatus = change.NewStatus;
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

        public async Task<IActionResult> AdmMember(string memberStatusFilter, int? page)
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");

            
            if (string.IsNullOrEmpty(memberStatusFilter))
            {
                // 初始加載時，return完整view包含ViewBags
                var allMembers = await _context.Members.ToPagedListAsync(page ?? 1, 5);
                var selectList = _context.Members.Select(m => m.MemberStatus).Distinct().ToList();
                ViewBag.MemberStatusList = new SelectList(selectList, selectList); // 为 ViewBags 设置数据
                return View(allMembers);
            }
            else
            {
               var filteredMembers = await _context.Members
                   .Where(m => m.MemberStatus == memberStatusFilter)
                   .ToPagedListAsync(page ?? 1, 5);
                return PartialView("_MemberListPartial", filteredMembers);
            }
        }
        

        public IActionResult FilterMembersByStatus(string memberStatus)
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            // 根據選擇的狀態篩選會員數據
            var filteredMembers = _context.Members
                .Include(m => m.MemberPostcodeNavigation)
                .Where(m => string.IsNullOrEmpty(memberStatus) || m.MemberStatus == memberStatus)
                .ToList();

            // 返回部分視圖
            return PartialView("_MemberListPartial", filteredMembers);
        }

      

        // GET: WS_AdmMembers/Details/5
        public async Task<IActionResult> Details(string id)
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            if (id == null || _context.Members == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.MemberPostcodeNavigation)
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }



        

        private bool MemberExists(string id)
        {
          return (_context.Members?.Any(e => e.MemberId == id)).GetValueOrDefault();
        }
    }
}
