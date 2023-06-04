using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

        public async Task<IActionResult> Index()
        {
            return View(await _context.BookOrder.ToListAsync());
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
                _context.Add(bookOrder);

                // 재고 수량 차감
                var bookInventory = _context.BookInventory.Find(bookOrder.BOOK_NUM);
                if (bookInventory != null)
                {
                    bookInventory.BOOK_QUANTITY -= bookOrder.BOOK_QUANTITY;
                    _context.BookInventory.Update(bookInventory);
                }

                // 가격 계산
                bookOrder.TOTAL_PRICE = CalculateTotalPrice(bookInventory.BOOK_PRICE, bookOrder.BOOK_QUANTITY);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookOrder);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BookOrder == null)
            {
                return NotFound();
            }

            var bookOrder = await _context.BookOrder.FindAsync(id);
            if (bookOrder == null)
            {
                return NotFound();
            }
            return View(bookOrder);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ORDER_NUM,USER_ID,ORDER_DATE,BOOK_QUANTITY,TOTAL_PRICE,BOOK_NAME,BOOK_NUM")] BookOrder bookOrder)
        {
            if (id != bookOrder.ORDER_NUM)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookOrderExists(bookOrder.ORDER_NUM))
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

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookOrderExists(int id)
        {
            return _context.BookOrder.Any(e => e.ORDER_NUM == id);
        }

        // 총 가격 계산 메소드
        private decimal CalculateTotalPrice(decimal bookPrice, int bookQuantity)
        {
            return bookPrice * bookQuantity;
        }
    }
}
