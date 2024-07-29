using DataManagement.Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace DataManagement.Infrastructure;

public class DataManagementDbContext(DbContextOptions<DataManagementDbContext> options) : DbContext(options)
{
    public DbSet<Advisor> Advisors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("TestDB");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AdvisorConfiguration());
    }
}