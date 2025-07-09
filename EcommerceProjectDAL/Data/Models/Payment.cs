using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectDAL.Data.Models
{
    public class Payment
    {
        //public int Id { get; set; }
        //public PaymentMethod PaymentMethod { get; set; }
        //public StatusOfPayment StatusOfPayment { get; set; }
        //[ForeignKey("Order")]
        //public int OrderId { get; set; }
        //public Order Order { get; set; }
        //public bool IsDeleted { get; set; }
        public int Id { get; set; }
        //public string PaymentIntentId { get; set; }
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(50)]
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Stripe;

        [MaxLength(100)]
        public string TransactionId { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        [MaxLength(500)]
        public string Notes { get; set; }
        public bool IsDeleted { get; set; }

        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

    }
    public enum PaymentMethod
    {
       // CreditCard=1,
       // PayPal=2,
       // cach=3,
        Stripe=4
    }
    public enum PaymentStatus
    {
        Pending=1,
        Completed=2,
        Failed=3
    }
}
