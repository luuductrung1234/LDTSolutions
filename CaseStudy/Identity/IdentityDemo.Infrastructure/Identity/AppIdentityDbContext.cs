using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityDemo.Infrastructure.Identity
{
   public class AppIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
   {
      public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
      {

      }

      protected override void OnModelCreating(ModelBuilder builder)
      {
         base.OnModelCreating(builder);

         builder.Entity<ApplicationUser>(user => user.HasIndex(x => x.Locale).IsUnique(false));

         builder.Entity<Organization>(org =>
         {
            org.ToTable("Organizations");
            org.HasKey(x => x.Id);

            org.HasMany<ApplicationUser>().WithOne().HasForeignKey(x => x.OrgId).IsRequired(false);
         });
      }
   }
}
