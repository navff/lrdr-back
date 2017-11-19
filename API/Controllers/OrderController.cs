using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using API.Common;
using API.Operations;
using API.ViewModels;
using AutoMapper;
using Models.Dtos;
using Models.Entities;
using Models.Operations;
using Models.Tools;

namespace API.Controllers
{
    [RoutePrefix("api/order")]
    public class OrderController: ApiController
    {
        private OrderOperations _orderOperations;
        private UserOperations _userOperations;

        public OrderController(OrderOperations orderOperations, UserOperations userOperations)
        {
            _orderOperations = orderOperations;
            _userOperations = userOperations;
        }


        /// <summary>
        /// Получает конкретный заказ
        /// </summary>
        [HttpGet]
        [Route("{code}")]
        [RESTAuthorize]
        [ResponseType(typeof(OrderViewModelGet))]
        public async Task<IHttpActionResult> Get(string code)
        {
            var order = await _orderOperations.GetAsync(code);
            if (order == null) return this.Result404("This order is not found");
            var result = Mapper.Map<OrderViewModelGet>(order);
            return Ok(result);
        }

        /// <summary>
        /// Все заказы для текузего пользователя
        /// </summary>
        [HttpGet]
        [ResponseType(typeof(PageView<OrderShortViewModelGet>))]
        [RESTAuthorize]
        [Route("search")]
        public async Task<IHttpActionResult> Search([FromUri] SearchOrderViewModelGet searchViewModel)
        {
            if (searchViewModel == null) searchViewModel = new SearchOrderViewModelGet();
            var dto = await _orderOperations.SearchAsync(word: searchViewModel.Word, 
                                                         customerUserId: searchViewModel.CustomerUserId,
                                                         ownerUserId:searchViewModel.OwnerUserId,
                                                         isPaid:searchViewModel.isPaid,
                                                         sortby: searchViewModel.SortBy, 
                                                         page: searchViewModel.Page);
            var result = Mapper.Map<PageView<OrderShortViewModelGet>>(dto);
            return Ok(result);
        }

        /// <summary>
        /// Изменение заказа
        /// </summary>
        [HttpPut]
        [Route("{id}")]
        [ResponseType(typeof(OrderViewModelGet))]
        [RESTAuthorize]
        public async Task<IHttpActionResult> Put(int id, OrderViewModelPost putViewModel)
        {
            try
            {
                bool canEdit = await OrderOperations.CheckRights(id, User.Identity.Name);
                if (!canEdit) return this.Result403("You have no rights to edit this order");
            }
            catch (NotFoundException ex)
            {
                return this.Result404("Order is not found");
            }

            var order = Mapper.Map<Order>(putViewModel);
            var customer = await _userOperations.GetAsync(putViewModel.CustomerEmail);
            order.CustomerUserId = customer.Id;
            order.Id = id;
            var dto = await _orderOperations.UpdateAsync(order);
            return await Get(dto.Code);
        }

        /// <summary>
        /// Добавление нового заказа
        /// </summary>
        [HttpPost]
        [ResponseType(typeof(OrderViewModelGet))]
        [RESTAuthorize]
        public async Task<IHttpActionResult> Post(OrderViewModelPost postViewModel)
        {
            var order = Mapper.Map<Order>(postViewModel);
            var client = await _userOperations.GetAsync(postViewModel.CustomerEmail);
            order.CustomerUserId = client.Id;
            order = await _orderOperations.AddAsync(order);
            return await Get(order.Code);
        }


        /// <summary>
        /// Удаление заказа
        /// </summary>
        [HttpDelete]
        [Route("{id}")]
        [RESTAuthorize]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                bool canEdit = await OrderOperations.CheckRights(id, User.Identity.Name);
                if (!canEdit) return this.Result403("You have no rights to edit this order");
            }
            catch (NotFoundException ex)
            {
                return this.Result404("Order is not found");
            }

            await _orderOperations.DeleteAsync(id);
            return Ok("Deleted");
        }

        /// <summary>
        /// Обновление цены для заказа
        /// </summary>
        [Route("changeprice")]
        [HttpPut]
        [RESTAuthorize]
        public async Task<IHttpActionResult> ChangePrice([FromBody] OrderChangePriceViewModelPost orderChangePriceViewModel)
        {
            try
            {
                bool canEdit = await OrderOperations.CheckRights(orderChangePriceViewModel.OrderId, User.Identity.Name);
                if (!canEdit) return this.Result403("You have no rights to edit this order");
            }
            catch (NotFoundException ex)
            {
                return this.Result404("Order is not found");
            }
            

            var result = await _orderOperations.ChangePriceAsync(orderChangePriceViewModel.OrderId, orderChangePriceViewModel.Price);
            return await Get(result.Code);
        }

        /// <summary>
        /// Обновление статуса заказа
        /// </summary>
        [Route("changestatus")]
        [HttpPut]
        [RESTAuthorize]
        public async Task<IHttpActionResult> ChangeStatus([FromBody] OrderChangeStatusViewModelPost orderChangeStatusViewModel)
        {
            try
            {
                bool canEdit = await OrderOperations.CheckRights(orderChangeStatusViewModel.OrderId, User.Identity.Name);
                if (!canEdit) return this.Result403("You have no rights to edit this order");
            }
            catch (NotFoundException ex)
            {
                return this.Result404("Order is not found");
            }

            var result = await _orderOperations.ChangeStatusAsync(orderChangeStatusViewModel.OrderId,
                orderChangeStatusViewModel.Status);
            return await Get(result.Code);
        }
    }
}