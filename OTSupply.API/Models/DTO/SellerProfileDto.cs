namespace OTSupply.API.Models.DTO
{
    public class SellerProfileDto : UserProfileDto
    {
        public string ImeFirme { get; set; }
        public string Bio { get; set; }
        public string PfpUrl { get; set; }
        public double Ocena { get; set; }
        public bool IsVerified { get; set; }
    }
}
