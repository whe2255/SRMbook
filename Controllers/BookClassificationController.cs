using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SrmBook.Data;
using SrmBook.Models;

namespace SrmBook.Controllers
{
    public class BookClassificationController : Controller
    {
        private readonly BookClassificationContext _context;

        public BookClassificationController(BookClassificationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string BookGenre)
        {
            if (_context.BookClassification == null)
            {
                return Problem("Entity set 'BookClassification'  is null.");
            }
            //장르별 검색, 검색
            IQueryable<string> genreQuery = from m in _context.BookClassification
                                            orderby m.BOOK_CLASS
                                            select m.BOOK_CLASS;

            var BookClassification = from m in _context.BookClassification
                                     select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                BookClassification = BookClassification.Where(s => s.BOOK_NAME!.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(BookGenre))
            {
                BookClassification = BookClassification.Where(x => x.BOOK_CLASS == BookGenre);
            }

            var bookClassificationView = new BookClassificationView
            {
                Genre = new SelectList(await genreQuery.Distinct().ToListAsync()),
                BookClassification = await BookClassification.ToListAsync()
            };

            return View(bookClassificationView);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BookClassification == null)
            {
                return NotFound();
            }

            var bookClassification = await _context.BookClassification
                .FirstOrDefaultAsync(m => m.BOOK_NUM == id);
            if (bookClassification == null)
            {
                return NotFound();
            }

            return View(bookClassification);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BOOK_NUM,BOOK_CLASS,BOOK_NAME,BOOK_WRITER,PUBLISHER,BOOK_PRICE")] BookClassification bookClassification)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookClassification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookClassification);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BookClassification == null)
            {
                return NotFound();
            }

            var bookClassification = await _context.BookClassification.FindAsync(id);
            if (bookClassification == null)
            {
                return NotFound();
            }
            return View(bookClassification);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BOOK_NUM,BOOK_CLASS,BOOK_NAME,BOOK_WRITER,PUBLISHER,BOOK_PRICE")] BookClassification bookClassification)
        {
            if (id != bookClassification.BOOK_NUM)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookClassification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookClassificationExists(bookClassification.BOOK_NUM))
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
            return View(bookClassification);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BookClassification == null)
            {
                return NotFound();
            }

            var bookClassification = await _context.BookClassification
                .FirstOrDefaultAsync(m => m.BOOK_NUM == id);
            if (bookClassification == null)
            {
                return NotFound();
            }

            return View(bookClassification);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BookClassification == null)
            {
                return Problem("Entity set 'BookClassificationContext.BookClassification'  is null.");
            }
            var bookClassification = await _context.BookClassification.FindAsync(id);
            if (bookClassification != null)
            {
                _context.BookClassification.Remove(bookClassification);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookClassificationExists(int id)
        {
            return _context.BookClassification.Any(e => e.BOOK_NUM == id);
        }
    }
}
