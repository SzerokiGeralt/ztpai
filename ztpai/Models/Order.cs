using System.ComponentModel.DataAnnotations;

namespace ztpai.Models
{
    public enum OrderStatus
    {
        Ordered,
        Confirmed,
        Prepared,
        Sent,
        Recieved,
        Complete,
        Cancelled
    }

    public class Order
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        [Required]
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        [Range(0.0, (double)decimal.MaxValue)]
        public decimal TotalPrice { get; set; }
        public OrderStatus Status {  get; set; } = OrderStatus.Ordered;
    }
}
