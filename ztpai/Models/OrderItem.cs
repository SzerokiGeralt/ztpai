using System.ComponentModel.DataAnnotations;

namespace ztpai.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; } = string.Empty;
        [Range(0.0, (double)decimal.MaxValue)]
        public decimal TotalPrice { get; set; }
        public uint Quantity { get; set; }
    }
}
