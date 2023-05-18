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
        public DbSet<SrmBook.Models.BookUser> BookUser { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-NV33LNS\\SQLEXPRESS;Database=SRMBOOK;Trusted_Connection=True; encrypt=false");
        }
    }
}
