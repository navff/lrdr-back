using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using API.Common;
using API.Models;
using API.Operations;
using Models;

namespace API.Common
{
    /// <summary>
    /// Аффтаризовывает пользователя по токену в http-заголовке «Authorization»
    /// </summary>
    public class RESTAuthorizeAttribute : AuthorizationFilterAttribute, IDisposable
    {
        private string[] _roles;
        private LrdrContext _context = new LrdrContext();
        private UserOperations _userOperations;

        /// <summary>
        /// Позволяет заходить всем зарегистрированным пользователям
        /// </summary>
        public RESTAuthorizeAttribute() : this(Role.PortalAdmin, Role.PortalManager, Role.RegisteredUser)
        {
        }

        /// <summary>
        /// Аффтаризация. Принимает массив ролей в строках
        /// </summary>
        /// <param name="roles"></param>
        public RESTAuthorizeAttribute(params  string[] roles)
        {
            this._roles = roles;
            _userOperations = new UserOperations(_context);
        }

        /// <summary>
        /// /// Аффтаризация. Принимает массив ролей в енумах
        /// </summary>
        /// <param name="roles"></param>
        public RESTAuthorizeAttribute(params Role[] roles)
        {
            var rolesList = new List<string>();
            foreach (Role role in roles)
            {
                rolesList.Add(role.ToString());
            }
            this._roles = rolesList.ToArray();
            _userOperations = new UserOperations(_context);
        }


        /// <summary>
        /// Вызывается каждый раз при поступлении запроса
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var token = actionContext.Request.Headers.Authorization?.Parameter;
            if (token == null)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent("Token is null")
                };
                return;
            }

            var user = _userOperations.GetUserByToken(token);

            if (user==null) 
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent("User with this token is not found")
                };
                return;
            }

            if (_roles != null)
            {
                if (!_roles.Contains(user.Role.ToString()))
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden)
                    {
                        Content = new StringContent("Your role is too small")
                    };
                    return;
                }
            }

            var identity = new Identity {Name = user.Email, IsAuthenticated = true};
            actionContext.RequestContext.Principal = new GenericPrincipal(identity, new[] { user.Role.ToString()});
        }

        /// <summary>
        /// Убивает связанные ресурсы
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}