using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SrmBook.Models;

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
        public DbSet<SrmBook.Models.BookUser> BookUser { get; set; } = default!;
    }
}
