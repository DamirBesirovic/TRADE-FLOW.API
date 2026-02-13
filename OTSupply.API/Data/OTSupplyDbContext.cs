using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OTSupply.API.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace OTSupply.API.Data
{
    public class OTSupplyDbContext : IdentityDbContext<Korisnik>
    {
        public OTSupplyDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Grad> Gradovi { get; set; }
        public DbSet<Kategorija> Kategorije { get; set; }
        public DbSet<Ocene> Ocene { get; set; }
        public DbSet<Oglas> Oglasi { get; set; }
        public DbSet<ImageUrl> ImageUrls { get; set; }
        public DbSet<Prodavac> Prodavci { get; set; }

        public DbSet<Zahtev> Zahtevi { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Table Mappings
            // Map Identity tables
            builder.Entity<Korisnik>().ToTable("AspNetUsers");
            builder.Entity<IdentityRole>().ToTable("AspNetRoles");
            builder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens");

            // Map custom entities
            builder.Entity<Grad>().ToTable("Gradovi");
            builder.Entity<Kategorija>().ToTable("Kategorije");
            builder.Entity<Ocene>().ToTable("Ocene");
            builder.Entity<Oglas>().ToTable("Oglasi");
            builder.Entity<ImageUrl>().ToTable("ImageUrls");
            builder.Entity<Prodavac>().ToTable("Prodavci");
            #endregion

            #region Relationships
            // One Prodavac can have many Oglasi
            builder.Entity<Prodavac>()
                .HasMany(p => p.Oglasi)
                .WithOne(o => o.Prodavac)
                .HasForeignKey(o => o.Prodavac_Id)
                .OnDelete(DeleteBehavior.Cascade);

            // One Oglas belongs to one Kategorija
            builder.Entity<Oglas>()
                .HasOne(o => o.Kategorija)
                .WithMany() // Kategorija doesn't need a collection of Oglasi
                .HasForeignKey(o => o.Kategorija_Id)
                .OnDelete(DeleteBehavior.Restrict);

            // One Oglas belongs to one Grad
            builder.Entity<Oglas>()
                .HasOne(o => o.Grad)
                .WithMany() // Grad doesn't need a collection of Oglasi
                .HasForeignKey(o => o.Grad_Id)
                .OnDelete(DeleteBehavior.Restrict);

            // One Oglas can have many ImageUrls
            builder.Entity<Oglas>()
                .HasMany(o => o.ImageURLs)
                .WithOne(i => i.Oglas)
                .HasForeignKey(i => i.OglasId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Default Values and Constraints
            // Default value for Oglas.Cena
            builder.Entity<Oglas>()
                .Property(o => o.Cena)
                .HasDefaultValue(0);

            // Default value for Oglas.DatumPostavljanja (if you add this property)
            // builder.Entity<Oglas>()
            //     .Property(o => o.DatumPostavljanja)
            //     .HasDefaultValueSql("GETDATE()");
            #endregion

            #region Indexes
            // Index for Oglas.Naslov
            builder.Entity<Oglas>()
                .HasIndex(o => o.Naslov);

            // Index for Prodavac.Ime
            builder.Entity<Prodavac>()
                .HasIndex(p => p.ImeFirme);
            #endregion

            #region Seed Data
            // Seed roles
            var userRoleId = "97871e9b-0f3f-4589-9358-1adbc1c873f8";
            var sellerRoleId = "f9992759-0283-4d2a-bf63-14f8155ee258";
            var adminRoleId = "b24e986e-9b13-4355-9227-0f6e4d2b9cb2";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId,
                    Name = "User",
                    NormalizedName = "User".ToUpper()
                },
                new IdentityRole
                {
                    Id = sellerRoleId,
                    ConcurrencyStamp = sellerRoleId,
                    Name = "Seller",
                    NormalizedName = "Seller".ToUpper()
                },
                new IdentityRole
                {
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
            #endregion
        }
    }
}



























//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using OTSupply.API.Models.Domain;

//namespace OTSupply.API.Data
//{
//    public class OTSupplyDbContext : IdentityDbContext<Korisnik>
//    {

//        public OTSupplyDbContext(DbContextOptions dbContextOptions):base(dbContextOptions)
//        {

//        }

//        public DbSet<Grad> Gradovi { get; set; }

//        public DbSet<Kategorija> Kategorije { get; set; }

//        public DbSet<Ocene> Ocene { get; set; }

//        public DbSet<Oglas> Oglasi { get; set; }

//        public DbSet<ImageUrl> ImageUrls { get; set; }

//        public DbSet<Prodavac> Prodavci { get; set; }




//        protected override void OnModelCreating(ModelBuilder builder)
//        {
//            base.OnModelCreating(builder);




//            //Dodavanje rolova direktno u bazu
//            var userRoleId = "97871e9b-0f3f-4589-9358-1adbc1c873f8";
//            var sellerRoleId = "f9992759-0283-4d2a-bf63-14f8155ee258";
//            var adminRoleId = "b24e986e-9b13-4355-9227-0f6e4d2b9cb2";  

//            var roles = new List<IdentityRole>
//            {

//                new IdentityRole
//                {
//                    Id= userRoleId,
//                    ConcurrencyStamp=userRoleId,
//                    Name="User",
//                    NormalizedName="User".ToUpper()
//                },
//                new IdentityRole
//                {
//                    Id=sellerRoleId,
//                    ConcurrencyStamp=adminRoleId,
//                    Name="Seller",
//                    NormalizedName="Seller".ToUpper()

//                },
//                new IdentityRole
//                {
//                    Id=adminRoleId,
//                    ConcurrencyStamp=userRoleId,
//                    Name="Admin",
//                    NormalizedName="Admin".ToUpper()
//                }

//            };
//            builder.Entity<IdentityRole>().HasData(roles);

//        }
//    }
//}
