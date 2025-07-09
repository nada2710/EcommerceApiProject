using EcommerceProjectDAL.Data.DbHelper;
using EcommerceProjectDAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectDAL.Repository.PaymentRepo
{
    public class PaymentRepo : IPaymentRepo
    {
        private readonly EcommerceProjectContext _ecommerceProject;
        public PaymentRepo(EcommerceProjectContext ecommerceProject)
        {
            _ecommerceProject=ecommerceProject;
        }
       
        public async Task<Payment> GetByIdAsync(int id)
        {
            return await _ecommerceProject.payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Payment> GetByTransactionIdAsync(string transactionId)
        {
            return await _ecommerceProject.payments
                .FirstOrDefaultAsync(p => p.TransactionId == transactionId);
        }
        public async Task<IEnumerable<Payment>> GetByOrderIdAsync(int orderId)
        {
            return await _ecommerceProject.payments
                .Where(p => p.OrderId == orderId)
                .ToListAsync();
        }

        public async Task AddAsync(Payment payment)
        {
            await _ecommerceProject.payments.AddAsync(payment);
        }

        public async Task Update(Payment payment)
        {
            
        }

        public async Task SaveChangesAsync()
        {
             await _ecommerceProject.SaveChangesAsync();
        }

       
    }
}
