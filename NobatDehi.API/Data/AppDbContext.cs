using Microsoft.EntityFrameworkCore;
using NobatDehi.API.Models;

namespace NobatDehi.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Holiday> Holidays { get; set; }
    public DbSet<Office> Offices { get; set; }
    public DbSet<OfficeHolidayException> OfficeHolidayExceptions { get; set; }
    public DbSet<OfficeSettings> OfficeSettings { get; set; }
    public DbSet<Plan> Plans { get; set; }
    public DbSet<Province> Provinces { get; set; }
    public DbSet<PlanDependency> PlanDependencies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlanDependency>()
        .HasOne(p => p.Plan)
        .WithMany(p => p.Dependencies)
        .HasForeignKey(p => p.PlanId)
        .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<PlanDependency>()
        .HasOne(p => p.RequiredPlan)
        .WithMany()
        .HasForeignKey(p => p.RequiredPlanId)
        .OnDelete(DeleteBehavior.NoAction);
    }
}