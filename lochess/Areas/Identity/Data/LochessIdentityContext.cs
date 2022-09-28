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
    public DbSet<Game> Games { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Assign the 'AspNetUser' model to the 'AspNetUsers' table
        builder.Entity<AspNetUser>().ToTable("AspNetUsers");
        base.OnModelCreating(builder);

        // Assign the 'Connection' model to the 'Connections' table
        builder.Entity<Connection>().ToTable("Connections");
        base.OnModelCreating(builder);

        // Assign the 'Game' model to the 'Games' table
        builder.Entity<Game>().ToTable("Games");
        base.OnModelCreating(builder);
    }
}

public class AspNetUserEntityConfiguration : IEntityTypeConfiguration<AspNetUser>
{
    public void Configure(EntityTypeBuilder<AspNetUser> builder)
    {
        builder.Property(a => a.GroupName).HasMaxLength(255);
    }
}

