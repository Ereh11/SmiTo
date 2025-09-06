using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmiTo.Domain.Entities;

namespace SmiTo.Infrastructure.Persistence.Context;

public class SmiToDbContext
  : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public SmiToDbContext(DbContextOptions<SmiToDbContext> options) : base(options) { }

    public DbSet<URL> URLs { get; set; }
    public DbSet<Visit> Visits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmiToDbContext).Assembly);
    }
}

