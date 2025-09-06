using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmiTo.Domain.Entities;

namespace SmiTo.Infrastructure.Persistence.Configurations;

public class Visit_Configuration : IEntityTypeConfiguration<Visit>
{
    public void Configure(EntityTypeBuilder<Visit> builder)
    {

        builder.Property(v => v.VisitedAt)
            .IsRequired();

        builder.Property(v => v.VisitorIp)
            .IsRequired()
            .HasMaxLength(45);

        builder.Property(v => v.UserAgent)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(v => v.DeviceType)
            .HasMaxLength(200);

        builder.Property(v => v.Browser)
            .HasMaxLength(200);

        // Relationships
        builder.HasOne(v => v.URL)
            .WithMany()
            .HasForeignKey(v => v.URLId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
