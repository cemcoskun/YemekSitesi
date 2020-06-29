﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YemekSitesi.Helpers;
using YemekSitesi.Models;

namespace YemekSitesi.Areas.Admin.Controllers
{
    public class CategoriesController : AdminBaseController
    {
        // GET: Admin/Categories
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }
        public ActionResult New()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = UrlService.URLFriendly(category.Slug);
                db.Categories.Add(category);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Category has been created successfully.";
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = db.Categories.Find(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = UrlService.URLFriendly(category.Slug);
                db.Entry(category).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Category has been updated successfully.";
                return RedirectToAction("Index");
            }

            return View(db.Categories.Find(category.Id));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            if (category.Recipes.Count > 0)
            {
                TempData["ErrorMessage"] = "You cannot delete a category that has a recipe";
            }
            db.Categories.Remove(category);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Category has been deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}