using System.ComponentModel.DataAnnotations.Schema;

namespace ePizzaHub.Entities
{
    public class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }
        public string Id { get; set; }
        public string PaymentId { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Locality { get; set; }
        public string PhoneNumber { get; set; }

        public DateTime CreatedDate { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
