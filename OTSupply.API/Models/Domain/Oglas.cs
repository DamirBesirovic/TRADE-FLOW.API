namespace OTSupply.API.Models.Domain
{
    public class Oglas
    {
        public Guid Id { get; set; }
        public string Naslov { get; set; }

        public string Opis { get; set; }

        public string Materijal { get; set; }

        public int Cena { get; set; }

        public string Mesto { get; set; }

        // strani kljucevi
        public Guid Kategorija_Id { get; set; }
        public Guid Grad_Id { get; set; }


        //navigacioni propertiji

        public Kategorija Kategorija { get; set; }
        public Grad Grad { get; set; }
    }
}
