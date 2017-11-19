using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using Models.Entities;

namespace Models
{
    public static class Seeder
    {
        public static void Seed(LrdrContext context)
        {


            #region USERS

            if (!context.Users.Any())
            {
                context.Users.AddOrUpdate(new User()
                {
                    AuthToken = "HREN",
                    Email = "hren@mhbb.ru",
                    Role = Role.PortalAdmin,
                    DateRegistered = DateTime.Now,
                    Name = "Вова Хрен",
                    Phone = "+79062990099"

                });

                context.Users.AddOrUpdate(new User()
                {
                    AuthToken = "ABRAKADABRA",
                    Email = "var@33kita.ru",
                    Role = Role.PortalAdmin,
                    DateRegistered = DateTime.Now,
                    Name = "Вова Петросян",
                    Phone = "+79062990044"

                });

                context.Users.AddOrUpdate(new User()
                {
                    AuthToken = "test",
                    Email = "test@mhbb.ru",
                    Role = Role.PortalAdmin,
                    DateRegistered = DateTime.Now,
                    Phone = "+79062990077",
                    Name = "Морковка"
                });

                context.Users.AddOrUpdate(new User()
                {
                    AuthToken = "registered_user_token1",
                    Email = "registered_user1@mhbb.ru",
                    Role = Role.RegisteredUser,
                    DateRegistered = DateTime.Now,
                    Name = "Зарегистрированный пользователь1"
                });

                context.Users.AddOrUpdate(new User()
                {
                    AuthToken = "registered_user_token2",
                    Email = "registered_user2@mhbb.ru",
                    Role = Role.RegisteredUser,
                    DateRegistered = DateTime.Now,
                    Name = "Зарегистрированный пользователь2"
                });

                context.Users.AddOrUpdate(new User()
                {
                    AuthToken = "registered_user_token3",
                    Email = "registered_user3@mhbb.ru",
                    Role = Role.RegisteredUser,
                    DateRegistered = DateTime.Now,
                    Name = "Зарегистрированный пользователь3"
                });
            }
            

            context.SaveChanges();

            #endregion

            #region ORDERS

            if (!context.Orders.Any())
            {
                context.Orders.Add(new Order
                {
                    Name = "Заказ для Михаила петровича",
                    Created = DateTimeOffset.Now,
                    CustomerUserId = context.Users.Take(3).ToList().Last().Id,
                    PostedByUserId = context.Users.First().Id,
                    Deadline = DateTimeOffset.Now.AddDays(5),
                    DeliveryAddress = "Череповец",
                    IsDeleted = false,
                    ContractorUserId = context.Users.First().Id,
                    Price = 50,
                    ShowPayment = true,
                    Status = OrderStatus.Created,
                    Code = Guid.NewGuid().ToString()
                });

                context.Orders.Add(new Order
                {
                    Name = "Заказ для Васечки",
                    Created = DateTimeOffset.Now,
                    CustomerUserId = context.Users.Take(3).ToList().Last().Id,
                    PostedByUserId = context.Users.First().Id,
                    Deadline = DateTimeOffset.Now.AddDays(5),
                    DeliveryAddress = "Череповец",
                    IsDeleted = false,
                    ContractorUserId = context.Users.First().Id,
                    Price = 500,
                    ShowPayment = false,
                    Status = OrderStatus.Created,
                    Code = Guid.NewGuid().ToString()
                });

                context.SaveChanges();
            }
            #endregion

            #region PAYMENTS

            if (!context.Payments.Any())
            {
                var user = context.Users.First();
                context.Payments.Add(new Payment
                {
                    Description = "Оплатил через Сберкарту",
                    ExternalId = null,
                    ExternalUserId = null,
                    IsDeleted = false,
                    OrderId = context.Orders.First().Id,
                    Sum = 20,
                    Time = DateTimeOffset.Now,
                    UserId = user.Id
                });

                context.Payments.Add(new Payment
                {
                    Description = "Оплатил через Сберкарту",
                    ExternalId = null,
                    ExternalUserId = null,
                    IsDeleted = false,
                    OrderId = context.Orders.First().Id,
                    Sum = 40,
                    Time = DateTimeOffset.Now,
                    UserId = user.Id
                });

                context.Payments.Add(new Payment
                {
                    Description = "Яндекс деньги. Детали первого платежа...",
                    ExternalId = "ym-9hends933nnsss",
                    ExternalUserId = "rerere@rerere",
                    IsDeleted = false,
                    Sum = 450,
                    Time = DateTimeOffset.Now,
                    PaymentType = PaymentType.ForSystemUsing,
                    UserId = user.Id
                });

                context.Payments.Add(new Payment
                {
                    Description = "Яндекс деньги. Детали второго платежа...",
                    ExternalId = "ym-8bwsbs8484bnmssj33",
                    ExternalUserId = "rerere@rerere",
                    IsDeleted = false,
                    Sum = 500,
                    Time = DateTimeOffset.Now,
                    PaymentType = PaymentType.ForSystemUsing,
                    UserId = user.Id
                });


                context.SaveChanges();
            }

            #endregion

            #region COMMENTS

            if (!context.Comments.Any())
            {
                context.Comments.Add(new Comment
                {
                    OrderId = context.Orders.First().Id,
                    Time = DateTimeOffset.Now,
                    Text = "Тут текст комментария",
                    UserId = context.Users.First().Id
                });
                context.Comments.Add(new Comment
                {
                    OrderId = context.Orders.First().Id,
                    Time = DateTimeOffset.Now,
                    Text = "Тут текст второго комментария",
                    UserId = context.Users.First().Id
                });

                context.SaveChanges();
            }

            


            #endregion

            #region FILES

            if (!context.Files.Any())
            {
                context.Files.Add(new File
                {
                    LinkedObjectType = LinkedObjectType.User,
                    Extension = "jpg",
                    LinkedObjectId = context.Users.First().Id,
                    Name = "avatarka",
                    Code = Guid.NewGuid().ToString()
                });

                context.Files.Add(new File
                {
                    LinkedObjectType = LinkedObjectType.Comment,
                    Extension = "jpg",
                    LinkedObjectId = context.Comments.First().Id,
                    Name = "фоточка к комментарию",
                    Code = Guid.NewGuid().ToString()
                });

                context.Files.Add(new File
                {
                    LinkedObjectType = LinkedObjectType.Comment,
                    Extension = "docx",
                    LinkedObjectId = context.Comments.First().Id,
                    Name = "документик к комментарию",
                    Code = Guid.NewGuid().ToString()
                });

                context.SaveChanges();
            }

            

            #endregion




        }
    }
}
