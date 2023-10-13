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

        public YH_CasePagesController(PaiPaiGoContext context)
        {
            _context = context;
        }
        public IActionResult SelectChange()
        {
            ViewData["County"] = new SelectList(_context.Counties, "CategoryId", "CategoryId");
            return View();
        }

        // GET: YH_CasePages
        public async Task<IActionResult> YH_CasePage(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (_context.Counties == null)
            {
                return Problem("Entity set 'Counties'  is null.");
            }
            var counties = from c in _context.Counties
                           select c;
            if (!String.IsNullOrEmpty(currentFilter))
            {
                counties = counties.Where(s => s.City!.Contains(currentFilter));
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

            //return View(await missions.ToListAsync());
            var paiPaiGoContext = _context.Missions.Include(m => m.CategoryNavigation);

            //分頁,一頁8個card
            var pageNumber = page ?? 1;
            return View(await missions.ToPagedListAsync(pageNumber, 8));
        }
     

        // GET: YH_CasePages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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
