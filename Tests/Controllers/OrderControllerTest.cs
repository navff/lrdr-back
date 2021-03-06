﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API;
using API.Models;
using API.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Entities;
using System.Data.Entity;

namespace Tests.Controllers
{
    [TestClass]
    public class OrderControllerTest: BaseControllerTest
    {
        public OrderControllerTest()
        {
            MapperMappings.Map();
        }

        [TestMethod]
        public void SearchByName_Ok_Test()
        {
            var order = _context.Orders.First();
            var user = _context.Users.First(u => u.Role == Role.PortalAdmin);
            var url = $"api/order/search?word={order.Name.Substring(2)}";
            var result = HttpGet<PageView<OrderShortViewModelGet>>(url, user.AuthToken);
            Assert.IsTrue(result.Content.Any());
        }

        [TestMethod]
        public void SearchByEmail_Ok_Test()
        {
            var order = _context.Orders.First();
            var user = _context.Users.First(u => u.Role == Role.PortalAdmin);
            var url = $"api/order/search?word={order.ContractorUser.Email}";
            var result = HttpGet<PageView<OrderShortViewModelGet>>(url, user.AuthToken);
            Assert.IsTrue(result.Content.Any());
        }

        [TestMethod]
        public void Get_Ok_Test()
        {
            var order = _context.Orders.First();
            var user = _context.Users.First(u => u.Role == Role.PortalAdmin);
            var url = $"api/order/{order.Code}";
            var result = HttpGet<OrderShortViewModelGet>(url, user.AuthToken);
            Assert.AreEqual(order.Code, result.Code);
        }

        [TestMethod]
        public void Put_Ok_Test()
        {
            var rndString = Guid.NewGuid().ToString();
            var order = _context.Orders.Include(o => o.CustomerUser).First();
            var user = _context.Users.First(u => u.Role == Role.PortalAdmin);
            var url = $"api/order/{order.Id}";
            var viewModel = new OrderViewModelPost
            {
                Name = rndString,
                CustomerEmail = order.CustomerUser.Email,
                Deadline = order.Deadline,
                ContractorUserId = order.ContractorUserId
            };
            var result = HttpPut<OrderViewModelGet>(url, viewModel, user.AuthToken);
            Assert.AreEqual(rndString, result.Name);
        }

        [TestMethod]
        public void PostByCustomer_Ok_Test()
        {
            var rndString = Guid.NewGuid().ToString();
            var customer = _context.Users.First(u => u.Role == Role.PortalAdmin);
            var contractor = _context.Users.First(u => u.Role != Role.PortalAdmin);

            var url = $"api/order";
            var viewModel = new OrderViewModelPost
            {
                Name = rndString,
                CustomerEmail = customer.Email,
                Deadline = DateTimeOffset.Now.AddDays(60),
                ContractorUserId = contractor.Id
            };
            var result = HttpPost<OrderViewModelGet>(url, viewModel, customer.AuthToken);
            Assert.AreEqual(rndString, result.Name);
        }

        [TestMethod]
        public void PostByContractor_Ok_Test()
        {
            var rndString = Guid.NewGuid().ToString();
            var customer = _context.Users.First(u => u.Role == Role.PortalAdmin);
            var contractor = _context.Users.First(u => u.Role != Role.PortalAdmin);

            var url = $"api/order";
            var viewModel = new OrderViewModelPost
            {
                Name = rndString,
                CustomerEmail = customer.Email,
                Deadline = DateTimeOffset.Now.AddDays(60),
                ContractorUserId = contractor.Id,
                DeliveryAddress = rndString
            };
            var result = HttpPost<OrderViewModelGet>(url, viewModel, contractor.AuthToken);
            Assert.AreEqual(rndString, result.Name);
        }

        [TestMethod]
        public void Delete_Ok_Test()
        {
            var order = _context.Orders.Take(10).ToList().Last();
            var user = _context.Users.First(u => u.Role == Role.PortalAdmin);
            var url = $"api/order/{order.Id}";
            var result = HttpDelete<string>(url, user.AuthToken);
        }

        [TestMethod]
        public void ChangePrice_Ok_Test()
        {
            var rndInt = DateTime.Now.Second;
            var order = _context.Orders.Take(10).ToList().Last();
            var user = _context.Users.First(u => u.Role == Role.PortalAdmin);
            var url = $"api/order/changeprice";
            var viewModel = new OrderChangePriceViewModelPost
            {
                OrderId = order.Id,
                Price = rndInt
            };
            var result = HttpPut<OrderViewModelGet>(url, viewModel, user.AuthToken);
            Assert.AreEqual(rndInt, result.Price);
        }

        [TestMethod]
        public void ChangeStatus_Ok_Test()
        {
            var order = _context.Orders.Take(10).ToList().Last();
            var user = _context.Users.First(u => u.Role == Role.PortalAdmin);
            var url = $"api/order/changestatus";
            var viewModel = new OrderChangeStatusViewModelPost
            {
                OrderId = order.Id,
                Status = OrderStatus.Payed
            };
            var result = HttpPut<OrderViewModelGet>(url, viewModel, user.AuthToken);
            Assert.AreEqual(OrderStatus.Payed, result.Status);
        }
    }
}
