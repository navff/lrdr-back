using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using API.Common;
using API.Models;
using API.ViewModels;

namespace API.Controllers
{
    /// <summary>
    /// https://money.yandex.ru/myservices/online.xml
    /// https://money.yandex.ru/page?id=523014
    /// </summary>

    [RoutePrefix("api/payment")]
    public class PaymentController : ApiController
    {
        

        [HttpGet]
        [RESTAuthorize]
        [Route("{id}")]
        [ResponseType(typeof(PaymentViewModelGet))]
        public async Task<IHttpActionResult> Get(int id)
        {
            //TODO: проверить права юзера
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("byuser/{userId}")]
        [RESTAuthorize(Role.PortalAdmin, Role.PortalManager)]
        [ResponseType(typeof(PageView<PaymentViewModelShortGet>))]
        public async Task<IHttpActionResult> GetByUser(int userId)
        {
            //TODO: проверить права юзера
            throw new NotImplementedException();
        }

        [HttpPut]
        [RESTAuthorize(Role.PortalAdmin, Role.PortalManager)]
        [Route("{id}")]
        [ResponseType(typeof(PaymentViewModelGet))]
        public async Task<IHttpActionResult> Put(int id, object putViewModel)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [RESTAuthorize(Role.PortalManager, Role.PortalAdmin)]
        [ResponseType(typeof(PaymentViewModelGet))]
        public async Task<IHttpActionResult> Post(object postViewModel)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [RESTAuthorize(Role.PortalManager, Role.PortalAdmin)]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            throw new NotImplementedException();
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