﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using AutoMapper;
using Camps.Tools;
using Models.Dtos;
using Models.Entities;
using Models.Tools;

namespace Models.Operations
{
    public class OrderOperations : IDisposable
    {
        private LrdrContext _context;
        private CommentOperations _commentOperations;

        public OrderOperations(LrdrContext context, CommentOperations commentOperations)
        {
            _context = context;
            _commentOperations = commentOperations;
            _commentOperations.OnModifyEventHandler += OnModifyComment;
            _commentOperations.OnDeleteEventHandler += OnDeleteComment;
        }

        /// <summary>
        /// Получает закакз по GUID
        /// </summary>
        public async Task<OrderDto> GetAsync(string code)
        {
            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Code == code);
                var dto =  Mapper.Map<OrderDto>(order);
                dto.IsReadedByContractor = await _context.Comments.Where(c => 
                    // комменты заказа, не созданные контрактором
                    (c.OrderId == order.Id) && (c.UserId != order.ContractorUserId))
                    // прочитаны
                    .AllAsync(c => (c.IsReaded));

                dto.IsReadedByCustomer = await _context.Comments.Where(c =>
                    // комменты заказа, не созданные клиентом
                    (c.OrderId == order.Id) && (c.UserId != order.CustomerUserId))
                    // прочитаны
                    .AllAsync(c => (c.IsReaded));

                return dto;
            }
            catch (Exception e)
            {
                ErrorLogger.Log("CANNOT GET ORDER", e);
                throw;
            }
        }

        public async Task<OrderDto> GetAsync(int id)
        {
            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
                return Mapper.Map<OrderDto>(order);
            }
            catch (Exception e)
            {
                ErrorLogger.Log("CANNOT GET ORDER", e);
                throw;
            }
        }

        public async Task<PageViewDTO<Order>> SearchAsync(string word="", int? customerUserId=null,
                                                          int? ownerUserId = null,
                                                          bool? isPaid = null,
                                                          OrderSorting sortby=OrderSorting.Updated, 
                                                          int page=1)
        {
            try
            {
                var query = _context.Orders.Include(o => o.CustomerUser)
                                           .Include(o => o.ContractorUser)
                                           .AsQueryable();

                if (!String.IsNullOrEmpty(word))
                {
                    query = query.Where(o =>  (o.Name.ToLower().Contains(word.ToLower()))
                                            || (o.Code.ToLower().Contains(word.ToLower()))
                                            || (o.CustomerUser.Email.ToLower().Contains(word.ToLower()))
                                            || (o.CustomerUser.Name.ToLower().Contains(word.ToLower()))
                                            || (o.ContractorUser.Email.ToLower().Contains(word.ToLower()))
                                            || (o.ContractorUser.Name.ToLower().Contains(word.ToLower()))
                                             );
                }

                if (customerUserId.HasValue)
                {
                    query = query.Where(o => o.CustomerUserId == customerUserId.Value);
                }

                if (ownerUserId.HasValue)
                {
                    query = query.Where(o => o.ContractorUserId == ownerUserId.Value);
                }

                if (isPaid == true)
                {
                    query = query.Where(o => (o.Status == OrderStatus.Payed)
                                             || (o.Status == OrderStatus.Done));
                }
                if (isPaid == false)
                {
                    query = query.Where(o => (o.Status != OrderStatus.Payed)
                                             && (o.Status != OrderStatus.Done));
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
                        query = query.OrderBy(o => o.ContractorUser.Name);
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
                    SortBy = sortby.ToString(),
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

        public async Task<OrderDto> UpdateAsync(Order order)
        {
            try
            {
                var oldOrder = _context.Orders.First(o => o.Id == order.Id);
                //oldOrder.Code = order.Code;
                oldOrder.Created = order.Created;
                oldOrder.CustomerUserId = order.CustomerUserId;
                oldOrder.Deadline = order.Deadline;
                oldOrder.ContractorUserId = order.ContractorUserId;
                oldOrder.Updated = DateTimeOffset.Now;
                oldOrder.DeliveryAddress = order.DeliveryAddress;
                oldOrder.IsDeleted = order.IsDeleted;
                oldOrder.Name = order.Name;
                //oldOrder.Price = order.Price;
                //oldOrder.Status = order.Status;
                oldOrder.ShowPayment = order.ShowPayment;
                await _context.SaveChangesAsync();

                return await GetAsync(order.Id);
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
                order.Updated = DateTimeOffset.Now;
                order.Created = DateTimeOffset.Now;
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
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ErrorLogger.Log("CANNOT DELETE ORDER", e);
                throw;
            }
        }

        public async Task<OrderDto> ChangePriceAsync(int orderId, Decimal newPrice)
        {
            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId); 
                order.Price = newPrice;
                order.Updated = DateTimeOffset.Now;
                await _context.SaveChangesAsync();
                return await GetAsync(orderId);

            }
            catch (Exception e)
            {
                ErrorLogger.Log("CANNOT CHANGE ORDER PRICE", e);
                throw;
            }
        }

        public async Task<OrderDto> ChangeStatusAsync(int orderId, OrderStatus newStatus)
        {
            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
                order.Status = newStatus;
                order.Updated = DateTimeOffset.Now;
                await _context.SaveChangesAsync();
                return await GetAsync(orderId);
            }
            catch (Exception e)
            {
                ErrorLogger.Log("CANNOT CHANGE ORDER STATUS", e);
                throw;
            }
        }

        /// <summary>
        /// Проверка прав на редактирование заказа. Заказ можно редактировать владельцу и админу
        /// </summary>
        public static async Task<bool> CheckRights(int orderId, string userEmail)
        {
            using (var context = new LrdrContext())
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == userEmail.ToLower());
                if (user == null) return false;

                var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
                if (order == null)
                {
                    throw new NotFoundException();
                }

                if ((order.ContractorUserId == user.Id) || (user.Role == Role.PortalAdmin)) return true;
                return false;
            }
            
        }

        public async Task UpdateUpdatedDate(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId); ;
            if (order == null)
            {
                throw new NotFoundException();
            }
            order.Updated = DateTimeOffset.Now;
            await _context.SaveChangesAsync();
        }

        private async Task OnModifyComment(object sender, Comment comment)
        {
            await UpdateUpdatedDate(comment.OrderId);
        }

        private async Task OnDeleteComment(object sender, Comment comment)
        {
            await UpdateUpdatedDate(comment.OrderId);
        }

        public void Dispose()
        {
            _context?.Dispose();
            _commentOperations.OnModifyEventHandler -= OnModifyComment;
            _commentOperations.OnDeleteEventHandler -= OnDeleteComment;
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
