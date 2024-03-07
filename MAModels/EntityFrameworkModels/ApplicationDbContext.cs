using MAModels.EntityFrameworkModels.AI;
using MAModels.EntityFrameworkModels.Identity;
using MAModels.EntityFrameworkModels.Movie;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MAModels.EntityFrameworkModels
{
    public class ApplicationDbContext : IdentityDbContext<Users>
    {
        public ApplicationDbContext(
            DbContextOptions options            
            ) : base(options) { }

        public DbSet<Movies> Movies { get; set; }

        public DbSet<Images> Images { get; set; }

        public DbSet<Tags> Tags { get; set; }

        public DbSet<Reviews> Reviews { get; set; }

        public DbSet<Users> Users { get; set; }

        public DbSet<Requests> Requests { get; set; }

        public DbSet<Sessions> Sessions { get; set; }   

        public DbSet<Recommendations> Recommendations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
