using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class RealEstateDbContext : DbContext
{
    public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options)
        : base(options) { }

    public DbSet<Owner> Owners => Set<Owner>();
    public DbSet<Property> Properties => Set<Property>();
    public DbSet<PropertyImage> PropertyImages => Set<PropertyImage>();
    public DbSet<PropertyTrace> PropertyTraces => Set<PropertyTrace>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Owner>(e =>
        {
            e.ToTable("Owner", "dbo");
            e.HasKey(x => x.IdOwner);
            e.Property(x => x.Name).HasMaxLength(150).IsRequired();
            e.Property(x => x.Address).HasMaxLength(200);
            e.Property(x => x.Photo).HasMaxLength(500);
        });

        b.Entity<Property>(e =>
        {
            e.ToTable("Property", "dbo");
            e.HasKey(x => x.IdProperty);
            e.Property(x => x.Name).HasMaxLength(150).IsRequired();
            e.Property(x => x.Address).HasMaxLength(200).IsRequired();
            e.Property(x => x.Price).HasColumnType("decimal(18,2)").IsRequired();
            e.Property(x => x.CodeInternal).HasMaxLength(50).IsRequired();
            e.Property(x => x.CreatedAt).HasColumnType("datetime2(0)");
            e.Property(x => x.UpdatedAt).HasColumnType("datetime2(0)");
            e.HasIndex(x => x.CodeInternal).IsUnique();
            e.HasIndex(x => x.Price);
            e.HasIndex(x => x.Year);
            e.HasIndex(x => x.CreatedAt);
            e.HasOne(x => x.Owner).WithMany(o => o.Properties).HasForeignKey(x => x.IdOwner);
        });

        b.Entity<PropertyImage>(e =>
        {
            e.ToTable("PropertyImage", "dbo");
            e.HasKey(x => x.IdPropertyImage);
            e.Property(x => x.File).HasMaxLength(500).IsRequired();
            e.Property(x => x.CreatedAt).HasColumnType("datetime2(0)");
            e.HasIndex(x => new { x.IdProperty, x.Enabled });
            e.HasOne(x => x.Property).WithMany(p => p.Images).HasForeignKey(x => x.IdProperty);
        });

        b.Entity<PropertyTrace>(e =>
        {
            e.ToTable("PropertyTrace", "dbo");
            e.HasKey(x => x.IdPropertyTrace);
            e.Property(x => x.Name).HasMaxLength(150).IsRequired();
            e.Property(x => x.Value).HasColumnType("decimal(18,2)");
            e.Property(x => x.Tax).HasColumnType("decimal(18,2)");
            e.HasIndex(x => new { x.IdProperty, x.DateSale });
            e.HasOne(x => x.Property).WithMany(p => p.Traces).HasForeignKey(x => x.IdProperty);
        });
    }
}
