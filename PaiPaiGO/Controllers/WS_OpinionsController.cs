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

        // GET: WS_Opinions
        public async Task<IActionResult> AdmOpinion()
        {
            var paiPaiGoContext = _context.Opinions.Include(o => o.Mission).Include(o => o.ReportMember).Include(o => o.ReportedMember);
            return View(await paiPaiGoContext.ToListAsync());
        }

        // GET: WS_Opinions/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Opinions == null)
            {
                return NotFound();
            }

            var opinion = await _context.Opinions
                .Include(o => o.Mission)
                .Include(o => o.ReportMember)
                .Include(o => o.ReportedMember)
                .FirstOrDefaultAsync(m => m.Type == id);
            if (opinion == null)
            {
                return NotFound();
            }

            return View(opinion);
        }

        // GET: WS_Opinions/Create
        public IActionResult Create()
        {
            ViewData["MissionId"] = new SelectList(_context.Missions, "MissionId", "MissionId");
            ViewData["ReportMemberId"] = new SelectList(_context.Members, "MemberId", "MemberId");
            ViewData["ReportedMemberId"] = new SelectList(_context.Members, "MemberId", "MemberId");
            return View();
        }

        // POST: WS_Opinions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Type,Date,MissionId,ReportMemberId,ReportedMemberId,Content,State,Score")] Opinion opinion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(opinion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MissionId"] = new SelectList(_context.Missions, "MissionId", "MissionId", opinion.MissionId);
            ViewData["ReportMemberId"] = new SelectList(_context.Members, "MemberId", "MemberId", opinion.ReportMemberId);
            ViewData["ReportedMemberId"] = new SelectList(_context.Members, "MemberId", "MemberId", opinion.ReportedMemberId);
            return View(opinion);
        }

        // GET: WS_Opinions/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
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

        // POST: WS_Opinions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Type,Date,MissionId,ReportMemberId,ReportedMemberId,Content,State,Score")] Opinion opinion)
        {
            if (id != opinion.Type)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(opinion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OpinionExists(opinion.Type))
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
            ViewData["MissionId"] = new SelectList(_context.Missions, "MissionId", "MissionId", opinion.MissionId);
            ViewData["ReportMemberId"] = new SelectList(_context.Members, "MemberId", "MemberId", opinion.ReportMemberId);
            ViewData["ReportedMemberId"] = new SelectList(_context.Members, "MemberId", "MemberId", opinion.ReportedMemberId);
            return View(opinion);
        }

        // GET: WS_Opinions/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Opinions == null)
            {
                return NotFound();
            }

            var opinion = await _context.Opinions
                .Include(o => o.Mission)
                .Include(o => o.ReportMember)
                .Include(o => o.ReportedMember)
                .FirstOrDefaultAsync(m => m.Type == id);
            if (opinion == null)
            {
                return NotFound();
            }

            return View(opinion);
        }

        // POST: WS_Opinions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Opinions == null)
            {
                return Problem("Entity set 'PaiPaiGoContext.Opinions'  is null.");
            }
            var opinion = await _context.Opinions.FindAsync(id);
            if (opinion != null)
            {
                _context.Opinions.Remove(opinion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OpinionExists(string id)
        {
          return (_context.Opinions?.Any(e => e.Type == id)).GetValueOrDefault();
        }
    }
}
