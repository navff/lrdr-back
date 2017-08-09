using System;
using System.Linq;
using API.Models;
using API.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Operations
{
    [TestClass]
    public class UsersOperationsTest : BaseTest
    {
        private UserOperations _userOperations;

        public UsersOperationsTest()
        {
            _userOperations = new UserOperations(_context);
        }

        [TestMethod]
        public void GetUserByToken_Ok_Test()
        {
            var token = _context.Users.First().AuthToken;

            var user = _userOperations.GetUserByTokenAsync(token).Result;

            Assert.IsNotNull(user);
            Assert.AreEqual(token, user.AuthToken);
        }

        [TestMethod]
        public void GetUserByToken_WrongToken_Test()
        {

            var user = _userOperations.GetUserByTokenAsync("hooy").Result;

            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUserByEmail_Ok_Test()
        {
            var user = _context.Users.First();
            var result = _userOperations.GetAsync(user.Email).Result;
            Assert.AreEqual(user.Email, result.Email);
            Assert.AreEqual(user.Name, result.Name);
            Assert.AreEqual(user.Role, result.Role);
            Assert.AreEqual(user.AuthToken, result.AuthToken);
        }

        [TestMethod]
        public void GetUserByEmail_WrongEmail_Test()
        {
            var result = _userOperations.GetAsync("piska").Result;
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Update_Ok_Test()
        {
            var randomString = Guid.NewGuid().ToString();
            var user = _context.Users.First(u => u.Role != Role.PortalAdmin);
            user.Name = randomString;
            user.Phone = randomString;
            var result = _userOperations.UpdateAsync(user).Result;

            Assert.AreEqual(randomString, result.Name);
            Assert.AreEqual(randomString, result.Phone);
        }

        [TestMethod]
        public void Add_Ok_Test()
        {
            var picture = _context.Pictures.First();

            var randomString = Guid.NewGuid().ToString();
            var user = new User
            {
                Name = randomString,
                Phone = randomString,
                Email = randomString + "@33kita.ru",
                AuthToken = randomString,
                PictureId = picture.Id,
                Role = Role.RegisteredUser,
                DateRegistered = DateTime.Now
            };
            var result = _userOperations.AddAsync(user).Result;

            Assert.AreEqual(randomString, result.Name);
            Assert.AreEqual(randomString, result.Phone);
        }

        [TestMethod]
        public void Delete_ok_Test()
        {
            var user = _context.Users.Where(u => u.Role == Role.RegisteredUser).ToList().Last();
            _userOperations.DeleteAsync(user.Email).Wait();

            var deletedUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            Assert.IsNull(deletedUser);
        }

       // [TestMethod]
        public void Register_Existing_Test()
        {
            var result = _userOperations.RegisterAsync("var@33kita.ru").Result;
            Assert.IsTrue(!String.IsNullOrEmpty(result.AuthToken));
        }

        //[TestMethod]
        public void Register_New_Test()
        {
            var result = _userOperations.RegisterAsync(Guid.NewGuid()+"@33kita.ru").Result;
            Assert.IsTrue(!String.IsNullOrEmpty(result.AuthToken));
        }

        [TestMethod]
        public void Search_ByEmail_Test()
        {
            var user = _context.Users.First();
            var result = _userOperations.SearchAsync(word: user.Email).Result;
            Assert.IsNotNull(result);
            Assert.AreEqual(user.Email, result.First().Email);
        }

        [TestMethod]
        public void Search_ByWrongEmail_Test()
        {
            var result = _userOperations.SearchAsync(word: "fsdjhkkjsdfsdfjkhdfskjhjdfskr@33kita.ru").Result;
            Assert.IsTrue(!result.Any());
        }

        [TestMethod]
        public void Search_ByName_Test()
        {
            var user = _context.Users.First();
            var result = _userOperations.SearchAsync(word: user.Name.Substring(2)).Result;
            Assert.IsNotNull(result);
            Assert.AreEqual(user.Email, result.First().Email);
        }

        [TestMethod]
        public void Search_ByWrongName_Test()
        {
            var result = _userOperations.SearchAsync(word: "Моркккккккк!!!рол").Result;
            Assert.IsTrue(!result.Any());
        }
    }
}
