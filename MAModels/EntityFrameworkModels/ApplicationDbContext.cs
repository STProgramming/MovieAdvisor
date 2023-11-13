using Microsoft.EntityFrameworkCore;

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
            #region RELAZIONI MOVIES

            #region RELAZIONE UNO A MOLTI CON MOVIE IMAGES

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.ImagesList)
                .WithOne(m => m.Movie)
                .HasForeignKey("MovieId")
                .IsRequired();

            #endregion

            #region RELAZIONE MOLTI A MOLTI CON TAGS, MOVIES TAGS

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.TagsList)
                .WithMany(t => t.MoviesList)
                .UsingEntity("MovieTag",
                m => m.HasOne(typeof(Movie)).WithMany().HasForeignKey("MovieId").HasPrincipalKey(nameof(Movie.Id)),
                t => t.HasOne(typeof(Tag)).WithMany().HasForeignKey("MovieTagId").HasPrincipalKey(nameof(MovieTag.Id)),
                u => u.HasKey("MovieId", "MovieTagId"));

            #endregion

            #region RELAZIONE MOLTI A MOLTI CON USERS, MOVIES USERS

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.UsersList)
                .WithMany(u => u.MoviesList)
                .UsingEntity("MovieUser",
                m => m.HasOne(typeof(User)).WithMany().HasForeignKey("UserId").HasPrincipalKey(nameof(User.Id)),
                u => u.HasOne(typeof(Movie)).WithMany().HasForeignKey("MovieId").HasPrincipalKey(nameof(Movie.Id)),
                u => u.HasKey("MovieId", "UserId"));

            #endregion

            #endregion

            #region RELAZIONI USERS

            #region RELAZIONE UNO A MOLTI CON REVIEWS

            modelBuilder.Entity<User>()
                .HasMany(u => u.ReviewsList)
                .WithOne(r => r.User)
                .HasForeignKey("UserId")
                .IsRequired();

            #endregion

            #endregion
        }
    }
}
