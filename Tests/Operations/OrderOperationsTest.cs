using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API;
using API.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Models.Entities;
using Models.Operations;
using Ninject;

namespace Tests.Operations
{
    [TestClass]
    public class OrderOperationsTest: BaseTest
    {
        private OrderOperations _orderOperations;

        public OrderOperationsTest()
        {
            var kernel = new StandardKernel();
            new NinjectDependencyResolver(kernel);
            _orderOperations = kernel.Get<OrderOperations>();
            MapperMappings.Map();
            
        }

        [TestMethod]
        public void GetById_Ok_Test()
        {
            var order = _context.Orders.First();
            var result = _orderOperations.GetAsync(order.Id).Result;
            Assert.AreEqual(order.Id, result.Id);
        }

        [TestMethod]
        public void GetByCode_Ok_Test()
        {
            var order = _context.Orders.First();
            var result = _orderOperations.GetAsync(order.Code).Result;
            Assert.AreEqual(order.Code, result.Code);
        }

        [TestMethod]
        public void Search_Ok_Test()
        {
            var order = _context.Orders.First();
            var result = _orderOperations.SearchAsync(order.ContractorUserId, order.Name.Substring(2)).Result;
            Assert.IsTrue(result.Content.Any());
        }

        [TestMethod]
        public void Search_NoResults_Test()
        {
            var order = _context.Orders.First();
            var result = _orderOperations.SearchAsync(order.ContractorUserId, "94nb8ends934n3jwndskjh348").Result;
            Assert.IsFalse(result.Content.Any());
        }

        [TestMethod]
        public void Update_Ok_Test()
        {
            var rndString = Guid.NewGuid().ToString();
            var order = _context.Orders.First();
            order.Name = rndString;
            var result = _orderOperations.UpdateAsync(order).Result;

            Assert.AreEqual(rndString, result.Name);
        }

        [TestMethod]
        public void Add_Ok_Test()
        {
            var rndString = Guid.NewGuid().ToString();
            var user = _context.Users.First();
            var order = new Order
            {
                Code = null,
                Created = DateTimeOffset.Now,
                CustomerUserId = user.Id,
                Deadline = DateTimeOffset.Now.AddDays(30),
                DeliveryAddress = rndString,
                IsDeleted = false,
                Name = rndString,
                ContractorUserId = user.Id,
                Price = DateTimeOffset.Now.Second,
                ShowPayment = true,
                Status = OrderStatus.Created,
                PostedByUserId = user.Id
            };
            var result = _orderOperations.AddAsync(order).Result;
            Assert.AreEqual(rndString, result.DeliveryAddress);
            Assert.AreEqual(rndString, result.Name);
        }

        [TestMethod]
        public void Delete_Ok_Test()
        {
            var order = _context.Orders.Take(100).ToList().Last();
            _orderOperations.DeleteAsync(order.Id).Wait();

            using (var cntxt = new LrdrContext())
            {
                Assert.IsNull(cntxt.Orders.FirstOrDefault(o => o.Id == order.Id));
            }
        }

        [TestMethod]
        public void UpdatePrice_Ok_Test()
        {
            var order = _context.Orders.First();
            var rndInt = DateTime.Now.Second;
            var result = _orderOperations.ChangePriceAsync(order.Id, rndInt).Result;
            Assert.AreEqual(rndInt, result.Price);
        }

        [TestMethod]
        public void UpdateStatus_Ok_Test()
        {
            var order = _context.Orders.First();
            var newStatus = OrderStatus.Cancelled;
            if (order.Status == OrderStatus.Cancelled)
            {
                newStatus = OrderStatus.Payed;
            }
            var result = _orderOperations.ChangeStatusAsync(order.Id, newStatus).Result;
            Assert.AreEqual(newStatus, result.Status);
        }

    }
}
