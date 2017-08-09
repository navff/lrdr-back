using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using API.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Entities;

namespace Tests.Controllers
{
    [TestClass]
    public class PictureControllerTest : BaseControllerTest
    {
        [TestMethod]
        public void Get_Ok_Test()
        {
            var tempFile = _context.Pictures.First();
            string url = $"api/picture/{tempFile.Id}";
            HttpGet<object>(url);
        }

        [TestMethod]
        public void Get_Info_Test()
        {
            var tempFile = _context.Pictures.First();
            string url = $"api/picture/getinfo/{tempFile.Id}";
            HttpGet<PictureViewModelGet>(url);
        }

        [TestMethod]
        [ExpectedException(typeof(WebException))]
        public void Get_404_Test()
        {
            string url = $"api/picture/999999";
            HttpGet<object>(url);
        }

        [TestMethod]
        public void Delete_Ok_Test()
        {
            var user = _context.Users.First(u => u.Role == Role.PortalAdmin);
            var picture = _context.Pictures.Take(10).ToList().Last();
            string url = $"api/picture/{picture.Id}";
            HttpDelete<string>(url, user.AuthToken);

            //Восстанавливаем всё как было
            _context.Pictures.Add(picture);
            _context.SaveChanges();
        }

        [TestMethod]
        public void GetByActivity_Ok_Test()
        {
            var activityId = _context.Pictures.Where(p => (p.LinkedObjectType == LinkedObjectType.Activity))
                                                .Select(p => p.LinkedObjectId).First();
            string url = $"api/picture/byactivity/{activityId}";
            var result = HttpGet<IEnumerable<PictureViewModelGet>>(url);
            Assert.IsTrue(result.Any());
        }

        //[TestMethod]
        public void GetByOrganizer_Ok_Test()
        {
            var organizerId = _context.Pictures.Where(p => (p.LinkedObjectType == LinkedObjectType.Organizer))
                                                .Select(p => p.LinkedObjectId).First();
            string url = $"api/picture/byorganizer/{organizerId}";
            var result = HttpGet<IEnumerable<PictureViewModelGet>>(url);
            Assert.IsTrue(result.Any());
        }


    }
}
