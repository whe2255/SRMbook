using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SrmBook.Models;

namespace SrmBook.Data
{
    public class BookDeliveryContext : DbContext
    {
        public BookDeliveryContext (DbContextOptions<BookDeliveryContext> options)
            : base(options)
        {
        }

        public DbSet<SrmBook.Models.BookDelivery> BookDelivery { get; set; } = default!;
    }
}
