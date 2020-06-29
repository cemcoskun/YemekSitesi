using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YemekSitesi.Models;
using YemekSitesi.ViewModels;

namespace YemekSitesi.Controllers
{
    public class RecipeController : BaseController
    {
        // GET: article/sample-recipe-1
        [Route("p/{id}/{slug?}")]
        public ActionResult Show(int id, string slug)
        {
            var recipe = db.Recipes.Find(id);

            if (recipe == null)
            {
                return HttpNotFound();
            }

            if (recipe.Slug != slug)
            {
                return RedirectToAction("Show", new { id = id, slug = recipe.Slug });
            }

            var vm = new ShowRecipeViewModel
            {
                Recipe = recipe,
                CommentViewModel = new CommentViewModel()
            };

            return View(vm);
        }

        [Route("p/{id}/{slug?}")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Show(int id, string slug, CommentViewModel commentViewModel)
        {
            var recipe = db.Recipes.Find(id);

            if (recipe == null)
            {
                return HttpNotFound();
            }

            if (recipe.Slug != slug)
            {
                return RedirectToAction("Show", new { id = id, slug = recipe.Slug });
            }

            if (ModelState.IsValid)
            {
                var comment = new Comment
                {
                    AuthorId = User.Identity.GetUserId(),
                    Content = commentViewModel.Content,
                    ParentId = commentViewModel.ParentId,
                    CreationTime = DateTime.Now,
                    ModificationTime = DateTime.Now,
                    State = Enums.CommentState.Approved,
                    RecipeId = id,
                };
                db.Comments.Add(comment);
                db.SaveChanges();
                return Redirect(Url.RouteUrl(
                    new
                    {
                        controller = "Recipe",
                        action = "Show",
                        id = id,
                        slug = slug,
                        commentSuccess = true
                    })
                    + "#leave-a-comment");
            }

            var vm = new ShowRecipeViewModel
            {
                Recipe = recipe,
                CommentViewModel = commentViewModel
            };

            return View(vm);
        }
    }
}