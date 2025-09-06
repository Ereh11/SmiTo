using Microsoft.EntityFrameworkCore;
using SmiTo.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace SmiTo.Infrastructure.Persistence.Configurations;

public class URL_Configuration : IEntityTypeConfiguration<URL>
{
    public void Configure(EntityTypeBuilder<URL> builder)
    {
        builder.Property(u => u.OriginalUrl)
            .IsRequired()
            .HasMaxLength(5000);

        builder.Ignore(u => u.ShortenedUrl);

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.UpdatedAt);

        builder.Property(u => u.ExpiresAt);

        builder.Property(u => u.ClickCount)
            .HasDefaultValue(0);

        // Relationships
        builder.HasOne(u => u.User)
            .WithMany(user => user.URLs)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

