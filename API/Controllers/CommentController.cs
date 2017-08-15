using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using API.Common;
using API.Operations;
using API.ViewModels;
using AutoMapper;
using Models.Entities;
using Models.Operations;
using Models.Tools;

namespace API.Controllers
{
    [RoutePrefix("api/comment")]
    public class CommentController : ApiController
    {
        private CommentOperations _commentOperations;
        private UserOperations _userOperations;

        public CommentController(CommentOperations commentOperations, UserOperations userOperations)
        {
            _commentOperations = commentOperations;
            _userOperations = userOperations;
        }

        /// <summary>
        /// Получает один конкретный коммент
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        [RESTAuthorize]
        [ResponseType(typeof(CommentViewModelGet))]
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                var comment = await _commentOperations.GetAsync(id);
                var result = Mapper.Map<CommentViewModelGet>(comment);
                return Ok(result);
            }
            catch (NotFoundException e)
            {
                return this.Result404("Comment is not found");
            }
        }

        /// <summary>
        /// Получает все комменты к опреденённому заказу
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Route("byorder/{orderId}")]
        [HttpGet]
        [RESTAuthorize]
        [ResponseType(typeof(PageView<CommentViewModelGet>))]
        public async Task<IHttpActionResult> GetAllByOrder(int orderId)
        {
            try
            {
                var comments = await _commentOperations.GetAllByOrder(orderId);
                var result = Mapper.Map<PageView<CommentViewModelGet>>(comments);
                return Ok(result);
            }
            catch (NotFoundException e)
            {
                return this.Result404("Order is not found");
            }
        }

        /// <summary>
        /// Получает все комменты пользователя
        /// </summary>
        [Route("byuser/{userId}")]
        [HttpGet]
        [RESTAuthorize]
        [ResponseType(typeof(PageView<CommentViewModelGet>))]
        public async Task<IHttpActionResult> GetAllByUser(int userId)
        {
            try
            {
                var comments = await _commentOperations.GetAllByUser(userId);
                var result = Mapper.Map<PageView<CommentViewModelGet>>(comments);
                return Ok(result);
            }
            catch (NotFoundException e)
            {
                return this.Result404("User is not found");
            }
        }
        
        /// <summary>
        /// Обновляет текст коммента
        /// </summary>
        [HttpPut]
        [Route("{id}")]
        [RESTAuthorize]
        [ResponseType(typeof(CommentViewModelGet))]
        public async Task<IHttpActionResult> Put(int id, CommentViewModelPut putViewModel)
        {
            var canEdit = await _commentOperations.CheckRights(id, User.Identity.Name);
            if (!canEdit) return this.Result403("You haven't rights to edit this comment");

            await _commentOperations.UpdateAsync(id, putViewModel.Text);
            return await Get(id);
        }


        /// <summary>
        /// Добавляет коммент
        /// </summary>
        [HttpPost]
        [RESTAuthorize]
        [ResponseType(typeof(CommentViewModelGet))]
        public async Task<IHttpActionResult> Post(CommentViewModelPost postViewModel)
        {
            var user = await _userOperations.GetAsync(User.Identity.Name);
            var comment = new Comment()
            {
                OrderId = postViewModel.OrderId,
                Text = postViewModel.Text,
                UserId = user.Id
            };
            var result = await _commentOperations.AddAsync(comment);
            return await Get(result.Id);
        }

        /// <summary>
        /// Удаляет коммент
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [RESTAuthorize]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var canEdit = await _commentOperations.CheckRights(id, User.Identity.Name);
            if (!canEdit) return this.Result403("You haven't rights to delete this comment");

            await _commentOperations.DeleteAsync(id);
            return Ok("Deleted");
        }
    }
}