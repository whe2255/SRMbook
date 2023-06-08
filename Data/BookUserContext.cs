using Microsoft.EntityFrameworkCore;

namespace SrmBook.Data
{
    public class BookUserContext : DbContext
    {
        public BookUserContext(DbContextOptions<BookUserContext> options) : base(options)
        {
        }
        
        public DbSet<SrmBook.Models.BookUser> BookUser { get; set; } = default!;
        
    }

}
