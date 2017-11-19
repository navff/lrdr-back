using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using API.Common;
using API.Models;
using API.Operations;
using API.ViewModels;
using AutoMapper;
using Models.Entities;
using Models.Operations;

namespace API.Controllers
{
    /// <summary>
    /// https://money.yandex.ru/myservices/online.xml
    /// https://money.yandex.ru/page?id=523014
    /// </summary>

    [RoutePrefix("api/payment")]
    public class PaymentController : ApiController
    {

        private PaymentOperations _paymentOperations;
        private UserOperations _userOperations;
        private OrderOperations _orderOperations;

        public PaymentController(PaymentOperations paymentOperations, UserOperations userOperations, OrderOperations orderOperations)
        {
            _paymentOperations = paymentOperations;
            _userOperations = userOperations;
            _orderOperations = orderOperations;
        }


        [HttpGet]
        [RESTAuthorize]
        [Route("{id}")]
        [ResponseType(typeof(PaymentViewModelGet))]
        public async Task<IHttpActionResult> Get(int id)
        {
            if (!User.IsInRole("PortalAdmin") && !User.IsInRole("PortalManager"))
            {
                var canEdit = await _paymentOperations.CheckRights(id, User.Identity.Name);
                if (!canEdit) return this.Result403("You haven't rights to see this payment");
            }

            var payment = await _paymentOperations.GetAsync(id);
            if (payment==null) return this.Result404("This payment is not found");

            var result = Mapper.Map<PaymentViewModelGet>(payment);
            return Ok(result);
        }

        /// <summary>
        /// Получает список платежей пользователя. Доступ самому этому пользователю и админам и менеджерам.
        /// </summary>
        [HttpGet]
        [Route("search")]
        [RESTAuthorize]
        [ResponseType(typeof(PageView<PaymentViewModelGet>))]
        public async Task<IHttpActionResult> Search(int? id=null, 
                                                    PaymentSearchType searchType = PaymentSearchType.All, 
                                                    bool isDeleted = false,
                                                    int page=1)
        {
            if (!User.IsInRole("PortalAdmin") && !User.IsInRole("PortalManager"))
            {
                var currentUser = await _userOperations.GetAsync(User.Identity.Name);
                if (searchType != PaymentSearchType.Order)
                {
                    var canRead = id == currentUser.Id;
                    if (!canRead) return this.Result403("You haven't rights to see payments for other users");
                }
                else
                {
                    var canRead = await OrderOperations.CheckRights(id.Value, User.Identity.Name);
                    if (!canRead) return this.Result403("You haven't rights to see payments for other user orders");
                }
                
            }

            var paymentsPageView = await _paymentOperations.Search(searchType, id, isDeleted, page);
            var result = Mapper.Map<PageView<PaymentViewModelGet>>(paymentsPageView);
            return Ok(result);
        }

        

        [HttpPut]
        [RESTAuthorize(Role.PortalAdmin, Role.PortalManager)]
        [Route("{id}")]
        [ResponseType(typeof(PaymentViewModelGet))]
        public async Task<IHttpActionResult> Put(int id, PaymentViewModelPost putViewModel)
        {
            if (!User.IsInRole("PortalAdmin") && !User.IsInRole("PortalManager"))
            {
                var canEdit = await OrderOperations.CheckRights(putViewModel.OrderId.Value, User.Identity.Name);
                if (!canEdit) return this.Result403("You haven't rights to add payments this order");
            }

            var payment = Mapper.Map<Payment>(putViewModel);
            payment.Id = id;
            var result = await _paymentOperations.UpdateAsync(payment);
            return await Get(result.Id);
        }

        [HttpPost]
        [RESTAuthorize()]
        [ResponseType(typeof(PaymentViewModelGet))]
        public async Task<IHttpActionResult> Post(PaymentViewModelPost postViewModel)
        {
            if (!User.IsInRole("PortalAdmin") && !User.IsInRole("PortalManager"))
            {
                var canEdit = await OrderOperations.CheckRights(postViewModel.OrderId.Value, User.Identity.Name);                
                if (!canEdit) return this.Result403("You haven't rights to add payments this order");
            }

            var payment = Mapper.Map<Payment>(postViewModel);
            var result = await _paymentOperations.AddAsync(payment);
            return await Get(result.Id);
        }

        [HttpDelete]
        [RESTAuthorize(Role.PortalManager, Role.PortalAdmin)]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            if (!User.IsInRole("PortalAdmin") && !User.IsInRole("PortalManager"))
            {
                var canEdit = await _paymentOperations.CheckRights(id, User.Identity.Name);
                if (!canEdit) return this.Result403("You haven't rights to add payments this order");
            }

            await _paymentOperations.DeleteAsync(id);
            return Ok("Deleted");
        }

        /// <summary>
        /// https://tech.yandex.ru/money/doc/dg/reference/notification-p2p-incoming-docpage/
        /// </summary>
        [HttpPost]
        [Route("ym-notify")]
        public async Task<IHttpActionResult> YmNotify(YmNotificationViewModelPost ymNotification)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tech.yandex.ru/money/doc/dg/reference/operation-details-docpage/
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ym-get-operation-details/{operation_id}")]
        [ResponseType(typeof(YmOperationDetailsViewModel))]
        public async Task<IHttpActionResult> YmGetOperationDetails(string operation_id)
        {
            throw new NotImplementedException();
        }
    }
}