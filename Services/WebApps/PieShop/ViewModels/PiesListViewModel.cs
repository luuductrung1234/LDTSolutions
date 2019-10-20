using PieShop.Models;

using System.Collections.Generic;

namespace PieShop.ViewModels
{
   public class PiesListViewModel
   {
      public IEnumerable<Pie> Pies { get; set; }

      public Category CurrentCategory { get; set; }
   }
}
