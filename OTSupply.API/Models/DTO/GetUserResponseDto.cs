namespace OTSupply.API.Models.DTO
{
    public class GetUserResponseDto
    {
        public string Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime DatumRegistracije { get; set; }
        public List<string> Roles { get; set; }
    }
}
