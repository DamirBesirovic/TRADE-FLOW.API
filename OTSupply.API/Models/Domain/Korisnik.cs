using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OTSupply.API.Models.Domain
{
    public class Korisnik : IdentityUser
    {

        [Required]
        [StringLength(50)]
        public string Ime { get; set; }
        [Required]
        [StringLength(50)]
        public string Prezime { get; set; }
        public bool isFirstLogin { get; set; } = true;
        public DateTime DatumRegistracije { get; set; }
        public bool isDeleted { get; set; }

        //Navigation properties
        public ICollection<Ocene> Ocenes { get; set; } = new List<Ocene>();
    }
}
