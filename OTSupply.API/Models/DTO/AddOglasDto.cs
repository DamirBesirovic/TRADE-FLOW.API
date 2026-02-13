namespace OTSupply.API.Models.DTO
{
    public class AddOglasDto
    {


        //public string Naslov { get; set; }
        //public string Opis { get; set; }
        //public string Materijal { get; set; }
        //public int Cena { get; set; }
        //public string Mesto { get; set; }

        //public List<string> ImageUrls { get; set; }

        //public Guid Kategorija_Id { get; set; }
        //public Guid Grad_Id { get; set; }
        //public Guid Prodavac_Id { get; set; }
       
            public string Naslov { get; set; }
            public string Opis { get; set; }
            public string Materijal { get; set; }
            public int Cena { get; set; }
            public string Mesto { get; set; }

            public List<string> ImageUrls { get; set; } = new();

            public Guid Kategorija_Id { get; set; }
            public Guid Grad_Id { get; set; }
           // public Guid Prodavac_Id { get; set; }
        

    }
}
