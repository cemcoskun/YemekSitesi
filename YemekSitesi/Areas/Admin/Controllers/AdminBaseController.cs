using YemekSitesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YemekSitesi.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminBaseController : Controller
    {
        protected ApplicationDbContext db = new ApplicationDbContext();

        // GET: Base
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}