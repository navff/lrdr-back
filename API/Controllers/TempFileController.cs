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
    /// Работа с временными файлами. Сначала картинки загружаются как временные файлы с уникальным FormId.
    /// Затем после сохранения формы они сохраняются как картинки в Picture.
    /// </summary>
    [RoutePrefix("api/tempfile")]
    public class TempFileController: ApiController
    {
        private TempFileOperations _tempFileOperations;

        public TempFileController(TempFileOperations tempFileOperations)
        {
            _tempFileOperations = tempFileOperations;
        }

        /// <summary>
        /// Получение картинки
        /// </summary>
        [Route("{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            var tempFile = await _tempFileOperations.GetAsync(id);
            HttpResponseMessage result = null;

            if (tempFile == null)
            {
                result = Request.CreateResponse(HttpStatusCode.Gone);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.OK);
                Stream stream = new MemoryStream(tempFile.Data);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            }

            return new ResponseMessageResult(result);
        }


        /// <summary>
        /// Получение информации о временном файле
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("getinfo/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetInfo(int id)
        {
            var tempFile = await _tempFileOperations.GetAsync(id);
            var result = Mapper.Map<TempFileViewModelGet>(tempFile);
            result.Url = Url.Content($"~/api/tempfile/{tempFile.Id}");
            return Ok(result);
        }


        /// <summary>
        /// Добавление временного файла
        /// </summary>
        [HttpPost]
        // [RESTAuthorize(Role.PortalAdmin, Role.PortalManager)]
        public async  Task<IHttpActionResult> Post(TempFileViewModelPost postViewModel)
        {
            var tempFile = Mapper.Map<TempFile>(postViewModel);
            var result = await _tempFileOperations.AddAsync(tempFile);
            return await GetInfo(result.Id);
        }

        /// <summary>
        /// Удаление
        /// </summary>
        [HttpDelete]
        //[RESTAuthorize(Role.PortalAdmin, Role.PortalManager)]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            await _tempFileOperations.DeleteAsync(id);
            return Ok("Deleted");
        }

        /// <summary>
        /// Получение всех временных файлов по FormId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("byformid/{id}")]
        public async Task<IHttpActionResult> GetByFormId(string id)
        {
            var result = await _tempFileOperations.GetByFormIdAsync(id);
            return Ok(result);
        }


    }
}