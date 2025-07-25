using System.ComponentModel.DataAnnotations;

namespace OTSupply.API.Models.DTO
{
    public class RegisterSellerRequestDto
    {
       
        [Required]
        [MaxLength(300)]
        public string Bio { get; set; }

        [Required]
        [MaxLength(50)]
        public string ImeFirme { get; set; }

        public string? Mesto { get; set; }

        public string? PhoneNumber { get; set; }

        public string? PfpUrl { get; set; }
    }
}
