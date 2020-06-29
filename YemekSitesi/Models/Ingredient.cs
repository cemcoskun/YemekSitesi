using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YemekSitesi.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        [Required]
        public string IngredientName { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}