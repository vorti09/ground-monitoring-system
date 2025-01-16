using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Report>()
            .HasMany(r => r.Indicators)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Report>()
            .HasOne(r => r.Employee)
            .WithMany()
            .HasForeignKey(r => r.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Indicator>()
            .Property(i => i.Name)
            .IsRequired();

        modelBuilder.Entity<Employee>()
            .Property(e => e.Name)
            .IsRequired();
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Indicator> Indicators { get; set; }
}