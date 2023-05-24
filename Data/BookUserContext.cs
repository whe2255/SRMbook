using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SrmBook.Models;

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
