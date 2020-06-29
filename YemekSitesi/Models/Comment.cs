using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using YemekSitesi.Enums;

namespace YemekSitesi.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        public string AuthorId { get; set; }
        public int? ParentId { get; set; }
        [Required]
        public int RecipeId { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime? CreationTime { get; set; }
        [Required]
        public DateTime? ModificationTime { get; set; }
        public CommentState State { get; set; }
        [ForeignKey("AuthorId")]
        public virtual ApplicationUser Author { get; set; }
        public virtual Recipe Recipe { get; set; }
        public virtual Comment Parent { get; set; }
        public virtual ICollection<Comment> Children { get; set; }
    }
}