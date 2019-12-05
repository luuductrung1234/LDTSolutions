using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using PieShop.Data;
using PieShop.Models;
using PieShop.ViewModels;

namespace PieShop.Controllers
{
   public class PiesController : Controller
   {
      private readonly IPieRepository _pieRepository;
      private readonly ICategoryRepository _categoryRepository;

      public PiesController(IPieRepository pieRepository, ICategoryRepository categoryRepository)
      {
         _pieRepository = pieRepository ?? throw new ArgumentNullException(nameof(pieRepository));
         _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
      }

      [HttpGet]
      public async Task<IActionResult> List(int? categoryId, bool? isPieOfTheWeek)
      {
         IEnumerable<Pie> pies = await _pieRepository.GetAllAsync(
            categoryId: categoryId,
            isPieOfTheWeeK: isPieOfTheWeek);

         Category category = null;
         if (categoryId.HasValue)
         {
            category = await _categoryRepository.GetByIdAsync(categoryId.Value);
         }

         PiesListViewModel viewData = new PiesListViewModel()
         {
            Pies = pies,
            CurrentCategory = category
         };

         ViewBag.Title = "Pies List";
         return View(viewData);
      }
   }
}