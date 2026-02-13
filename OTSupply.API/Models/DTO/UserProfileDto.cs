namespace OTSupply.API.Models.DTO
{
    public class UserProfileDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRegistracije { get; set; }

        public string PhoneNumber {  get; set; }
        public bool isFirstLogin { get; set; }
        public List<string> Roles { get; set; }
    }

}
