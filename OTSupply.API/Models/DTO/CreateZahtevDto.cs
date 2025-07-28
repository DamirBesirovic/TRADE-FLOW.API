namespace OTSupply.API.Models.DTO
{
    public class CreateZahtevDto
    {
        public Guid OglasId { get; set; }     
        public Guid GradId { get; set; }       
        public string Poruka { get; set; }     
        public string Telefon { get; set; }   
        public int Kolicina { get; set; }      
    }

}
