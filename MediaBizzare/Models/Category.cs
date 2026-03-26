using System.ComponentModel.DataAnnotations;

namespace MediaBizzare.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Slug { get; set; }
    }
}
