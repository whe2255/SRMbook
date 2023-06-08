using Microsoft.EntityFrameworkCore;

namespace SrmBook.Data
{
    public class BookClassificationContext : DbContext
    {
        public BookClassificationContext (DbContextOptions<BookClassificationContext> options)
            : base(options)
        {
        }

        public DbSet<SrmBook.Models.BookClassification> BookClassification { get; set; } = default!;
    }
}
