using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Models.Entities;
using Models.Operations;
using System.Data.Entity;

namespace Tests.Operations
{
    [TestClass]
    public class CommentOperationsTest : BaseTest
    {
        private CommentOperations _commentOperations;

        public CommentOperationsTest()
        {
            _commentOperations = new CommentOperations(_context);
        }

        [TestMethod]
        public void Get_Ok_Test()
        {
            var comment = _context.Comments.First();
            var result = _commentOperations.GetAsync(comment.Id).Result;
            Assert.AreEqual(comment.Id, result.Id);
        }

        [TestMethod]
        public void GetByOrder_Ok_Test()
        {
            var comment = _context.Comments.First();
            var result = _commentOperations.GetAllByOrder(comment.OrderId).Result;
            Assert.IsTrue(result.Content.Any());
        }

        [TestMethod]
        public void GetByUser_Ok_Test()
        {
            var comment = _context.Comments.Include(c => c.Order).First();
            var result = _commentOperations.GetAllByUser(comment.Order.OwnerUserId).Result;
            Assert.IsTrue(result.Content.Any());
        }

        [TestMethod]
        public void Add_Ok_Test()
        {
            var order = _context.Orders.First();
            var user = _context.Users.First();
            var rndString = Guid.NewGuid().ToString();
            var comment = new Comment
            {
                OrderId = order.Id,
                Text = rndString,
                UserId  = user.Id
            };
            var result = _commentOperations.AddAsync(comment).Result;
            Assert.AreEqual(rndString, result.Text);
        }

        [TestMethod]
        public void Update_Ok_Test()
        {
            var comment = _context.Comments.First();
            var rndString = Guid.NewGuid().ToString();
            var result = _commentOperations.UpdateAsync(comment.Id, rndString).Result;
            Assert.AreEqual(rndString, result.Text);
        }

        [TestMethod]
        public void Delete_Ok_Test()
        {
            var comment = _context.Comments.ToList().Last();
            _commentOperations.DeleteAsync(comment.Id).Wait();

            using (var cntxt = new LrdrContext())
            {
                Assert.IsNull(cntxt.Comments.FirstOrDefault(c => c.Id == comment.Id));
            }
        }

        [TestMethod]
        public void SetAsRead_Ok_Test()
        {
            var order = _context.Orders.First();
            var rndString = Guid.NewGuid().ToString();
            var comment = new Comment
            {
                OrderId = order.Id,
                Text = rndString,
                UserId = order.OwnerUserId,
                IsReaded = false
            };
            _context.Comments.Add(comment);
            _context.SaveChanges();

            var customerUser = _context.Users.First(u => u.Id == order.CustomerUserId);
            var result = _commentOperations.SetAsRead(comment.Id, customerUser.Email).Result;
            Assert.IsTrue(result.IsReaded);
        }

    }
}
