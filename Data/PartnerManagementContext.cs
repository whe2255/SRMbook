using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SrmBook.Models;

namespace SrmBook.Data
{
    public class PartnerManagementContext : DbContext
    {
        public PartnerManagementContext (DbContextOptions<PartnerManagementContext> options)
            : base(options)
        {
        }

        public DbSet<SrmBook.Models.PartnerManagement> PartnerManagement { get; set; } = default!;
    }
}
