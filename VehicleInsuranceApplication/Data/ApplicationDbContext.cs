using Microsoft.EntityFrameworkCore;
using VehicleInsuranceApplication.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<WebUser> WebUsers { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Policy> Policies { get; set; }
    public DbSet<Claim> Claims { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleCategory> VehicleCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Payment Config
        modelBuilder.Entity<Payment>()
            .Property(p => p.AmountPaid)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Payment>()
            .HasOne(pm => pm.Policy)
            .WithMany(p => p.Payments)
            .HasForeignKey(pm => pm.PolicyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Claim Config
        modelBuilder.Entity<Claim>()
            .Property(c => c.AmountPaid)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Claim>()
            .HasOne(c => c.Policy)
            .WithMany(p => p.Claims)
            .HasForeignKey(c => c.PolicyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Claim>()
            .HasOne(c => c.WebUser)
            .WithMany() // or use .WithMany(w => w.Claims) if you have it in WebUser
            .HasForeignKey(c => c.Uid)
            .OnDelete(DeleteBehavior.Restrict);
    }


}
