using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using API.Common;
using API.Operations;
using API.ViewModels;
using AutoMapper;
using Models.HelpClasses;
using Models.Operations;
using Models.Entities;
using File = Models.Entities.File;

namespace API.Controllers
{
    [RoutePrefix("api/file")]
    public class FileController : ApiController
    {
        private FileOperations _fileOperations;
        private CommentOperations _commentOperations;
        private UserOperations _userOperations;

        public FileController(FileOperations fileOperations, CommentOperations commentOperations, UserOperations userOperations)
        {
            _fileOperations = fileOperations;
            _commentOperations = commentOperations;
            _userOperations = userOperations;
        }

        [HttpGet]
        [Route("{code}")]
        [ResponseType(typeof(FileViewModelGet))]
        public async Task<IHttpActionResult> Get(string code)
        {
            var file = await _fileOperations.GetFile(code);
            if (file == null) return this.Result404("This file is not found in Db");

            var result = Mapper.Map<FileViewModelGet>(file);
            return Ok(result);
        }

        [HttpGet]
        [Route("data/{code}")]
        [ResponseType(typeof(byte[]))]
        public async Task<IHttpActionResult> GetData(string code, bool thumb=false)
        {
            var file = await _fileOperations.GetFile(code);
            if (file == null) return this.Result404("This file is not found in Db");

            try
            {
                var data = await _fileOperations.GetFileData(code, thumb);
                var result = Request.CreateResponse(HttpStatusCode.OK);
                Stream stream = new MemoryStream(data);
                result.Content = new StreamContent(stream);
                if (FileHelpers.IsImage(file.Extension))
                {
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue($"image/{file.Extension}");
                }

                return new ResponseMessageResult(result);
            }
            catch (FileNotFoundException e)
            {
                return this.Result404("This file is not found in file system");
            }
            
        }

        [RESTAuthorize]
        [ResponseType(typeof(FileViewModelGet))]
        [HttpPost]
        public async Task<IHttpActionResult> Post(FileViewModelPost postViewModel)
        {
            var file = Mapper.Map<File>(postViewModel);
            var result = await _fileOperations.Addfile(file, postViewModel.Data);
            return await Get(result.Code);
        }

        [Route("to-exists-object")]
        [RESTAuthorize]
        [ResponseType(typeof(FileViewModelGet))]
        [HttpPost]
        public async Task<IHttpActionResult> PostToExistObject(FileToExistsObjectViewModelPost postViewModel)
        {
            bool canEdit = false;

            if (postViewModel.LinkedObjectType == LinkedObjectType.Comment)
            {
                canEdit = await _commentOperations.CheckRights(postViewModel.LinkedObjectId, User.Identity.Name);
            }
            else if (postViewModel.LinkedObjectType == LinkedObjectType.User)
            {
                canEdit = await _userOperations.CheckRights(postViewModel.LinkedObjectId, User.Identity.Name);
            }
            
            if (!canEdit) return this.Result403("You haven't rights to add files to this object");

            var file = Mapper.Map<File>(postViewModel);
            var result = await _fileOperations.Addfile(file, postViewModel.Data);
            return await Get(result.Code);
        }

        [HttpDelete]
        [RESTAuthorize]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var canEdit = await _fileOperations.CheckRights(id, User.Identity.Name);
            if (!canEdit) return this.Result403("You haven't rights to delete this file");

            var file = await _fileOperations.GetFile(id);
            if (file == null) return this.Result404("This file is not found");

            await _fileOperations.DeleteFile(id);
            return Ok("Deleted");
        }
    }
}
