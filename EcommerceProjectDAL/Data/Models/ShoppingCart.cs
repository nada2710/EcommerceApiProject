using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectDAL.Data.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<ShoppingCartItem> shoppingCartItems { get; set; } = new HashSet<ShoppingCartItem>();
    }
}
