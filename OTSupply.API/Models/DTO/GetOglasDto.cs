using OTSupply.API.Models.Domain;

namespace OTSupply.API.Models.DTO
{
    public class GetOglasDto
    {
        public Guid Id { get; set; }
        public string Naslov { get; set; }
        public string Opis { get; set; }
        public string Materijal { get; set; }
        public int Cena { get; set; }
        public string Mesto { get; set; }
        public List<string> ImageUrls { get; set; }


        public string Kategorija { get; set; }
        public string Grad { get; set; }
        public string Prodavac { get; set; } //ime od firme prodavca
        public Guid ProdavacId { get; set; } 
    }

}
