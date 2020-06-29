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
    public class EditRecipeViewModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int[] IngredientId { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [AllowHtml]
        public List<Ingredient> Ingredients { get; set; }
        [AllowHtml]
        public string Directions { get; set; }
        public string CurrentFeaturedImage { get; set; }
        [Display(Name = "Created")]
        public DateTime CreationTime { get; set; }
        [Display(Name = "Last Modified")]
        public DateTime ModificationTime { get; set; }
        [PostedImage]
        public HttpPostedFileBase FeaturedImage { get; set; }
        [Required]
        [Display(Name = "Short Url")]
        [StringLength(200)]
        public string Slug { get; set; }
    }
}