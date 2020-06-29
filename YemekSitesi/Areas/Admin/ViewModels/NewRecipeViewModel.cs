using YemekSitesi.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YemekSitesi.Models;

namespace YemekSitesi.Areas.Admin.ViewModels
{
    public class NewRecipeViewModel
    {
        public int CategoryId { get; set; }
        public int[] IngredientId { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        [AllowHtml]
        public string Directions { get; set; }
        [PostedImage]
        public HttpPostedFileBase FeaturedImage { get; set; }
        [Required]
        [Display(Name = "Short Url")]
        [StringLength(200)]
        public string Slug { get; set; }
    }
}