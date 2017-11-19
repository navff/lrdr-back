using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using API;
using API.App_Start;
using API.Common;
using API.Controllers;
using API.Models;
using API.Operations;
using API.ViewModels;
using Models;
using Ninject.Web.Common;
using NLog;
using Tests.Helpers;

namespace Tests.Controllers
{
    [TestClass]
    public class UserControllerTest : BaseControllerTest
    {

        public UserControllerTest()
        {
            MapperMappings.Map();
        }


        [TestMethod]
        public void HTTP_GetByEmail_OK_Test()
        {
            var user = _context.Users.First();
            var result = HttpGet<UserViewModelGet>($"api/user?email={user.Email}");
            Assert.AreEqual(user.Email, result.Email);
        }

        [TestMethod]
        public void HTTP_GetMe_OK_Test()
        {
            var user = _context.Users.First();
            var result = HttpGet<UserViewModelGet>("api/user/getme", user.AuthToken);
            Assert.AreEqual(user.Email, result.Email);
        }

        [TestMethod]
        public void HTTP_Search_OK_Test()
        {
            var user = _context.Users.First(u => u.Role == Role.PortalAdmin);
            var result = HttpGet<IEnumerable<UserViewModelGet>>("api/user/search?word=var@33kita.ru", user.AuthToken);
            Assert.AreEqual("var@33kita.ru", result.First().Email);
        }

        [TestMethod]
        public void HTTP_SearchByPhone_OK_Test()
        {
            var user = _context.Users.First(u => u.Role == Role.PortalAdmin);
            var result = HttpGet<IEnumerable<UserViewModelGet>>($"api/user/search?word={user.Phone.Substring(2)}", user.AuthToken);
            Assert.AreEqual(user.Email, result.First().Email);
        }

        [TestMethod]
        public void HTTP_SearchByPhone_WrongText_Test()
        {
            var user = _context.Users.First(u => u.Role == Role.PortalAdmin);
            var result = HttpGet<IEnumerable<UserViewModelGet>>("api/user/search?word=idbdjdjdd93hbhsbishjs", user.AuthToken);
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void HTTP_Delete_OK_Test()
        {
            var userForDelete = _context.Users.Take(100).ToList().Last();
            var user = _context.Users.First(u => u.Role == Role.PortalAdmin);
            var result = HttpDelete<string>($"api/user/delete?email={userForDelete.Email}", user.AuthToken);
            Assert.AreEqual("Deleted "+ userForDelete.Email, result);
        }

        [TestMethod]
        [ExpectedException(typeof(WebException))]
        public void HTTP_Delete_WrongToken_Test()
        {
            var user = _context.Users.First();
            HttpDelete<string>($"api/user/delete?email={user.Email}", "wrong_token");
        }

        [TestMethod]
        [ExpectedException(typeof(WebException))]
        public void HTTP_Delete_AnotherUserToken_Test()
        {
            var userForDelete = _context.Users.Take(100).ToList().Last();
            var anotherUser = _context.Users.First(u => (u.Role == Role.RegisteredUser) && (u.Id != userForDelete.Id));
            HttpDelete<string>($"api/user/delete?email={userForDelete.Email}", anotherUser.AuthToken);
        }

        [TestMethod]
        public void HTTP_Put_Ok_Test()
        {
            var user = _context.Users.First();
            var rndString = Guid.NewGuid().ToString();

            var viewModel = new UserViewModelPut
            {
                Name = rndString,
                Phone = rndString,
                Role = user.Role,
                Email = user.Email,
                About = rndString
            };

            var result = HttpPut<UserViewModelGet>($"api/user?email={user.Email}", viewModel, user.AuthToken);

            Assert.AreEqual(rndString, result.Name);
            Assert.AreEqual(rndString, result.Phone);
            Assert.AreEqual(rndString, result.About);

        }

        [TestMethod]
        [ExpectedException(typeof(WebException))]
        public void HTTP_Put_AnotherUser_Test()
        {
            var user = _context.Users.First();
            var anotherUser = _context.Users.Where(u => u.Role == Role.RegisteredUser).Take(2).ToList().Last();

            var rndString = Guid.NewGuid().ToString();

            var viewModel = new UserViewModelPut
            {
                Name = rndString,
                Phone = rndString,
                Role = user.Role,
            };

            HttpPut<UserViewModelGet>($"api/user?email={user.Email}", viewModel, anotherUser.AuthToken);
        }

        [TestMethod]
        public void HTTP_Post_Ok_Test()
        {
            var rndString = Guid.NewGuid().ToString();
            var viewModel = new UserRegisterViewModel
            {
                Email = rndString+"@mhbb.ru"
            };
            var result = HttpPost<UserViewModelGet>($"api/user", viewModel);

            Assert.AreEqual(rndString+ "@mhbb.ru", result.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(WebException))]
        public void HTTP_Post_EmptyEmail_Test()
        {
            var viewModel = new UserRegisterViewModel
            {
                Email = ""
            };
            HttpPost<UserViewModelGet>($"api/user", viewModel);
        }

        [TestMethod]
        public void HTTP_Logout_Test()
        {
            var user = _context.Users.First();
            var oldToken = user.AuthToken;

            HttpPut<string>($"api/user/logout", null, oldToken);

            _context.Entry(user).State = EntityState.Modified;

            using (var context = new LrdrContext())
            {
                var updatedUserToken = context.Users.First(u => u.Email == user.Email).AuthToken;
                Assert.AreNotEqual(oldToken, updatedUserToken);
            }

            

            
        }
    }
}
