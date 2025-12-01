using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using VisitSystem.Models;

namespace VisitSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<VisitRecord> VisitRecords { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
