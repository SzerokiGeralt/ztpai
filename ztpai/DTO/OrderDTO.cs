using System.ComponentModel.DataAnnotations;
using ztpai.Models;

namespace ztpai.DTO
{
    public class CreateOrderRequestDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        public string? EmailAddress { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Delivery address is required")]
        public string? DeliveryAddress { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Order must contain at least one item")]
        public List<OrderItemRequestDTO> Items { get; set; } = new();
    }

    public class OrderItemRequestDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than 0")]
        public int ProductId { get; set; }

        [Range(1, uint.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public uint Quantity { get; set; }
    }

    public class CreateOrderResultDTO
    {
        public bool Success { get; set; }
        public int? OrderId { get; set; }
        public string? Error { get; set; }
        public OrderStatus? Status { get; set; }
    }
}
