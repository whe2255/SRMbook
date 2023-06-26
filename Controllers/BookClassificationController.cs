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
        public async Task<IActionResult> Index(string searchString, string BookGenre, int page = 1)
        {
            if (_context.BookClassification == null)
            {
                return Problem("Entity set 'BookClassification' is null.");
            }

            // 검색 쿼리
            IQueryable<string> genreQuery = from m in _context.BookClassification
                                            orderby m.BOOK_CLASS
                                            select m.BOOK_CLASS;

            var BookClassification = from m in _context.BookClassification
                                     select m;

            var BookSearchView = await SearchBooks(searchString, BookGenre, genreQuery, BookClassification);

            // 페이징 처리
            int pageSize = 5; // 페이지당 도서 개수

            var pagedBooks = PaginateBooks(BookSearchView.BookClassification, page, pageSize);

            int totalBooks = BookSearchView.BookClassification.Count(); // 전체 도서 개수

            BookSearchView.PagedBooksClassification = pagedBooks;
            BookSearchView.CurrentPage = page;
            BookSearchView.PageSize = pageSize;
            BookSearchView.TotalBooks = totalBooks;

            return View(BookSearchView);
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
        public async Task<IActionResult> Create([Bind("BOOK_NUM,BOOK_CLASS,BOOK_NAME,BOOK_WRITER,BOOK_PRICE,BOOK_IMAGE")] BookClassification bookClassification, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                await ProcessImageFile(bookClassification, imageFile);

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
        public async Task<IActionResult> Edit(int id, [Bind("BOOK_NUM,BOOK_CLASS,BOOK_NAME,BOOK_WRITER,BOOK_PRICE")] BookClassification bookClassification, IFormFile imageFile)
        {
            if (id != bookClassification.BOOK_NUM)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ProcessImageFile(bookClassification, imageFile);
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
                return Problem("Entity set 'BookClassificationContext.BookClassification' is null.");
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

        //image파일 바이너리로 변환 메소드
        private async Task ProcessImageFile(BookClassification bookClassification, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(memoryStream);
                    bookClassification.BOOK_IMAGE = memoryStream.ToArray();
                }
            }
        }

        //검색 메소드 
        private async Task<BookSearchView> SearchBooks(string searchString, string BookGenre, IQueryable<string> genreQuery, IQueryable<BookClassification> BookClassification)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                BookClassification = BookClassification.Where(s => s.BOOK_NAME!.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(BookGenre))
            {
                BookClassification = BookClassification.Where(x => x.BOOK_CLASS == BookGenre);
            }

            var BookSearchView = new BookSearchView
            {
                Genre = new SelectList(await genreQuery.Distinct().ToListAsync()),
                BookClassification = await BookClassification.ToListAsync()
            };

            return BookSearchView;
        }

        //페이징 처리
        private List<BookClassification> PaginateBooks(List<BookClassification> books, int page, int pageSize)
        {
            return books
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }
}
