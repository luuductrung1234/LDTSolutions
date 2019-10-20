﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data
{
   public class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
   {
      public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
         : base(options)
      {

      }
   }
}
