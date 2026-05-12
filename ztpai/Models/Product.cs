using System.ComponentModel.DataAnnotations;

namespace ztpai.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Range(0.0, double.MaxValue)]
        public decimal Price { get; set; }
        public uint InStock { get; set; } = 1;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
