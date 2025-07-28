namespace OTSupply.API.Models.Domain
{
    public class Zahtev
    {
        public Guid Id { get; set; }
        public Guid Oglas_Id { get; set; }

        public Guid Grad_Id { get; set; }

        public Guid Kupac_Id { get; set; }

        public int Kolicina { get; set; }

        public string Poruka    { get; set; }
        public string Telefon   { get; set; }
        public DateTime PoslatoVreme    { get; set; } = DateTime.UtcNow;

        public bool Procitano   { get; set; }= false;

        public string VlasnikOglasa_KorisnikId { get; set; }
    }
}
