using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YemekSitesi.Models;

namespace YemekSitesi.ViewModels
{
    public class ShowRecipeViewModel
    {
        public Recipe Recipe { get; set; }
        public CommentViewModel CommentViewModel { get; set; }
    }
}