using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityWithEFDemo.Identity
{
   public class AppIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
   {
      public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
      {

      }
   }
}
