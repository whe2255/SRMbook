using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SrmBook.Data;
using SrmBook.Models;

namespace SrmBook.Controllers
{
    public class PartnerManagementController : Controller
    {
        private readonly PartnerManagementContext _context;

        public PartnerManagementController(PartnerManagementContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.PartnerManagement == null)
            {
                return Problem("Entity set 'PartnerManagement' is null.");
            }
            //검색
            var PartnerManagement = from m in _context.PartnerManagement
                                    select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                PartnerManagement = PartnerManagement.Where(s => s.PUBLISHER!.Contains(searchString));
            }

            return View(await PartnerManagement.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PartnerManagement == null)
            {
                return NotFound();
            }

            var partnerManagement = await _context.PartnerManagement
                .FirstOrDefaultAsync(m => m.PM_NUM == id);
            if (partnerManagement == null)
            {
                return NotFound();
            }

            return View(partnerManagement);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PM_NUM,PM_CONTACT,PUBLISHER,PM_ADD")] PartnerManagement partnerManagement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(partnerManagement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(partnerManagement);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PartnerManagement == null)
            {
                return NotFound();
            }

            var partnerManagement = await _context.PartnerManagement.FindAsync(id);
            if (partnerManagement == null)
            {
                return NotFound();
            }
            return View(partnerManagement);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PM_NUM,PM_CONTACT,PUBLISHER,PM_ADD")] PartnerManagement partnerManagement)
        {
            if (id != partnerManagement.PM_NUM)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(partnerManagement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartnerManagementExists(partnerManagement.PM_NUM))
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
            return View(partnerManagement);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PartnerManagement == null)
            {
                return NotFound();
            }

            var partnerManagement = await _context.PartnerManagement
                .FirstOrDefaultAsync(m => m.PM_NUM == id);
            if (partnerManagement == null)
            {
                return NotFound();
            }

            return View(partnerManagement);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PartnerManagement == null)
            {
                return Problem("Entity set 'PartnerManagementContext.PartnerManagement'  is null.");
            }
            var partnerManagement = await _context.PartnerManagement.FindAsync(id);
            if (partnerManagement != null)
            {
                _context.PartnerManagement.Remove(partnerManagement);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartnerManagementExists(int id)
        {
            return _context.PartnerManagement.Any(e => e.PM_NUM == id);
        }
    }
}
