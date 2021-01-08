using EfProblemSample.WebApi.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace EfProblemSample.WebApi.Repository
{
    public class Context : DbContext
    {
        public DbSet<Entity2> Entity2s { get; set; }

        public DbSet<Entity1> Entity1s { get; set; }

        public Context(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entity1>().HasIndex(x => x.AUniqueProperty).IsUnique();
        }
    }
}
