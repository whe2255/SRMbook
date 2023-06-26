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

        public async Task<IActionResult> Index(string searchString, string BookGenre, int page = 1)
        {
            if (_context.BookInventory == null)
            {
                return Problem("Entity set 'BookInventory' is null.");
            }
            //검색 쿼리
            IQueryable<string> genreQuery = from m in _context.BookInventory
                                            orderby m.BOOK_CLASS
                                            select m.BOOK_CLASS;

            var BookInventory = from m in _context.BookInventory
                                select m;

            var BookSearchView = await SearchBooks(searchString, BookGenre, genreQuery, BookInventory);
            // 페이징 처리
            int pageSize = 5; // 페이지당 도서 개수

            var pagedBooks2 = PaginateBooks(BookSearchView.BookInventory, page, pageSize);

            int totalBooks = BookSearchView.BookInventory.Count(); // 전체 도서 개수

            BookSearchView.PagedBooks2 = pagedBooks2;
            BookSearchView.CurrentPage = page;
            BookSearchView.PageSize = pageSize;
            BookSearchView.TotalBooks = totalBooks;


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

        //검색 메소드
        private async Task<BookSearchView> SearchBooks(string searchString, string BookGenre, IQueryable<string> genreQuery, IQueryable<BookInventory> BookInventory)
        {
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

            return BookSearchView;
        }

        //페이징 처리
        private List<BookInventory> PaginateBooks(List<BookInventory> books, int page, int pageSize)
        {
            return books
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }
}
