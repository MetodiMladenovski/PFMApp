using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PFM.Database.Entities;

namespace PFM.Database
{

    public class PfmDbContext : DbContext 
    {
        public PfmDbContext(DbContextOptions<PfmDbContext> options) : base (options)
        {
                
        }
        public  DbSet<TransactionEntity> transactions { get; set; }
        public  DbSet<CategoryEntity> categories { get; set; }
        public DbSet<MccCodes> mcccodes { get; set; }
        public DbSet<SplitTransactions> splittransactions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}