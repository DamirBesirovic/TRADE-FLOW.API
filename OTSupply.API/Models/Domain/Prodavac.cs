using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OTSupply.API.Models.Domain
{
    public class Prodavac
    {

       
        public Guid Id { get; set; }
        [Required]
        public string Bio { get; set; }
        [Required]
        public string ImeFirme { get; set; }
        public double Ocena { get; set; }
        public string? PfpUrl { get; set; }
        public bool IsVerified { get; set; } = false;
        public string? PhoneNumber { get; set; }

        //Foreign keys
        [ForeignKey("Korisnik")]
        public string Id_Korisnik { get; set; }

        //Navigation properties 
        public Korisnik Korisnik { get; set; }
        public ICollection<Oglas> Oglasi { get; set; } = new List<Oglas>();
        public ICollection<Ocene> Ocenes { get; set; } = new List<Ocene>();
    }
}
