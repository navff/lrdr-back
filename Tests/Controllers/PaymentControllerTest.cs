using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using API.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using System.Data.Entity;

namespace Tests.Controllers
{
    [TestClass]
    public class PaymentControllerTest : BaseControllerTest
    {
        private User _user;
        public PaymentControllerTest()
        {
            _user = _context.Users.First(u => u.Role == Role.PortalAdmin);
        }

        [TestMethod]
        public void Get_Ok_Test()
        {
            var payment = _context.Payments.First();
            var url = $"api/payment/{payment.Id}";
            var result = HttpGet<PaymentViewModelGet>(url, _user.AuthToken);
            Assert.AreEqual(payment.Description, result.Description);
            Assert.AreEqual(payment.Sum, result.Sum);

        }

        [TestMethod]
        public void Search_ByCustomerUser_Ok_Test()
        {
            var payment = _context.Payments.Include(p => p.Order).First();
            var url = $"api/payment/search?id={payment.Order.CustomerUserId}&searchType=CustomerUser";
            var result = HttpGet<PageView<PaymentViewModelShortGet>>(url, _user.AuthToken);
            Assert.IsTrue(result.Content.Any());
        }

        [TestMethod]
        public void Put_Ok_Test()
        {
            var payment = _context.Payments.First();
            var rndInt = DateTimeOffset.Now.Second;
            var rndString = Guid.NewGuid().ToString();
            var url = $"api/payment/{payment.Id}";
            var putViewModel = new PaymentViewModelPost
            {
                OrderId = payment.OrderId,
                IsDeleted = false,
                Time = payment.Time,
                Sum = rndInt,
                ExternalUserId = rndString,
                Description = rndString,
                ExternalId = rndString
            };
            var result = HttpPut<PaymentViewModelGet>(url, putViewModel, _user.AuthToken);
            Assert.AreEqual(rndString, result.Description);
            Assert.AreEqual(rndString, result.ExternalUserId);
            Assert.AreEqual(rndString, result.ExternalId);
            Assert.AreEqual(rndInt, result.Sum);
        }

        [TestMethod]
        public void Post_Ok_Test()
        {
            var payment = _context.Payments.First();
            var rndInt = DateTimeOffset.Now.Second;
            var rndString = Guid.NewGuid().ToString();
            var url = $"api/payment";
            var putViewModel = new PaymentViewModelPost
            {
                OrderId = payment.OrderId,
                IsDeleted = false,
                Time = payment.Time,
                Sum = rndInt,
                ExternalUserId = rndString,
                Description = rndString,
                ExternalId = rndString
            };
            var result = HttpPost<PaymentViewModelGet>(url, putViewModel, _user.AuthToken);
            Assert.AreEqual(rndString, result.Description);
            Assert.AreEqual(rndString, result.ExternalUserId);
            Assert.AreEqual(rndString, result.ExternalId);
            Assert.AreEqual(rndInt, result.Sum);
            Assert.IsTrue(result.Id!=0);
        }

        [TestMethod]
        public void Delete_Ok_Test()
        {
            var payment = _context.Payments.ToList().Last();
            var url = $"api/payment/{payment.Id}";
            HttpDelete<string>(url, _user.AuthToken);

            using (var cntx = new LrdrContext())
            {
                Assert.IsNull(cntx.Payments.FirstOrDefault(p => p.Id == payment.Id));
            }
        }

        //[TestMethod]
        public void YmNotify_Ok_Test()
        {
            throw new NotImplementedException();
        }

        //[TestMethod]
        public void YmGetOperationDetails_Ok_Test()
        {
            throw new NotImplementedException();
        }


    }
}
