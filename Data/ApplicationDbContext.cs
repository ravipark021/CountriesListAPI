using CountriesListApi.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CountriesListApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Country> Country { get; set; }
        public DbSet<Favorites> Favorites { get; set; }
    }
}