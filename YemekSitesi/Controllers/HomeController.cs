using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.PagedList;
using YemekSitesi.Models;
using YemekSitesi.ViewModels;

namespace YemekSitesi.Controllers
{
    public class HomeController : BaseController
    {
        [Route("", Order = 2, Name = "HomeDefault")]
        [Route("c/{cid}/{slug}", Order = 1)]
        public ActionResult Index(IngredientsViewModel IngVm, string q, int? cid, string slug, int page = 1)
        {
            var pageSize = 10;
            IQueryable<Recipe> recipes = db.Recipes;
            Category category = null;
            var ingredients = db.Ingredients.Select(x => new
            {
                IngredientId = x.Id,
                IngredientName = x.IngredientName
            });
            ViewBag.Ingredients = new MultiSelectList(ingredients.ToList(), "IngredientId", "IngredientName");
            if (IngVm.IngredientId != null)
            {
                TempData["DropBoxBosalt"] = true;
                var selectedIngredients = new List<Ingredient>();
                foreach (var item in IngVm.IngredientId)
                {
                    selectedIngredients.Add(db.Ingredients.FirstOrDefault(x => x.Id == item));
                }
                foreach (var item in selectedIngredients)
                {
                    recipes = recipes.Where(x => x.Ingredients.Where(y => y.Id == item.Id).Any());
                }
            }
            if (q != null)
            {
                recipes = recipes.Where(x => x.Category.CategoryName.Contains(q)
                || x.Title.Contains(q));
            }
            if (cid != null && q == null)
            {
                category = db.Categories.Find(cid);
                if (category == null)
                {
                    return HttpNotFound();
                }

                recipes = recipes.Where(x => x.CategoryId == cid);
            }
            var vm = new HomeIndexViewModel
            {
                Recipes = recipes.OrderByDescending(x => x.CreationTime).ToPagedList(page, pageSize),
                Category = category,
                SearchTerm = q,
                CategoryId = cid
            };

            return View(vm);
        }

        /*[HttpPost]
        public ActionResult Index(IngredientsViewModel IngVm, string q, int? cid, string slug, int page = 1 )
        {
            var pageSize = 10;
            IQueryable<Recipe> recipes = db.Recipes;
            Category category = null;
            var ingredients = db.Ingredients.Select(x => new
            {
                IngredientId = x.Id,
                IngredientName = x.IngredientName
            });
            ViewBag.Ingredients = new MultiSelectList(ingredients.ToList(), "IngredientId", "IngredientName");
            if (ModelState.IsValid)
            {
                var selectedIngredients = new List<Ingredient>();
                foreach (var item in IngVm.IngredientId)
                {
                    selectedIngredients.Add(db.Ingredients.FirstOrDefault(x => x.Id == item));
                }
            }
            if (q != null)
            {
                recipes = recipes.Where(x => x.Category.CategoryName.Contains(q)
                || x.Title.Contains(q));
            }
            if (cid != null && q == null)
            {
                category = db.Categories.Find(cid);
                if (category == null)
                {
                    return HttpNotFound();
                }

                recipes = recipes.Where(x => x.CategoryId == cid);
            }
            var vm = new HomeIndexViewModel
            {
                Recipes = recipes.OrderByDescending(x => x.CreationTime).ToPagedList(page, pageSize),
                Category = category,
                SearchTerm = q,
                CategoryId = cid
            };

            return View(vm);
        }*/

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CategoriesPartial()
        {
            return PartialView("_CategoriesPartial",
            db.Categories.OrderBy(x => x.CategoryName).ToList());
        }
    }
}