using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using API.Common;
using API.Models;
using API.Operations;
using API.ViewModels;
using AutoMapper;
using Camps.Tools;
using Models;
using Models.Entities;
using Models.Operations;

namespace API.Controllers
{
    /// <summary>
    /// Управление пользователями
    /// </summary>
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private readonly UserOperations _userOperations;
        private readonly FileOperations _fileOperations;
        private readonly OrderOperations _orderOperations;

        public UserController(UserOperations userOperations, 
            FileOperations fileOperations, OrderOperations orderOperations)
        {
            _userOperations = userOperations;
            _fileOperations = fileOperations;
            _orderOperations = orderOperations;
        }

        /// <summary>
        /// Получает пользователя по его email
        /// </summary>
        /// <param name="email"></param>
        [HttpGet]
        [ResponseType(typeof(UserViewModelGet))]
        public async Task<IHttpActionResult> Get(string email)
        {
            try
            {
                var entity = await _userOperations.GetAsync(email);
                if (entity == null)
                {
                    return new NotFoundResult(new HttpRequestMessage
                        { Content = new StringContent("User is not found") });
                }
                var result = Mapper.Map<UserViewModelGet>(entity);
                result.RoleName = entity.Role.ToString();
                var pic = (await _fileOperations.GetFilesByLinkedObject(entity.Id, LinkedObjectType.User)).FirstOrDefault();
                if (pic != null)
                {
                    result.AvatarUrl = Url.Content($"~/api/file/data/{pic.Code}");
                    result.AvatarUrlThumb = Url.Content($"~/api/file/data/{pic.Code}?thumb=true");
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                ErrorLogger.Log("CANNOT GET USER", e);
                throw;
            }
            
        }

        /// <summary>
        /// Обновляет данные пользователя
        /// </summary>
        /// <param name="email"></param>
        /// <param name="putViewModel"></param>
        [HttpPut]
        [ResponseType(typeof(UserViewModelGet))]
        [RESTAuthorize]
        public async Task<IHttpActionResult> Put([FromUri] string email, [FromBody] UserViewModelPut putViewModel)
        {
            if ((!User.IsInRole("PortalAdmin")) && ((!User.IsInRole("PortalManager")))  
                  && (User.Identity.Name!=email))
            {
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent("You cannot edit another users")
                });

            }
            var userEntity = Mapper.Map<User>(putViewModel);
            var userId = (await _userOperations.GetAsync(email))?.Id;
            if (!userId.HasValue)
            {
                return this.Result404("User is not found");
            }

            userEntity.Id = userId.Value;
            userEntity = await _userOperations.UpdateAsync(userEntity);
            return await Get(userEntity.Email);
        }


        /// <summary>
        /// Удаляет пользователя
        /// </summary>
        /// <param name="email"></param>
        [HttpDelete]
        [RESTAuthorize]
        [Route("delete")]
        public async Task<IHttpActionResult> Delete(string email)
        {
            if (!User.IsInRole("PortalAdmin") 
                && !User.IsInRole("PortalManager")
                && User.Identity.Name!=email)
            {
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent("You cannot delete another users")
                });
            }
            await _userOperations.DeleteAsync(email);
            return Ok("Deleted "+email);
        }

        /// <summary>
        /// Регистрирует пользователя
        /// </summary>
        /// <param name="viewModel"></param>
        [HttpPost]
        public async Task<IHttpActionResult> Register(UserRegisterViewModel viewModel)
        {
            var user = await _userOperations.RegisterAsync(viewModel.Email);

            return await Get(user.Email);
        }

        /// <summary>
        /// Получает данные пользователя по его токену в заголовке
        /// </summary>
        /// <returns></returns>
        [RESTAuthorize]
        [ResponseType(typeof(UserViewModelGet))]
        [HttpGet]
        public async Task<IHttpActionResult> GetMe()
        {
            return await Get(User.Identity.Name);
        }

        /// <summary>
        /// Ищет пользователей по электропочте, имени или номеру телефона.
        /// </summary>
        /// <param name="roles">Роли списком. Например: roles=0&amp;roles=2</param>
        /// <param name="cityId">Id города</param>
        /// <param name="word">Поисковое слово. Можно не передавать, тогда выдаст всех</param>
        /// <param name="page">Номер страницы для постраничной навигации. По-умолчанию — 1</param>
        /// <returns></returns>
        [HttpGet]
        [RESTAuthorize(Role.PortalAdmin, Role.PortalManager)]
        [ResponseType(typeof(IEnumerable<UserViewModelGet>))]
        [Route("search")]
        public async Task<IHttpActionResult> Search([FromUri]List<Role> roles, string word="", int page=1)
        {
            var users = await _userOperations.SearchAsync(roles, word, page);
            var result = new List<UserViewModelGet>();

            foreach (var user in users)
            {
                var userViewModel = Mapper.Map<UserViewModelGet>(user);
                userViewModel.RoleName = user.Role.ToString();

                result.Add(userViewModel);
            }
            return Ok(result);
        }

        /// <summary>
        /// Выход пользователя со всех устройств. Убивает предыдущий токен и создаёт новый. 
        /// Чтобы получить новый токен нужно вызвать метод Register
        /// </summary>
        [HttpPut]
        [RESTAuthorize]
        [Route("logout")]
        public async Task<IHttpActionResult> Logout()
        {
            await _userOperations.ResetTokenAsync(User.Identity.Name);
            return Ok("Logout succeeded");
        }

    }
}
