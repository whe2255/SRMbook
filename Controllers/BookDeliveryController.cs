using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SrmBook.Data;
using SrmBook.Models;

namespace SrmBook.Controllers
{
    public class BookDeliveryController : Controller
    {
        private readonly BookDeliveryContext _context;

        public BookDeliveryController(BookDeliveryContext context)
        {
            _context = context;
        }

        // GET: BookDelivery
        public async Task<IActionResult> Index()
        {
              return View(await _context.BookDelivery.ToListAsync());
        }

        // GET: BookDelivery/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BookDelivery == null)
            {
                return NotFound();
            }

            var bookDelivery = await _context.BookDelivery
                .FirstOrDefaultAsync(m => m.DELIVERY_NUM == id);
            if (bookDelivery == null)
            {
                return NotFound();
            }

            return View(bookDelivery);
        }

        // GET: BookDelivery/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BookDelivery/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DELIVERY_NUM,ORDER_NUM,USER_ID,DELIVERY_DATE,BOOK_QUANTITY,BOOK_NUM,BOOK_NAME,ADDRESS,TEL,STATUS")] BookDelivery bookDelivery)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookDelivery);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookDelivery);
        }

        // GET: BookDelivery/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BookDelivery == null)
            {
                return NotFound();
            }

            var bookDelivery = await _context.BookDelivery.FindAsync(id);
            if (bookDelivery == null)
            {
                return NotFound();
            }
            return View(bookDelivery);
        }

        // POST: BookDelivery/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DELIVERY_NUM,ORDER_NUM,USER_ID,DELIVERY_DATE,BOOK_QUANTITY,BOOK_NUM,BOOK_NAME,ADDRESS,TEL,STATUS")] BookDelivery bookDelivery)
        {
            if (id != bookDelivery.DELIVERY_NUM)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookDelivery);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookDeliveryExists(bookDelivery.DELIVERY_NUM))
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
            return View(bookDelivery);
        }

        // GET: BookDelivery/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BookDelivery == null)
            {
                return NotFound();
            }

            var bookDelivery = await _context.BookDelivery
                .FirstOrDefaultAsync(m => m.DELIVERY_NUM == id);
            if (bookDelivery == null)
            {
                return NotFound();
            }

            return View(bookDelivery);
        }

        // POST: BookDelivery/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BookDelivery == null)
            {
                return Problem("Entity set 'BookDeliveryContext.BookDelivery'  is null.");
            }
            var bookDelivery = await _context.BookDelivery.FindAsync(id);
            if (bookDelivery != null)
            {
                _context.BookDelivery.Remove(bookDelivery);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookDeliveryExists(int id)
        {
          return _context.BookDelivery.Any(e => e.DELIVERY_NUM == id);
        }
    }
}
