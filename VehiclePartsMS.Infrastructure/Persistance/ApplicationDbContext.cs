using Microsoft.EntityFrameworkCore;
using VehiclePartsMS.Domain.Models;

namespace VehiclePartsMS.Infrastructure.Persistance;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User>               Users               { get; set; } = null!;
    public DbSet<Vendor>             Vendors             { get; set; } = null!;
    public DbSet<VehiclePart>        VehicleParts        { get; set; } = null!;
    public DbSet<CustomerProfile>    CustomerProfiles    { get; set; } = null!;
    public DbSet<Vehicle>            Vehicles            { get; set; } = null!;
    public DbSet<SalesInvoice>       SalesInvoices       { get; set; } = null!;
    public DbSet<SalesInvoiceItem>   SalesInvoiceItems   { get; set; } = null!;
    public DbSet<ServiceAppointment> ServiceAppointments { get; set; } = null!;
    public DbSet<PartRequest>        PartRequests        { get; set; } = null!;
    public DbSet<Review>             Reviews             { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User (1) -> (0..1) CustomerProfile
        modelBuilder.Entity<CustomerProfile>()
            .HasOne(cp => cp.User)
            .WithOne(u => u.CustomerProfile)
            .HasForeignKey<CustomerProfile>(cp => cp.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // CustomerProfile (1) -> (many) Vehicle
        modelBuilder.Entity<Vehicle>()
            .HasOne(v => v.CustomerProfile)
            .WithMany(cp => cp.Vehicles)
            .HasForeignKey(v => v.CustomerProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        // Vendor (1) -> (many) VehiclePart
        modelBuilder.Entity<VehiclePart>()
            .HasOne(p => p.Vendor)
            .WithMany(v => v.Parts)
            .HasForeignKey(p => p.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        // CustomerProfile (1) -> (many) SalesInvoice
        modelBuilder.Entity<SalesInvoice>()
            .HasOne(si => si.CustomerProfile)
            .WithMany(cp => cp.SalesInvoices)
            .HasForeignKey(si => si.CustomerProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        // User/Staff (1) -> (many) SalesInvoice
        modelBuilder.Entity<SalesInvoice>()
            .HasOne(si => si.Staff)
            .WithMany(u => u.SalesInvoicesAsStaff)
            .HasForeignKey(si => si.StaffId)
            .OnDelete(DeleteBehavior.Restrict);

        // SalesInvoice (1) -> (many) SalesInvoiceItem
        modelBuilder.Entity<SalesInvoiceItem>()
            .HasOne(item => item.SalesInvoice)
            .WithMany(si => si.Items)
            .HasForeignKey(item => item.SalesInvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        // VehiclePart (1) -> (many) SalesInvoiceItem
        modelBuilder.Entity<SalesInvoiceItem>()
            .HasOne(item => item.Part)
            .WithMany(p => p.SalesInvoiceItems)
            .HasForeignKey(item => item.PartId)
            .OnDelete(DeleteBehavior.Restrict);

        // CustomerProfile (1) -> (many) ServiceAppointment
        modelBuilder.Entity<ServiceAppointment>()
            .HasOne(a => a.CustomerProfile)
            .WithMany(cp => cp.Appointments)
            .HasForeignKey(a => a.CustomerProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        // Vehicle (0..1) -> (many) ServiceAppointment
        modelBuilder.Entity<ServiceAppointment>()
            .HasOne(a => a.Vehicle)
            .WithMany(v => v.Appointments)
            .HasForeignKey(a => a.VehicleId)
            .OnDelete(DeleteBehavior.SetNull);

        // CustomerProfile (1) -> (many) PartRequest
        modelBuilder.Entity<PartRequest>()
            .HasOne(pr => pr.CustomerProfile)
            .WithMany(cp => cp.PartRequests)
            .HasForeignKey(pr => pr.CustomerProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        // CustomerProfile (1) -> (many) Review
        modelBuilder.Entity<Review>()
            .HasOne(r => r.CustomerProfile)
            .WithMany(cp => cp.Reviews)
            .HasForeignKey(r => r.CustomerProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email).IsUnique();

        modelBuilder.Entity<Vehicle>()
            .HasIndex(v => v.VehicleNumber).IsUnique();
    }
}
