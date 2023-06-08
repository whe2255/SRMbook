using Microsoft.EntityFrameworkCore;

namespace SrmBook.Data
{
    public class BookInventoryContext : DbContext
    {
        public BookInventoryContext (DbContextOptions<BookInventoryContext> options)
            : base(options)
        {
        }

        public DbSet<SrmBook.Models.BookInventory> BookInventory { get; set; } = default!;
    }
}
