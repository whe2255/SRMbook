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

        public async Task<IActionResult> Index(string searchString, DateTime? searchDate, int? orderPage, int? deliveryPage)
        {

            int itemsPerPage = 5; // 페이지당 항목 수
            int orderCurrentPage = orderPage ?? 1; // 발주 페이지 (기본값: 1)
            int deliveryCurrentPage = deliveryPage ?? 1; // 배송 페이지 (기본값: 1)

            // 로그인한 사용자의 정보를 세션에서 가져옴
            var currentUser = HttpContext.Session.GetString("USER_SESSION_KEY");
            var userType = HttpContext.Session.GetString("USER_TYPE_KEY");

            //배송 검색
            var bookDeliverys = await SearchBookDeliverys(searchString, userType, searchDate, currentUser);
            //발주 검색
            var bookOrders = await SearchBookOrders(searchString, userType, searchDate, currentUser);

            // 페이징 처리 - bookDelivery
            var totalDeliveryItems = bookDeliverys.Count();
            var pagedBookDeliverys = bookDeliverys.Skip((deliveryCurrentPage - 1) * itemsPerPage).Take(itemsPerPage);

            // 페이징 처리 - bookOrder
            var totalOrderItems = bookOrders.Count();
            var pagedBookOrders = bookOrders.Skip((orderCurrentPage - 1) * itemsPerPage).Take(itemsPerPage);

            // 복합ViewModel에 데이터 할당
            var bookDeliveryComposite = new BookDeliveryComposite
            {
                BookOrder = pagedBookOrders,
                BookDelivery = pagedBookDeliverys,
                OrderPageInfo = new BookDeliveryComposite.PageInfo
                {
                    TotalItems = totalOrderItems,
                    CurrentPage = orderCurrentPage,
                    ItemsPerPage = itemsPerPage
                },
                DeliveryPageInfo = new BookDeliveryComposite.PageInfo
                {
                    TotalItems = totalDeliveryItems,
                    CurrentPage = deliveryCurrentPage,
                    ItemsPerPage = itemsPerPage
                },
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
        private async Task<List<BookDelivery>> SearchBookDeliverys(string searchString, string userType, DateTime? searchDate, string currentUser)
        {
            var bookDeliverys = _context.BookDelivery.AsQueryable();
            if (IsAdminUser(userType))
            {
                // 관리자인 경우 모든 발주를 반환
                bookDeliverys = bookDeliverys.Where(s => true);
            }
            else
            {
                // 일반 사용자인 경우 해당 사용자의 발주만 필터링
                bookDeliverys = bookDeliverys.Where(s => s.USER_ID == currentUser);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                bookDeliverys = bookDeliverys.Where(s => s.BOOK_NAME.Contains(searchString));
            }

            if (searchDate.HasValue)
            {
                DateTime startDate = searchDate.Value.Date;
                DateTime endDate = searchDate.Value.AddDays(1).Date;
                bookDeliverys = bookDeliverys.Where(s => s.DELIVERY_DATE >= startDate && s.DELIVERY_DATE < endDate);
            }

            return await bookDeliverys.ToListAsync();
        }

        //발주 검색 메소드
        private async Task<List<BookOrder>> SearchBookOrders(string searchString, string userType, DateTime? searchDate, string currentUser)
        {
            var bookOrders = _context.BookOrder.AsQueryable();

            if (IsAdminUser(userType))
            {
                // 관리자인 경우 모든 발주를 반환
                bookOrders = bookOrders.Where(s => true);
            }
            else
            {
                // 일반 사용자인 경우 해당 사용자의 발주만 필터링
                bookOrders = bookOrders.Where(s => s.USER_ID == currentUser);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                bookOrders = bookOrders.Where(s => s.BOOK_NAME.Contains(searchString));
            }

            if (searchDate.HasValue)
            {
                DateTime startDate = searchDate.Value.Date;
                DateTime endDate = searchDate.Value.AddDays(1).Date;
                bookOrders = bookOrders.Where(s => s.ORDER_DATE >= startDate && s.ORDER_DATE < endDate);
            }

            return await bookOrders.ToListAsync();
        }

        private bool IsAdminUser(string userType)
        {
            // 세션을 통해 현재 사용자가 관리자인지 여부를 판별
            // 예시: 세션에서 userType 값이 "admin"인 경우에만 관리자로 판별
            return userType == "admin";
        }
    }
}
