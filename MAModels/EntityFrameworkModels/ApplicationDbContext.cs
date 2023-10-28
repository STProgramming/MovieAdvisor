using Microsoft.EntityFrameworkCore;

namespace MAModels.EntityFrameworkModels
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<MovieDescription> MoviesDescriptions { get; set; }

        public DbSet<MovieTag> MoviesTags { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
