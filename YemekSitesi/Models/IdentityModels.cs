using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace YemekSitesi.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("DisplayName", DisplayName));
            return userIdentity;
        }
        [Required]
        [MaxLength(30)]
        public string DisplayName { get; set; }
        [MaxLength(100)]
        public string ProfilePhoto { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("ApplicationDbContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>()
                .HasRequired(x => x.Category)
                .WithMany(x => x.Recipes)
                .HasForeignKey(x => x.CategoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Comment>()
                 .HasRequired(x => x.Author)
                 .WithMany(x => x.Comments)
                 .HasForeignKey(x => x.AuthorId)
                 .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
    }
}