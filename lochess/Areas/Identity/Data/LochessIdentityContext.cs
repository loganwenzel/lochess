using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace lochess.Areas.Identity.Data;

public class LochessIdentityContext : IdentityDbContext<IdentityUser>
{
    public LochessIdentityContext(DbContextOptions<LochessIdentityContext> options)
        : base(options)
    {
    }

    protected LochessIdentityContext(DbContextOptions options)
    : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        //builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
    }
}

//public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
//{
//    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
//    {
//        builder.Property(u => u.FirstName).HasMaxLength(255);
//        builder.Property(u => u.LastName).HasMaxLength(255);
//    }
//}

