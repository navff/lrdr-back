using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using API.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using Models;

namespace Tests.Controllers
{
    [TestClass]
    public class CommentControllerTest : BaseControllerTest
    {
        private User _user;
        public CommentControllerTest()
        {
            _user = _context.Users.First(u => u.Role == Role.PortalAdmin);
        }

        [TestMethod]
        public void Get_Ok_Test()
        {
            var comment = _context.Comments.First();
            var url = $"api/comment/{comment.Id}";
            var result = HttpGet<CommentViewModelGet>(url, _user.AuthToken);
            Assert.AreEqual(comment.Text, result.Text);
        }

        [TestMethod]
        public void GetByOrder_Ok_Test()
        {
            var comment = _context.Comments.Include(c => c.Order).First();
            var url = $"api/comment/byorder/{comment.OrderId}";
            var result = HttpGet<PageView<CommentViewModelGet>>(url, _user.AuthToken);
            Assert.IsTrue(result.Content.Any());
        }

        [TestMethod]
        public void GetByUser_Ok_Test()
        {
            var comment = _context.Comments.Include(c => c.Order).First();
            var url = $"api/comment/byuser/{comment.UserId}";
            var result = HttpGet<PageView<CommentViewModelGet>>(url, _user.AuthToken);
            Assert.IsTrue(result.Content.Any());
        }

        [TestMethod]
        public void Post_Ok_Test()
        {
            var comment = _context.Comments.First();
            var rndString  = Guid.NewGuid().ToString();

            var viewModel = new CommentViewModelPost
            {
                OrderId = comment.OrderId,
                Text = rndString,
            };
            var url = $"api/comment";
            var result = HttpPost<CommentViewModelGet>(url, viewModel, _user.AuthToken);
            Assert.AreEqual(rndString, result.Text);
        }

        [TestMethod]
        public void Put_Ok_Test()
        {
            var comment = _context.Comments.First();
            var rndString = Guid.NewGuid().ToString();

            var viewModel = new CommentViewModelPut {Text = rndString}; 
            var url = $"api/comment/{comment.Id}";
            var result = HttpPut<CommentViewModelGet>(url, viewModel, _user.AuthToken);
            Assert.AreEqual(rndString, result.Text);
        }

        [TestMethod]
        public void Delete_Ok_Test()
        {
            var comment = _context.Comments.ToList().Last();
            var url = $"api/comment/{comment.Id}";
            HttpDelete<string>(url, _user.AuthToken);

            using (var cntxt = new LrdrContext())
            {
                Assert.IsNull(cntxt.Comments.FirstOrDefault(c => c.Id == comment.Id));
            }

        }
    }
}
