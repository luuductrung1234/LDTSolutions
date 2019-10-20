using Microsoft.EntityFrameworkCore;
using PieShop.Models;

namespace PieShop.Data
{
   public class AppDbContext : DbContext
   {
      public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
      {

      }

      public DbSet<Pie> Pies { get; set; }

      public DbSet<Category> Categories { get; set; }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         base.OnModelCreating(modelBuilder);

         // seed categories
         modelBuilder.Entity<Category>().HasKey(c => c.Id);
         modelBuilder.Entity<Category>().Property(c => c.Id).ValueGeneratedOnAdd();
         modelBuilder.Entity<Category>().HasMany<Pie>(c => c.Pies).WithOne(p => p.Category);
         modelBuilder.Entity<Category>().HasData(new Category(1, "Fruit pies", "All-fruity pies"));
         modelBuilder.Entity<Category>().HasData(new Category(2, "Cheese cakes", "Cheesy all the way"));
         modelBuilder.Entity<Category>().HasData(new Category(3, "Seasonal pies", "Get in the mood for a seasonal pie"));


         // seed pies
         modelBuilder.Entity<Pie>().HasKey(p => p.Id);
         modelBuilder.Entity<Pie>().Property(p => p.Id).ValueGeneratedOnAdd();
         modelBuilder.Entity<Pie>().HasOne<Category>(p => p.Category).WithMany(c => c.Pies);
         modelBuilder.Entity<Pie>().HasData(new Pie(1,
               "Strawberry Pie",
               "Lorem Ipsum",
               "",
               "Icing carrot cake jelly-o cheesecake. Sweet roll marzipan marshmallow toffee brownie brownie candy tootsie roll. Chocolate cake gingerbread tootsie roll oat cake pie chocolate bar cookie dragée brownie. Lollipop cotton candy cake bear claw oat cake. Dragée candy canes dessert tart. Marzipan dragée gummies lollipop jujubes chocolate bar candy canes. Icing gingerbread chupa chups cotton candy cookie sweet icing bonbon gummies. Gummies lollipop brownie biscuit danish chocolate cake. Danish powder cookie macaroon chocolate donut tart. Carrot cake dragée croissant lemon drops liquorice lemon drops cookie lollipop toffee. Carrot cake carrot cake liquorice sugar plum topping bonbon pie muffin jujubes. Jelly pastry wafer tart caramels bear claw. Tiramisu tart pie cake danish lemon drops. Brownie cupcake dragée gummies.",
               15.95M,
               "https://gillcleerenpluralsight.blob.core.windows.net/files/strawberrypie.jpg",
               "https://gillcleerenpluralsight.blob.core.windows.net/files/strawberrypiesmall.jpg",
               true,
               false,
               1,
               ""));
         modelBuilder.Entity<Pie>().HasData(new Pie(2,
               "Cheese cake",
               "Lorem Ipsum",
               "",
               "Icing carrot cake jelly-o cheesecake. Sweet roll marzipan marshmallow toffee brownie brownie candy tootsie roll. Chocolate cake gingerbread tootsie roll oat cake pie chocolate bar cookie dragée brownie. Lollipop cotton candy cake bear claw oat cake. Dragée candy canes dessert tart. Marzipan dragée gummies lollipop jujubes chocolate bar candy canes. Icing gingerbread chupa chups cotton candy cookie sweet icing bonbon gummies. Gummies lollipop brownie biscuit danish chocolate cake. Danish powder cookie macaroon chocolate donut tart. Carrot cake dragée croissant lemon drops liquorice lemon drops cookie lollipop toffee. Carrot cake carrot cake liquorice sugar plum topping bonbon pie muffin jujubes. Jelly pastry wafer tart caramels bear claw. Tiramisu tart pie cake danish lemon drops. Brownie cupcake dragée gummies.",
               18.95M,
               "https://gillcleerenpluralsight.blob.core.windows.net/files/cheesecake.jpg",
               "https://gillcleerenpluralsight.blob.core.windows.net/files/cheesecakesmall.jpg",
               true,
               false,
               2,
               ""));
         modelBuilder.Entity<Pie>().HasData(new Pie(3,
               "Rhubarb Pie",
               "Lorem Ipsum",
               "",
               "Icing carrot cake jelly-o cheesecake. Sweet roll marzipan marshmallow toffee brownie brownie candy tootsie roll. Chocolate cake gingerbread tootsie roll oat cake pie chocolate bar cookie dragée brownie. Lollipop cotton candy cake bear claw oat cake. Dragée candy canes dessert tart. Marzipan dragée gummies lollipop jujubes chocolate bar candy canes. Icing gingerbread chupa chups cotton candy cookie sweet icing bonbon gummies. Gummies lollipop brownie biscuit danish chocolate cake. Danish powder cookie macaroon chocolate donut tart. Carrot cake dragée croissant lemon drops liquorice lemon drops cookie lollipop toffee. Carrot cake carrot cake liquorice sugar plum topping bonbon pie muffin jujubes. Jelly pastry wafer tart caramels bear claw. Tiramisu tart pie cake danish lemon drops. Brownie cupcake dragée gummies.",
               15.95M,
               "https://gillcleerenpluralsight.blob.core.windows.net/files/rhubarbpie.jpg",
               "https://gillcleerenpluralsight.blob.core.windows.net/files/rhubarbpiesmall.jpg",
               true,
               true,
               1,
               ""));
         modelBuilder.Entity<Pie>().HasData(new Pie(4,
               "Pumpkin Pie",
               "Lorem Ipsum",
               "",
               "Icing carrot cake jelly-o cheesecake. Sweet roll marzipan marshmallow toffee brownie brownie candy tootsie roll. Chocolate cake gingerbread tootsie roll oat cake pie chocolate bar cookie dragée brownie. Lollipop cotton candy cake bear claw oat cake. Dragée candy canes dessert tart. Marzipan dragée gummies lollipop jujubes chocolate bar candy canes. Icing gingerbread chupa chups cotton candy cookie sweet icing bonbon gummies. Gummies lollipop brownie biscuit danish chocolate cake. Danish powder cookie macaroon chocolate donut tart. Carrot cake dragée croissant lemon drops liquorice lemon drops cookie lollipop toffee. Carrot cake carrot cake liquorice sugar plum topping bonbon pie muffin jujubes. Jelly pastry wafer tart caramels bear claw. Tiramisu tart pie cake danish lemon drops. Brownie cupcake dragée gummies.",
               12.95M,
               "https://gillcleerenpluralsight.blob.core.windows.net/files/pumpkinpie.jpg",
               "https://gillcleerenpluralsight.blob.core.windows.net/files/pumpkinpiesmall.jpg",
               true,
               true,
               3,
               ""));
      }
   }
}
