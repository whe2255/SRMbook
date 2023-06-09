using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SrmBook.Data;
using SrmBook.Models;

namespace SrmBook.Controllers
{
    public class BookOrderController : Controller
    {
        private readonly BookOrderContext _context;

        public BookOrderController(BookOrderContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, DateTime? searchDate)
        {
            // 발주 검색
            var bookOrders = await SearchBookOrders(searchString, searchDate);

            // 재고 정보 가져오기
            var bookInventory = await _context.BookInventory.ToListAsync();

            // 복합ViewModel에 데이터 할당
            var bookComposite = new BookComposite
            {
                BookOrders = bookOrders,
                BookInventory = bookInventory
            };

            return View(bookComposite);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BookOrder == null)
            {
                return NotFound();
            }

            var bookOrder = await _context.BookOrder
                .FirstOrDefaultAsync(m => m.ORDER_NUM == id);
            if (bookOrder == null)
            {
                return NotFound();
            }

            return View(bookOrder);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ORDER_NUM,USER_ID,ORDER_DATE,BOOK_QUANTITY,TOTAL_PRICE,BOOK_NAME,BOOK_NUM")] BookOrder bookOrder)
        {
            if (ModelState.IsValid)
            {
                //발주 시 도서 재고에 있는 도서 제목과 도서 번호가 일치하는지 확인
                var bookInventory = await _context.BookInventory.FirstOrDefaultAsync(b => b.BOOK_NAME == bookOrder.BOOK_NAME);
                var bookNum = await _context.BookInventory.FirstOrDefaultAsync(c => c.BOOK_NUM == bookOrder.BOOK_NUM);
                
                if (bookInventory == null || bookNum == null)
                {
                    ModelState.AddModelError(string.Empty, "도서 제목이나 도서 번호를 확인해주세요");
                    return View(bookOrder);
                }
                else
                {
                    // 재고 수량 감소
                    await DecreaseBookInventory(bookOrder.BOOK_NUM, bookOrder.BOOK_QUANTITY);
                    // 가격 계산
                    bookOrder.TOTAL_PRICE = await CalculateTotalPrice(bookOrder.BOOK_NUM, bookOrder.BOOK_QUANTITY);
                    _context.Add(bookOrder);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(bookOrder);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BookOrder == null)
            {
                return NotFound();
            }

            var bookOrder = await _context.BookOrder
                .FirstOrDefaultAsync(m => m.ORDER_NUM == id);
            if (bookOrder == null)
            {
                return NotFound();
            }

            return View(bookOrder);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BookOrder == null)
            {
                return Problem("Entity set 'BookOrderContext.BookOrder'  is null.");
            }

            var bookOrder = await _context.BookOrder.FindAsync(id);
            if (bookOrder != null)
            {
                _context.BookOrder.Remove(bookOrder);
            }
            //재고 수량 증가
            await IncreaseBookInventory(bookOrder.BOOK_NUM, bookOrder.BOOK_QUANTITY);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookOrderExists(int id)
        {
            return _context.BookOrder.Any(e => e.ORDER_NUM == id);
        }

        // 총 가격 계산 메소드
        private async Task<decimal> CalculateTotalPrice(int bookNum, int quantity)
        {
            var bookInventory = await _context.BookInventory.FindAsync(bookNum);
            if (bookInventory != null)
            {
                return bookInventory.BOOK_PRICE * quantity;
            }
            return 0;
        }

        //재고 수량 감소
        private async Task DecreaseBookInventory(int bookNum, int quantity)
        {
            var bookInventory = await _context.BookInventory.FindAsync(bookNum);
            if (bookInventory != null)
            {
                bookInventory.BOOK_QUANTITY -= quantity;
                _context.BookInventory.Update(bookInventory);
                await _context.SaveChangesAsync();
            }
        }

        //재고 수량 증가
        private async Task IncreaseBookInventory(int bookNum, int quantity)
        {
            var bookInventory = await _context.BookInventory.FindAsync(bookNum);
            if (bookInventory != null)
            {
                bookInventory.BOOK_QUANTITY += quantity;
                _context.BookInventory.Update(bookInventory);
                await _context.SaveChangesAsync();
            }
        }

        //검색 메소드
        private async Task<List<BookOrder>> SearchBookOrders(string searchString, DateTime? searchDate)
        {
            var bookOrders = from m in _context.BookOrder
                             select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                bookOrders = bookOrders.Where(s => s.BOOK_NAME.Contains(searchString));
            }

            if (searchDate.HasValue)
            {
                // 검색한 날짜의 시작 시간과 끝 시간 계산
                DateTime startDate = searchDate.Value.Date;
                DateTime endDate = searchDate.Value.AddDays(1).Date;

                bookOrders = bookOrders.Where(s => s.ORDER_DATE >= startDate && s.ORDER_DATE < endDate);
            }

            return await bookOrders.ToListAsync();
        }
    }
}
