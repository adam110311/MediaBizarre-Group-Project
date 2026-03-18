namespace MediaBizzare.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
    }
}
