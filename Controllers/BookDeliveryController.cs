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

        public async Task<IActionResult> Index(string searchString, DateTime? searchDate)
        {
            //배송 검색
            var bookDeliverys = await SearchBookOrders(searchString, searchDate);
            //발주와 배송
            var bookDelivery = await _context.BookDelivery.ToListAsync();
            var bookOrder = await _context.BookOrder.ToListAsync();

            // 복합ViewModel에 데이터 할당
            var bookDeliveryComposite = new BookDeliveryComposite
            {
                BookOrder = bookOrder,
                BookDelivery = bookDeliverys
            };

            return View(bookDeliveryComposite);
        }

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

        public IActionResult Create(int orderNum)
        {
            //발주 정보를 가져오기
            var bookOrder = _context.BookOrder.FirstOrDefault(o => o.ORDER_NUM == orderNum);

            if (bookOrder != null)
            {
                var bookUser = _context.BookUser.FirstOrDefault(u => u.USER_ID == bookOrder.USER_ID);

                if (bookUser != null)
                {
                    var bookDelivery = new BookDelivery
                    {
                        ORDER_NUM = bookOrder.ORDER_NUM,
                        USER_ID = bookOrder.USER_ID,
                        BOOK_QUANTITY = bookOrder.BOOK_QUANTITY,
                        BOOK_NUM = bookOrder.BOOK_NUM,
                        BOOK_NAME = bookOrder.BOOK_NAME,
                        ADDRESS = bookUser.ADDRESS,
                        TEL = bookUser.TEL
                    };

                    return View(bookDelivery);
                }
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DELIVERY_NUM,ORDER_NUM,USER_ID,DELIVERY_DATE,BOOK_QUANTITY,BOOK_NUM,BOOK_NAME,ADDRESS,TEL")] BookDelivery bookDelivery)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookDelivery);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookDelivery);
        }

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

        //검색 메소드
        private async Task<List<BookDelivery>> SearchBookOrders(string searchString, DateTime? searchDate)
        {
            var bookDeliverys = from m in _context.BookDelivery
                                select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                bookDeliverys = bookDeliverys.Where(s => s.BOOK_NAME.Contains(searchString));
            }

            if (searchDate.HasValue)
            {
                // 검색한 날짜의 시작 시간과 끝 시간 계산
                DateTime startDate = searchDate.Value.Date;
                DateTime endDate = searchDate.Value.AddDays(1).Date;

                bookDeliverys = bookDeliverys.Where(s => s.DELIVERY_DATE >= startDate && s.DELIVERY_DATE < endDate);
            }

            return await bookDeliverys.ToListAsync();
        }
    }
}
