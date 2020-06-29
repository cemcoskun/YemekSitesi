using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using X.PagedList;
using YemekSitesi.Models;

namespace YemekSitesi.ViewModels
{
    public class HomeIndexViewModel

    {
        public IPagedList<Recipe> Recipes { get; set; }
        public Category Category { get; set; }
        public string SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public int[] IngredientId { get; set; }
    }
}