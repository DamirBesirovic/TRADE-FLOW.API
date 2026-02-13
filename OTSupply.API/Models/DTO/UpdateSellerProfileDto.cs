namespace OTSupply.API.Models.DTO
{
    public class UpdateSellerProfileDto : UpdateUserProfileDto
    {
        public string ImeFirme { get; set; }
        public string? Bio { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PfpUrl { get; set; }
    }
}
