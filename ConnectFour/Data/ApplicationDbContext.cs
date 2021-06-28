using System;
using System.Collections.Generic;
using System.Text;
using ConnectFour.Models;
using Microsoft.EntityFrameworkCore;

namespace ConnectFour.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ConnectFour.Models.Player> Player { get; set; }

    }
}
