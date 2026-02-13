namespace OTSupply.API.Models.DTO
{
    public class UpdateOglasDto
    {
        public string Naslov { get; set; }
        public string Opis { get; set; }
        public string Materijal { get; set; }
        public int Cena { get; set; }
        public string Mesto { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();
        public Guid Kategorija_Id { get; set; }
        public Guid Grad_Id { get; set; }
    }

}
