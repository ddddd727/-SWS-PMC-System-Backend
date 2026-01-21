using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Entities;

namespace PMCSystem_Backend.Data
{
    public class MyDbContext : DbContext
    {
        public DbSet<ExampleEntity> Examples { get; set; }      // DbSet 对应表

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExampleEntity>().ToTable(nameof(ExampleEntity));        // 指定表名
            base.OnModelCreating(modelBuilder);
        }
    }
}
