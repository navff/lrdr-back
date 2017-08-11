using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Camps.Tools;
using Models.Entities;
using Models.Tools;

namespace Models.Operations
{
    public class OrderOperations
    {
        private LrdrContext _context;

        public OrderOperations(LrdrContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает закакз по GUID
        /// </summary>
        public async Task<Order> GetAsync(string code)
        {
            try
            {
                return await _context.Orders.FirstOrDefaultAsync(o => o.Code == code);
            }
            catch (Exception e)
            {
                ErrorLogger.Log("CANNOT GET ORDER", e);
                throw;
            }
        }

        public async Task<Order> GetAsync(int id)
        {
            try
            {
                return await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            }
            catch (Exception e)
            {
                ErrorLogger.Log("CANNOT GET ORDER", e);
                throw;
            }
        }

        public async Task<PageViewDTO<Order>> SearchAsync(string word="", int? userId=null, 
                                                          OrderSorting sortby=OrderSorting.Updated, 
                                                          int page=1)
        {
            try
            {
                var query = _context.Orders.Include(o => o.CustomerUser)
                                           .Include(o => o.OwnerUser)
                                           .AsQueryable();

                if (!String.IsNullOrEmpty(word))
                {
                    query = query.Where(o =>  (o.Name.ToLower().Contains(word.ToLower()))
                                            || (o.Code.ToLower().Contains(word.ToLower()))
                                            || (o.CustomerUser.Email.ToLower().Contains(word.ToLower()))
                                            || (o.CustomerUser.Name.ToLower().Contains(word.ToLower()))
                                             );
                }

                if (userId.HasValue)
                {
                    query = query.Where(o => (o.CustomerUserId == userId.Value)
                                          || (o.OwnerUserId == userId.Value));
                }

                switch (sortby)
                {
                    case OrderSorting.Updated:
                        query = query.OrderByDescending(o => o.Updated);
                        break;
                    case OrderSorting.Created:
                        query = query.OrderByDescending(o => o.Created);
                        break;
                    case OrderSorting.Customer:
                        query = query.OrderBy(o => o.CustomerUser.Name);
                        break;
                    case OrderSorting.Deadline:
                        query = query.OrderByDescending(o => o.Deadline);
                        break;
                    case OrderSorting.Owner:
                        query = query.OrderBy(o => o.OwnerUser.Name);
                        break;
                    default:
                        query = query.OrderBy(o => o.Updated);
                        break;
                }

                var total = query.Count();
                var result=  await query.Skip(ModelsSettings.PAGE_SIZE * (page - 1))
                    .Take(ModelsSettings.PAGE_SIZE)
                    .ToListAsync();
                return new PageViewDTO<Order>
                {
                    Content = result,
                    PageNumber = page,
                    SortBy = sortby,
                    Total = total,
                    TotalPages = (int)Math.Ceiling((double)total / (double)ModelsSettings.PAGE_SIZE)
                };

            }
            catch (Exception e)
            {
                ErrorLogger.Log("CANNOT SEARCH ORDERS", e);
                throw;
            }
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            try
            {
                var oldOrder = await GetAsync(order.Id);
                oldOrder.Code = order.Code;
                oldOrder.Created = order.Created;
                oldOrder.CustomerUserId = order.CustomerUserId;
                oldOrder.Deadline = order.Deadline;
                oldOrder.OwnerUserId = order.OwnerUserId;
                oldOrder.Updated = order.Updated;
                oldOrder.DeliveryAddress = order.DeliveryAddress;
                oldOrder.IsDeleted = order.IsDeleted;
                oldOrder.Name = order.Name;
                oldOrder.Price = order.Price;
                oldOrder.Status = order.Status;
                oldOrder.ShowPayment = order.ShowPayment;

                await _context.SaveChangesAsync();
                return oldOrder;
            }
            catch (Exception e)
            {
                ErrorLogger.Log("CANNOT UPDATE ORDER", e);
                throw;
            }
        }

        public async Task<Order> AddAsync(Order order)
        {
            try
            {
                order.Code = Guid.NewGuid().ToString();
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                return order;
            }
            catch (Exception e)
            {
                ErrorLogger.Log("CANNOT ADD ORDER", e);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var order = await GetAsync(id);
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ErrorLogger.Log("CANNOT DELETE ORDER", e);
                throw;
            }
        }

        public async Task<Order> ChangePriceAsync(int orderId, Decimal newPrice)
        {
            try
            {
                var order = await GetAsync(orderId);
                order.Price = newPrice;
                await _context.SaveChangesAsync();
                return order;

            }
            catch (Exception e)
            {
                ErrorLogger.Log("CANNOT CHANGE ORDER PRICE", e);
                throw;
            }
        }

        public async Task<Order> ChangeStatusAsync(int orderId, OrderStatus newStatus)
        {
            try
            {
                var order = await GetAsync(orderId);
                order.Status = newStatus;
                await _context.SaveChangesAsync();
                return order;
            }
            catch (Exception e)
            {
                ErrorLogger.Log("CANNOT CHANGE ORDER STATUS", e);
                throw;
            }
        }

    }

    public enum OrderSorting
    {
        Updated = 1,
        Created = 2,
        Deadline = 3,
        Owner = 4,
        Customer = 5
    }
}
