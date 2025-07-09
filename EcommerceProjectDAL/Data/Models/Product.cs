﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectDAL.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<ShoppingCartItem> shoppingCartItems { get; set; } = new HashSet<ShoppingCartItem>();
        public ICollection<Review> reviews { get; set; } = new HashSet<Review>();
        public ICollection<OrderItem> orderItems { get; set; } = new HashSet<OrderItem>();



    }
}
