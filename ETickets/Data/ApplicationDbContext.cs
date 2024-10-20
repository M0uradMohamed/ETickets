using ETickets.Models;
using Microsoft.EntityFrameworkCore;

namespace ETickets.Data
{

    public class ApplicationDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cinema> cinemas { get; set; }
        public DbSet<ActorMovie> ActorsMovies { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true).Build();

            var connection = builder.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connection);


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Movie>().HasMany(m => m.Actors).WithMany(a => a.Movies).
                UsingEntity<ActorMovie>().HasKey(j => new { j.MovieId, j.ActorId });
        }
    }
}

