using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;
using Models.HelpClasses;

namespace Models.Operations
{
    public class PaymentOperations
    {
        public async Task<Payment> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PageViewDTO<Payment>> GetByUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Payment> AddAsync(Payment payment)
        {
            throw new NotImplementedException();
        }

        public async Task<Payment> UpdateAsync(Payment payment)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task YmNotifyAsync(YmNotification ymNotification)
        {
            throw new NotImplementedException();
        }

        



    }
}
