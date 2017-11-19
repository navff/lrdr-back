using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Operations;
using System.Data.Entity;
using API.Common;
using API.Operations;
using Models;
using Models.Entities;
using Ninject;

namespace Tests.Operations
{
    [TestClass]
    public class PaymentOperationsTest :  BaseTest
    {
        private PaymentOperations _paymentOperations;

        public PaymentOperationsTest()
        {
            var kernel = new StandardKernel();
            new NinjectDependencyResolver(kernel);
            _paymentOperations = kernel.Get<PaymentOperations>();
        }

        [TestMethod]
        public void Get_Ok_Test()
        {
            var payment = _context.Payments.First();
            var result = _paymentOperations.GetAsync(payment.Id).Result;
            Assert.AreEqual(payment.Sum, result.Sum);
        }

        [TestMethod]
        public void Search_ByCustomerUser_Ok_Test()
        {
            var payment = _context.Payments.Include(p => p.Order).First(p => p.Order.CustomerUserId.HasValue);
            var result = _paymentOperations.Search(PaymentSearchType.CustomerUser, payment.Order.CustomerUserId.Value).Result;
            Assert.IsTrue(result.Content.Any());
        }

        [TestMethod]
        public void GetBySystemUser_Ok_Test()
        {
            var payment = _context.Payments.Include(p => p.Order).First();
            var result = _paymentOperations.Search(PaymentSearchType.SystemUser, payment.UserId).Result;
            Assert.IsTrue(result.Content.Any());
        }

        [TestMethod]
        public void GetByOrder_Ok_Test()
        {
            var payment = _context.Payments.Include(p => p.Order).First(p => p.OrderId.HasValue);
            var result = _paymentOperations.Search(PaymentSearchType.Order, payment.OrderId.Value).Result;
            Assert.IsTrue(result.Content.Any());
        }

        [TestMethod]
        public void Add_Ok_Test()
        {
            var rndString = Guid.NewGuid().ToString();
            var rndInt = DateTime.Now.Second;
            var order = _context.Orders.First();
            var payment = new Payment
            {
                Description =rndString,
                ExternalId = rndString,
                ExternalUserId = rndString,
                IsDeleted = false,
                Sum = rndInt,
                Time = DateTimeOffset.Now,
                OrderId = order.Id
            };
            var result = _paymentOperations.AddAsync(payment).Result;
            Assert.IsTrue(result.Id!=0);
        }

        [TestMethod]
        public void Update_Ok_Test()
        {
            var rndString = Guid.NewGuid().ToString();
            var rndInt = DateTime.Now.Second;
            var paymentDbEntity = _context.Payments.First();
            var payment = new Payment
            {
                Description = rndString,
                ExternalId = rndString,
                ExternalUserId = rndString,
                IsDeleted = false,
                Sum = rndInt,
                Time = DateTimeOffset.Now,
                OrderId = paymentDbEntity.OrderId,
                Id = paymentDbEntity.Id
            };
            var result = _paymentOperations.UpdateAsync(payment).Result;
            Assert.AreEqual(rndString, result.Description);
        }

        [TestMethod]
        public void Delete_Ok_Test()
        {
            var payment = _context.Payments.ToList().Last();
            _paymentOperations.DeleteAsync(payment.Id).Wait();

            using (var cntxt = new LrdrContext())
            {
                Assert.IsNull(cntxt.Payments.FirstOrDefault(p => p.Id == payment.Id));
            }
        }

    }
}
