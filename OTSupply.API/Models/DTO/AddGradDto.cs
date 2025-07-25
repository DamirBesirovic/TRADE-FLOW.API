using System.ComponentModel.DataAnnotations;

namespace OTSupply.API.Models.DTO
{
    public class AddGradDto
    {
        [Required]
        public string Name { get; set; }
    }
}
