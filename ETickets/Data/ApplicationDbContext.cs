using ETickets.Models;
using Microsoft.EntityFrameworkCore;

namespace ETickets.Data
{

    public class ApplicationDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<ActorMovie> ActorsMovies { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
: base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Movie>().HasMany(m => m.Actors).WithMany(a => a.Movies).
                UsingEntity<ActorMovie>().HasKey(j => new { j.MovieId, j.ActorId });
        }
    }
}

