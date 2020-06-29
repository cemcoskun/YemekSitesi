using Microsoft.AspNet.Identity;
using YemekSitesi.Areas.Admin.ViewModels;
using YemekSitesi.Helpers;
using YemekSitesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace YemekSitesi.Areas.Admin.Controllers
{
    public class RecipesController : AdminBaseController
    {
        // GET: Admin/Posts
        public ActionResult Index()
        {
            return View(db.Recipes.ToList());
        }
        public ActionResult New()
        {
            ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x => x.CategoryName).ToList(), "Id", "CategoryName");
            var ingredients = db.Ingredients.Select(x => new
            {
                IngredientId = x.Id,
                IngredientName = x.IngredientName
            });
            ViewBag.Ingredients = new MultiSelectList(ingredients.ToList(), "IngredientId", "IngredientName");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(NewRecipeViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Recipe recipe = new Recipe
                {
                    CategoryId = vm.CategoryId,
                    Title = vm.Title,
                    AuthorId = User.Identity.GetUserId(),
                    Directions = vm.Directions,
                    Slug = UrlService.URLFriendly(vm.Slug),
                    CreationTime = DateTime.Now,
                    ModificationTime = DateTime.Now,
                    PhotoPath = this.SaveImage(vm.FeaturedImage)
                };
                var selectedIngredients = new List<Ingredient>();
                foreach (var item in vm.IngredientId)
                {
                    selectedIngredients.Add(db.Ingredients.FirstOrDefault(x => x.Id == item));
                }
                recipe.Ingredients = selectedIngredients;
                db.Recipes.Add(recipe);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Post has been created successfully.";

                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x => x.CategoryName).ToList(), "Id", "CategoryName");
            return View();
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            var ingredients = db.Ingredients.Select(x => new
            {
                IngredientId = x.Id,
                IngredientName = x.IngredientName
            });
            ViewBag.Ingredients = new MultiSelectList(ingredients.ToList(), "IngredientId", "IngredientName");
            var vm = new EditRecipeViewModel
            {
                Id = recipe.Id,
                CategoryId = recipe.CategoryId,
                Directions = recipe.Directions,
                CreationTime = recipe.CreationTime.Value,
                CurrentFeaturedImage = recipe.PhotoPath,
                ModificationTime = recipe.ModificationTime.Value,
                Slug = recipe.Slug,
                Title = recipe.Title
            };
            ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x => x.CategoryName).ToList(), "Id", "CategoryName");
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(EditRecipeViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var recipe = db.Recipes.Find(vm.Id);
                recipe.CategoryId = vm.CategoryId;
                recipe.Directions = vm.Directions;
                recipe.ModificationTime = DateTime.Now;
                recipe.Slug = UrlService.URLFriendly(vm.Slug);
                if (vm.FeaturedImage != null)
                {
                    this.DeleteImage(recipe.PhotoPath);
                    recipe.PhotoPath = this.SaveImage(vm.FeaturedImage);
                }
                var selectedIngredients = new List<Ingredient>();
                foreach (var item in vm.IngredientId)
                {
                    selectedIngredients.Add(db.Ingredients.FirstOrDefault(x => x.Id == item));
                }
                if (selectedIngredients != recipe.Ingredients)
                {
                    recipe.Ingredients = selectedIngredients;
                }
                db.SaveChanges();
                TempData["SuccessMessage"] = "Recipe has been updated successfully.";
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x => x.CategoryName).ToList(), "Id", "CategoryName");
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            this.DeleteImage(recipe.PhotoPath);
            db.Recipes.Remove(recipe);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Recipe has been deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}