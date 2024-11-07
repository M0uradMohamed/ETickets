using ETickets.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ETickets.Areas.Admin.ViewModels;

namespace ETickets.Data
{

    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

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

public DbSet<ETickets.Areas.Admin.ViewModels.ActorVM> ActorVM { get; set; } = default!;

public DbSet<ETickets.Areas.Admin.ViewModels.CategoryVM> CategoryVM { get; set; } = default!;

public DbSet<ETickets.Areas.Admin.ViewModels.CinemaVM> CinemaVM { get; set; } = default!;

    }
}

