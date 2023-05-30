using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SrmBook.Models;

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
