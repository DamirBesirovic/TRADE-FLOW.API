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
    public Guid KategorijaId { get; set; }   // dodatak
    public string Grad { get; set; }
    public Guid GradId { get; set; }         // dodato
    public string Prodavac { get; set; } //ime od firme prodavca
    public Guid ProdavacId { get; set; }
}
