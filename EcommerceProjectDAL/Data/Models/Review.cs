﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectDAL.Data.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Range(1,5,ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }
        public string Comment { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
