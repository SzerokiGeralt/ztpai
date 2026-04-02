using System.ComponentModel.DataAnnotations;

namespace ztpai.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Range(0.0, double.MaxValue)]
        public decimal Price { get; set; }

    }
}
