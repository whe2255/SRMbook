using Microsoft.EntityFrameworkCore;

namespace SrmBook.Data
{
    public class BookOrderContext : DbContext
    {
        public BookOrderContext (DbContextOptions<BookOrderContext> options)
            : base(options)
        {
        }

        public DbSet<SrmBook.Models.BookOrder> BookOrder { get; set; } = default!;
        public DbSet<SrmBook.Models.BookInventory> BookInventory { get; set; } = default!;
    }
}
