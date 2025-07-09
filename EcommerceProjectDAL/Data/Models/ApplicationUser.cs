using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectDAL.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        
        public UserRole UserRole { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsEmailVerified { get; set; } = false;
        public string VerificationCode { get; set; }
        public DateTime? VerificationCodeExpiration { get; set; }
    }
    public class Admin :ApplicationUser
    {
    }
    public class Customer : ApplicationUser
    {
        public ICollection<Order> orders { get; set; } = new HashSet<Order>();
        public ICollection<Review> reviews { get; set; } = new HashSet<Review>();
        [ForeignKey("ShoppingCart")]
        public int? ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
       
        //public ICollection<ShoppingCart> ShoppingCarts { get; set; } = new HashSet<ShoppingCart>();
    }
    public enum UserRole
    {
        Admin=1,
        Customer=2
    }
}
   

