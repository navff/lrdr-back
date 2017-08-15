using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
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

        [Route("{id}")]
        [HttpGet]
        [RESTAuthorize]
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

        [Route("byorder/{orderId}")]
        [HttpGet]
        [RESTAuthorize]
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

        [Route("byuser/{userId}")]
        [HttpGet]
        [RESTAuthorize]
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

        [HttpPut]
        [Route("{id}")]
        [RESTAuthorize]
        public async Task<IHttpActionResult> Put(int id, CommentViewModelPut putViewModel)
        {
            var canEdit = await _commentOperations.CheckRights(id, User.Identity.Name);
            if (!canEdit) return this.Result403("You haven't rights to edit this comment");

            await _commentOperations.UpdateAsync(id, putViewModel.Text);
            return await Get(id);
        }

        [HttpPost]
        [RESTAuthorize]
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