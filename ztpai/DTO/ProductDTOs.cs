using System.ComponentModel.DataAnnotations;

namespace ztpai.DTO
{
    public class ProductRequestDTO
    {
        [Required(AllowEmptyStrings =false, ErrorMessage ="Product name is required")]
        [MinLength(3, ErrorMessage = "Name needs to be at least 3 characters long")]
        public string? Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Product description is required")]
        [MinLength(3, ErrorMessage = "Description needs to be at least 3 characters long")]
        public string? Description { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Price needs to be equal or grater than 0")]
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public uint InStock { get; set; }
    }

    public class ProductResponseDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public uint InStock { get; set; }
    }
}
