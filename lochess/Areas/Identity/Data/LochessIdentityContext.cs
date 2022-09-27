using lochess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace lochess.Areas.Identity.Data;

public class LochessIdentityContext : IdentityDbContext<AspNetUser>
{
    public LochessIdentityContext(DbContextOptions<LochessIdentityContext> options)
        : base(options)
    {
    }

    protected LochessIdentityContext(DbContextOptions options)
    : base(options)
    {
    }

    public DbSet<AspNetUser> AspNetUsers { get; set; }
    public DbSet<Connection> Connections { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.Entity<AspNetUser>().ToTable("AspNetUsers");
        base.OnModelCreating(builder);

        builder.Entity<Connection>().ToTable("Connections");
        base.OnModelCreating(builder);

        //builder.ApplyConfiguration(new AspNetUserEntityConfiguration());
    }
}

public class AspNetUserEntityConfiguration : IEntityTypeConfiguration<AspNetUser>
{
    public void Configure(EntityTypeBuilder<AspNetUser> builder)
    {
        builder.Property(a => a.GroupName).HasMaxLength(255);
    }
}

