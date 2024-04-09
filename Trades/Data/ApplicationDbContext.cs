using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Trades.Model;


namespace Trades.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Inputs> Inputs { get; set; }
        public DbSet<OutPuts> OutPuts { get; set; }
        public DbSet<OutPuts> Outputs { get; set; }

    }
}
