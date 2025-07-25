namespace OTSupply.API.Models.DTO
{
    public class GetSellerResponseDto
    {
        public string Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime DatumRegistracije { get; set; }
        public List<string> Roles { get; set; }

        // Prodavac specifično
        public string ImeFirme { get; set; }
        public string Bio { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PfpUrl { get; set; }
        public double Ocena { get; set; }
        public bool IsVerified { get; set; }
    }
}
