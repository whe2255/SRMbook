using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SrmBook.Data;
using SrmBook.Models;

namespace SrmBook.Controllers;

public class HomeController : Controller
{
    private readonly BookOrderContext _context;

    public HomeController(BookOrderContext context)
    {
        _context = context;
    }
    //구매 순위
    public async Task<IActionResult> Index()
    {
        var purchaseRanking = await _context.BookOrder
            .GroupBy(p => p.BOOK_NAME)
            .Select(g => new PurchaseRankingViewModel
            {
                BOOK_NAME = g.Key,
                TOTAL_QUANTITY = g.Sum(p => p.BOOK_QUANTITY)
            })
            .OrderByDescending(p => p.TOTAL_QUANTITY)
            .ToListAsync();

        return View(purchaseRanking);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
