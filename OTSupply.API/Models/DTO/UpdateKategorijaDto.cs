using System.ComponentModel.DataAnnotations;

namespace OTSupply.API.Models.DTO
{
    public class UpdateKategorijaDto
    {
        [Required]
        public string Name { get; set; }
    }
}
