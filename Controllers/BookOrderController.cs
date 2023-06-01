using Microsoft.AspNetCore.Mvc;
using SrmBook.Data;
using SrmBook.Models;
using System.Linq;

namespace SrmBook.Controllers
{
    public class BookOrderController : Controller
    {
        private readonly BookOrderContext _context;

        public BookOrderController(BookOrderContext context)
        {
            _context = context;
        }

        // 주문 생성 페이지
        public IActionResult Create()
        {
            // 주문 생성 페이지 뷰 반환
            return View();
        }

        // 주문 생성 처리
        [HttpPost]
        public IActionResult Create(BookOrder order)
        {
            if (ModelState.IsValid)
            {
                // 주문 정보 저장
                _context.BookOrder.Add(order);

                // 재고 수량 차감
                var bookInventory = _context.BookInventory.Find(order.BOOK_NUM);
                if (bookInventory != null)
                {
                    bookInventory.BOOK_QUANTITY -= order.BOOK_QUANTITY;
                    _context.BookInventory.Update(bookInventory);
                }

                // 가격 계산
                order.TOTAL_PRICE = CalculateTotalPrice(bookInventory.BOOK_PRICE, order.BOOK_QUANTITY);

                // 주문 저장
                _context.SaveChanges();

                // 주문 생성 후 성공 메시지 출력
                TempData["SuccessMessage"] = "주문이 성공적으로 생성되었습니다.";

                return RedirectToAction("List");
            }

            // 유효성 검사 실패 시 주문 생성 페이지 뷰 반환
            return View(order);
        }

        // 주문 상세 정보 페이지
        public IActionResult Details(int id)
        {
            // 주문 조회
            BookOrder order = _context.BookOrder.Find(id);

            if (order != null)
            {
                // 주문 상세 정보 뷰 반환
                return View(order);
            }

            // 주문이 존재하지 않을 경우 에러 페이지 뷰 반환
            return View("Error");
        }

        // 주문 목록 페이지
        public IActionResult List()
        {
            // 모든 주문 조회
            var orders = _context.BookOrder.ToList();

            // 주문 목록 뷰 반환
            return View(orders);
        }

        // 주문 삭제 처리
        [HttpPost]
        public IActionResult Delete(int id)
        {
            // 주문 조회
            BookOrder order = _context.BookOrder.Find(id);

            if (order != null)
            {
                // 주문 삭제
                _context.BookOrder.Remove(order);

                // 재고 수량 복구
                var bookInventory = _context.BookInventory.Find(order.BOOK_NUM);
                if (bookInventory != null)
                {
                    bookInventory.BOOK_QUANTITY += order.BOOK_QUANTITY;
                    _context.BookInventory.Update(bookInventory);
                }

                // 주문 저장
                _context.SaveChanges();

                // 주문 삭제 후 성공 메시지 출력
                TempData["SuccessMessage"] = "주문이 성공적으로 삭제되었습니다.";
                return RedirectToAction("List");
            }

            // 주문이 존재하지 않을 경우 에러 페이지 뷰 반환
            return View("Error");
        }

        // 총 가격 계산 메소드
        private decimal CalculateTotalPrice(decimal bookPrice, int bookQuantity)
        {
            return bookPrice * bookQuantity;
        }
    }
}
