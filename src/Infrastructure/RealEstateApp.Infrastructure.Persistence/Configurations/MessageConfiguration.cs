using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Infrastructure.Persistence.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Content)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(m => m.SentAt)
            .IsRequired();

        builder.Property(m => m.ClientId).IsRequired().HasMaxLength(450);
        builder.Property(m => m.AgentId).IsRequired().HasMaxLength(450);
        builder.Property(m => m.SenderId).IsRequired().HasMaxLength(450);

        builder.HasIndex(m => new { m.PropertyId, m.ClientId, m.AgentId });

        builder.HasOne(m => m.Property)
            .WithMany(p => p.Messages)
            .HasForeignKey(m => m.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(m => m.CreatedAt).IsRequired();
    }
}