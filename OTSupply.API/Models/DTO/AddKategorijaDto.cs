using System.ComponentModel.DataAnnotations;

namespace OTSupply.API.Models.DTO
{
    public class AddKategorijaDto
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
