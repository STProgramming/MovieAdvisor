using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace MAModels.EntityFrameworkModels
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<MovieTag> MoviesTags { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<MovieUser> MoviesUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
