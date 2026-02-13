using System.ComponentModel.DataAnnotations;

namespace OTSupply.API.Models.DTO
{
    public class UpdateGradDto
    {
        [Required]
        public string Name { get; set; }
    }
}
