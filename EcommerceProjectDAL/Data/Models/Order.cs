using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectDAL.Data.Models
{
    public class Order
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [Required]
        public string ShippingAddress { get; set; }

        public Status Status { get; set; } = Status.Pending;

        public decimal TotalAmount => OrderItems.Sum(item => item.TotalPrice);

        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("Payment")]
        public int? PaymentId { get; set; }
        public Payment Payment { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();

    }
    public enum Status
    {
        Pending=1,
        Processing=2,
        Delivered=3,
        Cancelled=4
    }
}
