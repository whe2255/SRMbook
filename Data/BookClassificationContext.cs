using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SrmBook.Models;

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
