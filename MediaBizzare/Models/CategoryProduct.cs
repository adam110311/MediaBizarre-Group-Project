using System.ComponentModel.DataAnnotations;

namespace MediaBizzare.Models
{
    public class CategoryProduct
    {
        public Category? Category { get; set; }
        public Product? Product { get; set; }
    }
}
