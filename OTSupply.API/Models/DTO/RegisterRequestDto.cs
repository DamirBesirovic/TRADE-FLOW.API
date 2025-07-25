using System.ComponentModel.DataAnnotations;

namespace OTSupply.API.Models.DTO
{
    public class RegisterRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)] 
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }

       
    }
}
