using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using API.Common;
using API.Models;
using API.ViewModels;
using AutoMapper;
using Models.Entities;
using Models.Operations;

namespace API.Controllers
{
    /// <summary>
    /// Работа с картинками
    /// </summary>
    [RoutePrefix("api/picture")]
    public class PictureController : ApiController
    {
        
        private PictureOperations _pictureOperations;

        public PictureController(PictureOperations pictureOperations)
        {
            _pictureOperations = pictureOperations;
        }

        /// <summary>
        /// Получает файл картинки
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetAsync(int id)
        {
            var picture = await _pictureOperations.GetAsync(id);
            HttpResponseMessage result = null;

            if (picture == null)
            {
                return this.Result404("Picture is not found");
            }

            result = Request.CreateResponse(HttpStatusCode.OK);
            Stream stream = new MemoryStream(picture.Data);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");

            return new ResponseMessageResult(result);
        }


        /// <summary>
        /// Получение информации о картинке
        /// </summary>
        [Route("getinfo/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetInfo(int id)
        {
            var picture = await _pictureOperations.GetAsync(id);
            if (picture == null)
            {
                return this.Result404("Picture is not found");
            }

            var result = Mapper.Map<PictureViewModelGet>(picture);
            result.Url = Url.Content($"~/api/picture/{picture.Id}");
            return Ok(result);
        }

        [HttpDelete]
        [Route("{id}")]
        [RESTAuthorize(Role.PortalAdmin, Role.PortalManager)]
        public async Task<IHttpActionResult> DeleteAsync(int id)
        {
            await _pictureOperations.DeleteAsync(id);
            return Ok("Deleted");
        }

        /// <summary>
        /// Получает картинки по активности
        /// </summary>
        [HttpGet]
        [Route("byactivity/{activityId}")]
        public async Task<IHttpActionResult> GetByActivityAsync(int activityId)
        {
            var result = await _pictureOperations.GetByLinkedObject(LinkedObjectType.Activity, activityId);
            return Ok(Mapper.Map<IEnumerable<PictureViewModelGet>>(result));
        }

        /// <summary>
        /// Получает картинки организатора
        /// </summary>
        [HttpGet]
        [Route("byorganizer/{organizerId}")]
        public async Task<IHttpActionResult> GetByOrganizerAsync(int organizerId)
        {
            var result = await _pictureOperations.GetByLinkedObject(LinkedObjectType.Organizer, organizerId);
            return Ok(Mapper.Map<IEnumerable<PictureViewModelGet>>(result));
        }



    }
}