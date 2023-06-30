using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<IActionResult> Index(string searchString, string searchStr, DateTime? searchDate, int? orderPage, int? inventoryPage)
        {
            int itemsPerPage = 5; // 페이지당 항목 수
            int orderCurrentPage = orderPage ?? 1; // 발주 페이지 (기본값: 1)
            int inventoryCurrentPage = inventoryPage ?? 1; // 재고 페이지 (기본값: 1)

            // 발주 검색
            var bookOrders = await SearchBookOrders(searchString, searchDate);

            // 페이징 처리 - bookOrders
            var totalOrderItems = bookOrders.Count();
            var pagedBookOrders = bookOrders.Skip((orderCurrentPage - 1) * itemsPerPage).Take(itemsPerPage);

            // 재고 정보 가져오기
            var bookInventory = await _context.BookInventory.ToListAsync();
            var searchInventory = await SearchInventory(searchStr);
            bookInventory = searchInventory;
            var totalInventoryItems = bookInventory.Count();
            var pagedBookInventory = bookInventory.Skip((inventoryCurrentPage - 1) * itemsPerPage).Take(itemsPerPage);

            // 복합 ViewModel에 데이터 할당
            var bookComposite = new BookComposite
            {
                BookOrders = pagedBookOrders,
                BookInventory = pagedBookInventory,
                OrderPageInfo = new PageInfo
                {
                    TotalItems = totalOrderItems,
                    CurrentPage = orderCurrentPage,
                    ItemsPerPage = itemsPerPage
                },
                InventoryPageInfo = new PageInfo
                {
                    TotalItems = totalInventoryItems,
                    CurrentPage = inventoryCurrentPage,
                    ItemsPerPage = itemsPerPage
                }
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
                // 발주 시 도서 재고에 있는 도서 제목과 도서 번호가 일치하는지 확인
                var bookInventory = await _context.BookInventory.FirstOrDefaultAsync(b => b.BOOK_NAME == bookOrder.BOOK_NAME && b.BOOK_NUM == bookOrder.BOOK_NUM);

                if (bookInventory == null)
                {
                    ModelState.AddModelError(string.Empty, "도서 제목이나 도서 번호를 확인해주세요");
                    return View(bookOrder);
                }
                else
                {
                    // 재고 수량 감소
                    bool isStockAvailable = await DecreaseBookInventory(bookOrder.BOOK_NUM, bookOrder.BOOK_QUANTITY);

                    if (!isStockAvailable)
                    {
                        ModelState.AddModelError(string.Empty, "재고가 부족하여 구매할 수 없습니다");
                        return View(bookOrder);
                    }
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

        //발주 확인 메소드
        public async Task<IActionResult> ConfirmOrder(int orderId)
        {
            var order = await _context.BookOrder.FindAsync(orderId);

            if (order == null)
            {
                // 발주가 존재하지 않을 경우 에러 처리
                return NotFound();
            }

            order.ORDER_CONFIRMED = true;

            _context.Entry(order).Property(x => x.ORDER_CONFIRMED).IsModified = true;

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
        private async Task<bool> DecreaseBookInventory(int bookNum, int quantity)
        {
            var bookInventory = await _context.BookInventory.FindAsync(bookNum);
            if (bookInventory != null)
            {
                if (bookInventory.BOOK_QUANTITY >= quantity)
                {
                    bookInventory.BOOK_QUANTITY -= quantity;
                    _context.BookInventory.Update(bookInventory);
                    await _context.SaveChangesAsync();
                    return true; // 재고 충분하여 구매 가능
                }
            }
            return false; // 재고 부족으로 구매 불가능
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

        //발주 검색 메소드
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

        //재고 검색 메소드
        private async Task<List<BookInventory>> SearchInventory(string searchStr)
        {
            var BookInventory = from m in _context.BookInventory
                                select m;

            if (!String.IsNullOrEmpty(searchStr))
            {
                BookInventory = BookInventory.Where(s => s.BOOK_NAME.Contains(searchStr));
            }
            return await BookInventory.ToListAsync();
        }
    }
}
