using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Infrastructure.Persistence.Configurations;

public class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.ToTable("Properties");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(6)
            .IsFixedLength();

        builder.HasIndex(p => p.Code)
            .IsUnique();

        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)"); 

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(p => p.Size)
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        builder.Property(p => p.Rooms)
            .IsRequired();

        builder.Property(p => p.Bathrooms)
            .IsRequired();

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<string>()  
            .HasMaxLength(20);

        builder.Property(p => p.AgentId)
            .IsRequired()
            .HasMaxLength(450); 

        builder.HasIndex(p => p.AgentId); 

        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.CreatedBy).HasMaxLength(450);
        builder.Property(p => p.LastModifiedBy).HasMaxLength(450);


        builder.HasOne(p => p.PropertyType)
            .WithMany(pt => pt.Properties)
            .HasForeignKey(p => p.PropertyTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.SaleType)
            .WithMany(st => st.Properties)
            .HasForeignKey(p => p.SaleTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}