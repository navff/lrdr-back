using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Camps.Tools;
using Models.Entities;
using Models.HelpClasses;
using Models.Tools;
using System.Data.Entity;
using API.Operations;

namespace Models.Operations
{

    /// <summary>
    /// Операции с оплатами
    /// </summary>
    public class PaymentOperations
    {
        private LrdrContext _context;
        private OrderOperations _orderOperations;
        private UserOperations _userOperations;

        public PaymentOperations(LrdrContext context, 
                                 OrderOperations orderOperations,
                                 UserOperations userOperations)
        {
            _context = context;
            _orderOperations = orderOperations;
            _userOperations = userOperations;
        }

        /// <summary>
        /// Получить платёж по Id
        /// </summary>
        public async Task<Payment> GetAsync(int id)
        {
            try
            {
                var result =  _context.Payments.Include(p => p.Order).FirstOrDefault(p => p.Id == id);
                return result;
            }
            catch (Exception e)
            {
                ErrorLogger.Log("CANNOT GET PAYMENT", e);
                throw;
            }
        }

        /// <summary>
        /// Ищет платежи
        /// </summary>
        /// <param name="searchType">Тип поиска: по какому критерию ищем</param>
        /// <param name="id">Id пользователя или Id счёта, по которому ищем</param>
        /// <param name="isDeleted">Показать только удалённые</param>
        /// <param name="page">Страница пагинации</param>
        public async Task<PageViewDTO<Payment>> Search(PaymentSearchType searchType = PaymentSearchType.All,
                                                       int? id = null,
                                                       bool isDeleted = false,
                                                       int page=1)
        {
            try
            {
                var query = _context.Payments.Include(p => p.Order).Where(p => p.IsDeleted == isDeleted).AsQueryable();

                switch (searchType)
                {
                    case PaymentSearchType.CustomerUser:
                        query = query.Where(p => p.Order.CustomerUserId == id);
                        break;
                    case PaymentSearchType.OwnerUser:
                        query = query.Where(p => p.Order.OwnerUserId == id);
                        break;
                    case PaymentSearchType.SystemUser:
                        query = query.Where(p => p.UserId == id);
                        break;
                    case PaymentSearchType.Order:
                        query = query.Where(p => p.OrderId == id);
                        break;
                    case PaymentSearchType.All:
                        break;
                }

                query = query.OrderByDescending(p => p.Time);
                var total = query.Count();
                var result = new PageViewDTO<Payment>
                {
                    Content = await query.Skip(ModelsSettings.PAGE_SIZE * (page - 1))
                                   .Take(ModelsSettings.PAGE_SIZE)
                                   .ToListAsync(),
                    PageNumber = page,
                    SortBy = "Time",
                    Total = total,
                    TotalPages = (int)Math.Ceiling((double)total / (double)ModelsSettings.PAGE_SIZE)
                };
                return result;
            }
            catch (Exception e)
            {
                ErrorLogger.Log("CANNOT GET PAYMENTS BY CUSTOMER_USER", e);
                throw;
            }
        }

        


        public async Task<Payment> AddAsync(Payment payment)
        {
            try
            {
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
                if (payment.OrderId.HasValue)
                {
                    await _orderOperations.UpdateUpdatedDate(payment.OrderId.Value);
                }
                

                return await GetAsync(payment.Id);
            }
            catch (Exception e)
            {
                ErrorLogger.Log("CANNOT ADD PAYMENT", e);
                throw;
            }
        }

        public async Task<Payment> UpdateAsync(Payment payment)
        {
            try
            {
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
                if (payment.OrderId.HasValue)
                {
                    await _orderOperations.UpdateUpdatedDate(payment.OrderId.Value);
                }

                return await GetAsync(payment.Id);
            }
            catch (Exception e)
            {
                ErrorLogger.Log("CANNOT UPDATE PAYMENT", e);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var payment = _context.Payments.FirstOrDefault(p => p.Id == id);
                if (payment == null) throw new NotFoundException();

                if (payment.OrderId.HasValue)
                {
                    await _orderOperations.UpdateUpdatedDate(payment.OrderId.Value);
                }
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ErrorLogger.Log("CANNOT DELETE PAYMENT", e);
                throw;
            }
        }

        public async Task YmNotifyAsync(YmNotification ymNotification)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                ErrorLogger.Log("CANNOT SAVE YM_PAYMENT_NOTIFICATION", e);
                throw;
            }
        }

        public async Task<bool> CheckRights(int paymentId, string userEmail)
        {
            var payment = await GetAsync(paymentId);
            var user = await _userOperations.GetAsync(userEmail);

            if (payment.Order != null)
            {
                if (payment.Order.CustomerUserId == user.Id) return true;
                if (payment.Order.OwnerUserId == user.Id) return true;
            }
            return false;
        }

    }

    public enum PaymentSearchType
    {
        SystemUser = 0,
        OwnerUser = 1,
        CustomerUser = 2,
        Order = 3,
        All = 4,
    }
}
