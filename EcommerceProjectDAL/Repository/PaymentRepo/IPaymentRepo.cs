using EcommerceProjectDAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectDAL.Repository.PaymentRepo
{
    public  interface IPaymentRepo
    {
        Task<Payment> GetByIdAsync(int id);
       // Task<Order> GetOrderByIdAsync(int orderId);
        Task<Payment> GetByTransactionIdAsync(string transactionId);
        Task<IEnumerable<Payment>> GetByOrderIdAsync(int orderId);
        //Task<Payment> GetByPaymentIntentId(string paymentIntentId);
        Task AddAsync(Payment payment);
         Task Update(Payment payment);
        Task SaveChangesAsync();
    }
}
