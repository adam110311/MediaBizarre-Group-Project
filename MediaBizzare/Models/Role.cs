using System.ComponentModel.DataAnnotations;

namespace MediaBizzare.Models
{
    public class Role
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
