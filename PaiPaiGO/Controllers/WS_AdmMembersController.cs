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
    public class WS_AdmMembersController : Controller
    {
        private readonly PaiPaiGoContext _context;

        public WS_AdmMembersController(PaiPaiGoContext context)
        {
            _context = context;
        }

        //我想在這抓到Opinion的東西

        //public class MyViewModel
        //{
        //    public IEnumerable<Member> Members { get; set; }
        //    public Opinion Opinion { get; set; }
        //}

        //public ActionResult MyAction()
        //{
        //    var viewModel = new MyViewModel
        //    {
        //        Members = _context.Members.ToList(),
        //        Opinion = _context.Opinions.FirstOrDefault()
        //    };
        //    return View(viewModel);
        //}


        // GET: WS_AdmMembers
        public async Task<IActionResult> AdmMember()
        {
            var paiPaiGoContext = _context.Members.Include(m => m.MemberPostcodeNavigation);
            return View(await paiPaiGoContext.ToListAsync());
        }

        // GET: WS_AdmMembers/Details/5
        public async Task<IActionResult> Details(string id)
        {
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
        {
            ViewData["MemberPostcode"] = new SelectList(_context.Counties, "Postcode", "Postcode");
            return View();
        }

        // POST: WS_AdmMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,MemberName,MemberPhoneNumber,MemberPostcode,MemberCity,MemberTownship,MemberAddress,MemberEmail,MemberStatus,MemberPassword,Gearing")] Member member)
        {
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
        {
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
        {
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
        {
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
        {
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
