using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using YemekSitesi.Models;

namespace YemekSitesi.Helpers
{
    public static class IdentityHelpers
    {
        public static string DisplayName(this IIdentity identity)
        {
            var claims = ((ClaimsIdentity)identity).Claims;

            string displayName = claims.FirstOrDefault(x => x.Type == "DisplayName").Value;

            return displayName;
        }

        public static string ProfilePhoto(this IIdentity identity)
        {
            using (var db = new ApplicationDbContext())
            {
                string userId = identity.GetUserId();
                var user = db.Users.Find(userId);
                return user.ProfilePhoto;
            }
        }

        public static void SeedRolesAndUsers()
        {
            var context = new ApplicationDbContext();

            // https://stackoverflow.com/questions/19280527/mvc-5-seed-users-and-roles
            if (!context.Roles.Any(r => r.Name == "admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "admin" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "cem10195@gmail.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    UserName = "cem10195@gmail.com",
                    Email = "cem10195@gmail.com",
                    DisplayName = "Cem Coşkun",
                    EmailConfirmed = true
                };

                manager.Create(user, "Password1.");
                manager.AddToRole(user.Id, "admin");

                #region Seed Categories and Recipes
                if (!context.Categories.Any())
                {
                    context.Categories.Add(new Category
                    {
                        CategoryName = "Sample Category 1",
                        Slug = "sample-category-1",
                        Recipes = new List<Recipe>
                        {
                            new Recipe
                            {
                                Title = "Sample Recipe 1",
                                AuthorId = user.Id,
                                Ingredients = new List<Ingredient>{
                                    new Ingredient { IngredientName = "Domates" },
                                    new Ingredient { IngredientName = "Zeytin Yağı" },
                                    new Ingredient { IngredientName = "Salatalık" }
                                },
                                Directions = "Domatesi doğra, Zeytin Yağını kızart, Salatalığı doğra",
                                Slug = "sample-recipe-1",
                                CreationTime = DateTime.Now,
                                ModificationTime = DateTime.Now
                            },
                            new Recipe
                            {
                                Title = "Sample Recipe 3",
                                AuthorId = user.Id,
                                Ingredients = new List<Ingredient>{
                                    new Ingredient { IngredientName = "Enginar" },
                                    new Ingredient { IngredientName = "Ayçiçek Yağı" },
                                    new Ingredient { IngredientName = "Kabak" }
                                },
                                Directions = "Enginarı doğra, Ayçiçek Yağını kızart, Kabağı doğra",
                                Slug = "sample-recipe-2",
                                CreationTime = DateTime.Now,
                                ModificationTime = DateTime.Now
                            },
                        }
                    });
                }
                #endregion
            }

            context.SaveChanges();
            context.Dispose();
        }
    }
}