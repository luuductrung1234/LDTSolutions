using System.Collections.Generic;

namespace PieShop.Models
{
   public class Category
   {
      public int Id { get; set; }

      public string Name { get; set; }

      public string Description { get; set; }

      public IEnumerable<Pie> Pies { get; set; }

      #region Constructors

      public Category(int id, string name, string description)
      {
         Id = id;
         Name = name;
         Description = description;
      }

      public Category(string categoryName, string description)
      {
         Name = categoryName;
         Description = description;
      }

      public Category(string categoryName, string description, IEnumerable<Pie> pies)
      {
         Name = categoryName;
         Description = description;
         Pies = pies;
      }

      #endregion
   }
}
