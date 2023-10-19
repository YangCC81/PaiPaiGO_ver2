using System;
using System.Collections.Generic;
using System.Linq;
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
            if (_context.Members == null)
			{
				return Problem("Entity set 'PaiPaiGoContext.Members' is null.");
			}

			foreach (var change in changes)
			{
				var member = _context.Members.FirstOrDefault(m =>Convert.ToInt32( m.MemberId )== change.MemberId);

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

		public async Task<IActionResult> AdmMember(string memberStatusFilter)
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            if (string.IsNullOrEmpty(memberStatusFilter))
            {
                // 初始加载时，返回完整视图并包含 ViewBags
                var allMembers = await _context.Members.ToListAsync();
                var selectList = _context.Members.Select(m => m.MemberStatus).Distinct().ToList();
                ViewBag.MemberStatusList = new SelectList(selectList, selectList); // 为 ViewBags 设置数据
                return View(allMembers);
            }
            else
            {
               var filteredMembers = await _context.Members
                   .Where(m => m.MemberStatus == memberStatusFilter)
                   .ToListAsync();
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

        // GET: WS_AdmMembers/Create
        public IActionResult Create()
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            ViewData["MemberPostcode"] = new SelectList(_context.Counties, "Postcode", "Postcode");
            return View();
        }

        // POST: WS_AdmMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,MemberName,MemberPhoneNumber,MemberPostcode,MemberCity,MemberTownship,MemberAddress,MemberEmail,MemberStatus,MemberPassword,Gearing")] Member member)
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberPostcode"] = new SelectList(_context.Counties, "Postcode", "Postcode", member.MemberPostcode);
            return View(member);
        }

        // GET: WS_AdmMembers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            if (id == null || _context.Members == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            ViewData["MemberPostcode"] = new SelectList(_context.Counties, "Postcode", "Postcode", member.MemberPostcode);
            return View(member);
        }

        // POST: WS_AdmMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MemberId,MemberName,MemberPhoneNumber,MemberPostcode,MemberCity,MemberTownship,MemberAddress,MemberEmail,MemberStatus,MemberPassword,Gearing")] Member member)
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            if (id != member.MemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.MemberId))
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
            ViewData["MemberPostcode"] = new SelectList(_context.Counties, "Postcode", "Postcode", member.MemberPostcode);
            return View(member);
        }

        // GET: WS_AdmMembers/Delete/5
        public async Task<IActionResult> Delete(string id)
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

        // POST: WS_AdmMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            if (_context.Members == null)
            {
                return Problem("Entity set 'PaiPaiGoContext.Members'  is null.");
            }
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(string id)
        {
          return (_context.Members?.Any(e => e.MemberId == id)).GetValueOrDefault();
        }
    }
}
