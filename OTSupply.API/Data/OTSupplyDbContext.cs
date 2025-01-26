using Microsoft.EntityFrameworkCore;
using OTSupply.API.Models.Domain;

namespace OTSupply.API.Data
{
    public class OTSupplyDbContext : DbContext
    {

        public OTSupplyDbContext(DbContextOptions dbContextOptions):base(dbContextOptions)
        {
                
        }

        public DbSet<Grad> Gradovi { get; set; }

        public DbSet<Kategorija> Kategorije { get; set; }

        public DbSet<Ocene> Ocene { get; set; }

        public DbSet<Oglas> Oglasi { get; set; }
    }
}
 