using FinalProjectInventory.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectInventory.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
        public DbSet<AdminUsers> Users { get; set; }
        public DbSet<AddRecordAClass> AddRecordAClass { get; set; }
    }
}
