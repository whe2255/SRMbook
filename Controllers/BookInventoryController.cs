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
    public class BookInventoryController : Controller
    {
        private readonly BookInventoryContext _context;

        public BookInventoryController(BookInventoryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string BookGenre)
        {
            if (_context.BookInventory == null)
            {
                return Problem("Entity set 'BookInventory'  is null.");
            }
            //장르별 검색, 검색
            IQueryable<string> genreQuery = from m in _context.BookInventory
                                            orderby m.BOOK_CLASS
                                            select m.BOOK_CLASS;

            var BookInventory = from m in _context.BookInventory
                                select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                BookInventory = BookInventory.Where(s => s.BOOK_NAME!.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(BookGenre))
            {
                BookInventory = BookInventory.Where(x => x.BOOK_CLASS == BookGenre);
            }
            var BookSearchView = new BookSearchView
            {
                Genre = new SelectList(await genreQuery.Distinct().ToListAsync()),
                BookInventory = await BookInventory.ToListAsync()
            };

            return View(BookSearchView);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BookInventory == null)
            {
                return NotFound();
            }

            var bookInventory = await _context.BookInventory
                .FirstOrDefaultAsync(m => m.BOOK_NUM == id);
            if (bookInventory == null)
            {
                return NotFound();
            }

            return View(bookInventory);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BOOK_NUM,BOOK_CLASS,BOOK_NAME,BOOK_WRITER,BOOK_PRICE,BOOK_QUANTITY")] BookInventory bookInventory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookInventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookInventory);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BookInventory == null)
            {
                return NotFound();
            }

            var bookInventory = await _context.BookInventory.FindAsync(id);
            if (bookInventory == null)
            {
                return NotFound();
            }
            return View(bookInventory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BOOK_NUM,BOOK_CLASS,BOOK_NAME,BOOK_WRITER,BOOK_PRICE,BOOK_QUANTITY")] BookInventory bookInventory)
        {
            if (id != bookInventory.BOOK_NUM)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookInventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookInventoryExists(bookInventory.BOOK_NUM))
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
            return View(bookInventory);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BookInventory == null)
            {
                return NotFound();
            }

            var bookInventory = await _context.BookInventory
                .FirstOrDefaultAsync(m => m.BOOK_NUM == id);
            if (bookInventory == null)
            {
                return NotFound();
            }

            return View(bookInventory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BookInventory == null)
            {
                return Problem("Entity set 'BookInventoryContext.BookInventory'  is null.");
            }
            var bookInventory = await _context.BookInventory.FindAsync(id);
            if (bookInventory != null)
            {
                _context.BookInventory.Remove(bookInventory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool BookInventoryExists(int id)
        {
            return _context.BookInventory.Any(e => e.BOOK_NUM == id);
        }
    }
}
