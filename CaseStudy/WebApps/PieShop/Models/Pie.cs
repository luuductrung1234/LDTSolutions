using System.ComponentModel.DataAnnotations.Schema;

namespace PieShop.Models
{
   public class Pie
   {
      public int Id { get; set; }

      public string Name { get; set; }

      public string ShortDescription { get; set; }

      public string LongDescription { get; set; }

      public string AllergyInformation { get; set; }

      [Column(TypeName = "decimal(5, 2)")]
      public decimal Price { get; set; }

      public string ImageUrl { get; set; }

      public string ImageThumbnailUrl { get; set; }

      public bool IsPieOfTheWeek { get; set; }

      public bool InStock { get; set; }

      public int CategoryId { get; set; }

      public Category Category { get; set; }

      public string Notes { get; set; }


      #region Constructors

      public Pie(int id, string name, string shortDescription, string longDescription, string allergyInformation, decimal price, string imageUrl, string imageThumbnailUrl, bool isPieOfTheWeek, bool inStock, int categoryId, string notes)
      {
         Id = id;
         Name = name;
         ShortDescription = shortDescription;
         LongDescription = longDescription;
         AllergyInformation = allergyInformation;
         Price = price;
         ImageUrl = imageUrl;
         ImageThumbnailUrl = imageThumbnailUrl;
         IsPieOfTheWeek = isPieOfTheWeek;
         InStock = inStock;
         CategoryId = categoryId;
         Notes = notes;
      }

      public Pie(string name, string shortDescription, string longDescription, string allergyInformation, decimal price, string imageUrl, string imageThumbnailUrl, bool isPieOfTheWeek, bool inStock, int categoryId, string notes)
      {
         Name = name;
         ShortDescription = shortDescription;
         LongDescription = longDescription;
         AllergyInformation = allergyInformation;
         Price = price;
         ImageUrl = imageUrl;
         ImageThumbnailUrl = imageThumbnailUrl;
         IsPieOfTheWeek = isPieOfTheWeek;
         InStock = inStock;
         CategoryId = categoryId;
         Notes = notes;
      }



      #endregion
   }
}
