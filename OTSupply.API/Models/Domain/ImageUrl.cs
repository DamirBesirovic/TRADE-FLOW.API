namespace OTSupply.API.Models.Domain
{
    public class ImageUrl
    {
        public Guid Id { get; set; }
        public string Url { get; set; }

        // Foreign key for Oglas
        public Guid? OglasId { get; set; }

        // Navigation property
        public Oglas Oglas { get; set; } 
    }
}
