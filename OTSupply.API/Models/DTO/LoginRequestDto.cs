using System.ComponentModel.DataAnnotations;

namespace OTSupply.API.Models.DTO
{
    public class LoginRequestDto
    {

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
