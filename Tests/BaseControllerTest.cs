using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using API;
using API.App_Start;
using API.Common;
using API.ViewModels;
using Camps.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject.Web.Common;
using NLog;
using UrlHelper = System.Web.Http.Routing.UrlHelper;

namespace Tests
{
   public abstract class BaseControllerTest : BaseTest
    {
        private string baseAddress = "http://localhost:3106/";
        private Logger _logger;

        public BaseControllerTest()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        protected void PrepareController(ApiController controller, string userName = null, string role=null)
        {
            HttpConfiguration config = new HttpConfiguration();

            var route = config.Routes.MapHttpRoute(
               name: "DefaultApi",
               routeTemplate: "api/{controller}/{id}",
               defaults: new { id = RouteParameter.Optional }
           );

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            var request = new HttpRequestMessage(HttpMethod.Post, baseAddress);
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "products" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            if (role != null)
            {
                Identity identity = new Identity
                {
                    Name = userName,
                    IsAuthenticated = true
                };

                controller.ControllerContext.RequestContext.Principal = 
                    new GenericPrincipal(identity, new string[] { role } );
            }

            controller.Url = new UrlHelper(new HttpRequestMessage());
            controller.Url.Request = request;
        }



        /// <summary>
        /// Отправляет HTTP-запрос на in-memory http-сервер
        /// </summary>
        /// <typeparam name="T">Возвращаемый тип объекта</typeparam>
        /// <param name="url">Относительный урл. Например: api/user?email=var@33kita.ru</param>
        /// <param name="token">Токен аутентификации</param>
        /// <returns></returns>
        public T HttpGet<T>(string url, string token = "")
        {
            return GetOrDeleteRequest<T>(url, HttpMethod.Get, token);
        }



        /// <summary>
        /// Отправляет DELETE-запрос на указанный урл
        /// </summary>
        /// <typeparam name="T">Возвращаемый тип объекта</typeparam>
        /// <param name="url">Относительный урл. Например: api/user?email=var@33kita.ru</param>
        /// <param name="token">Токен аутентификации</param>
        public T HttpDelete<T>(string url, string token = "")
        {
            return GetOrDeleteRequest<T>(url, HttpMethod.Delete, token);
        }



        /// <summary>
        /// Отправляет PUT-запрос на указанный урл
        /// </summary>
        /// <typeparam name="T">Возвращаемый тип объекта</typeparam>
        /// <param name="url">Относительный урл. Например: api/user?email=var@33kita.ru</param>
        /// <param name="objectForSend">Отправляемый объект</param>
        /// <param name="token">Токен аутентификации</param>
        public T HttpPut<T>(string url, object objectForSend, string token = "")
        {
            return PostOrPutRequest<T>(url, objectForSend, HttpMethod.Put, token);
        }



        /// <summary>
        /// Отправляет POST-запрос на указанный урл
        /// </summary>
        /// <typeparam name="T">Возвращаемый тип объекта</typeparam>
        /// <param name="url">Относительный урл. Например: api/user?email=var@33kita.ru</param>
        /// <param name="objectForSend">Отправляемый объект</param>
        /// <param name="token">Токен аутентификации</param>
        public T HttpPost<T>(string url, object objectForSend, string token = "")
        {
            return PostOrPutRequest<T>(url, objectForSend, HttpMethod.Post, token);
        }


        private T PostOrPutRequest<T>(string url, object objectForSend, HttpMethod method, string token = "")
        {
            HttpMessageInvoker messageInvoker = new HttpMessageInvoker(new InMemoryHttpContentSerializationHandler(PrepareServer()));
            HttpRequestMessage request = new HttpRequestMessage();

            var json = "";
            if (objectForSend != null)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                json = js.Serialize(objectForSend);
            }

            request.Content = new StringContent(json);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.RequestUri = new Uri(baseAddress + url);
            Console.WriteLine(request.RequestUri);
            Console.WriteLine(json);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("Authorization", "Token " + token);
            request.Method = method;

            return SendRequest<T>(request, messageInvoker);
        }

        private T GetOrDeleteRequest<T>(string url, HttpMethod method, string token ="")
        {
            HttpMessageInvoker messageInvoker = new HttpMessageInvoker(new InMemoryHttpContentSerializationHandler(PrepareServer()));

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(baseAddress + url);
            Console.WriteLine(request.RequestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("Authorization", "Token " + token);
            request.Method = method;

            return SendRequest<T>(request, messageInvoker);
        }


        private T SendRequest<T>(HttpRequestMessage request, HttpMessageInvoker messageInvoker)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            _logger.Info(request.Method + ": " + request.RequestUri.AbsoluteUri);
            Debug.WriteLine(request.Method + ": " + request.RequestUri.AbsoluteUri);

            using (HttpResponseMessage response = messageInvoker.SendAsync(request, cts.Token).Result)
            {
                //Assert.IsNotNull(response.Content);
                if (response.IsSuccessStatusCode)
                {
                    if (response.Content.Headers.ContentType.MediaType.Contains("image"))
                    {
                        return default(T);
                    }
                    return response.Content.ReadAsAsync<T>().Result;
                }
                else
                {
                    HandleError(response);
                    return default(T);
                }
            }
        }

        private void HandleError(HttpResponseMessage response)
        {
            string message;

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                try
                {
                    HttpError httpError = response.Content.ReadAsAsync<HttpError>().Result;
                    string inner = httpError.ContainsKey("InnerException") ? httpError["InnerException"].ToString() : "";

                    message = httpError.ExceptionMessage + " " 
                            + httpError.Message + " " 
                            + httpError.StackTrace + " " 
                            + httpError.InnerException?.Message
                            + inner;
                    throw new Exception(message);
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    ErrorLogger.Log(message, ex);
                    throw;
                }
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                message = $"Status code: {response.StatusCode}.";
            }
            else if (response.StatusCode == HttpStatusCode.Gone)
            {
                message = $"Status code: {response.StatusCode}.";
            }
            else
            {
                message = response.Content.ReadAsStringAsync().Result;
            }
            Debug.WriteLine(message);
            var exception = new WebException(message);
            ErrorLogger.Log("ERROR IN HTTP REQUEST", exception);
            throw exception;


        }

        private HttpServer PrepareServer()
        {
            HttpConfiguration config = new HttpConfiguration();

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Initialize(NinjectWebCommon.CreateKernel);
            config.DependencyResolver = NinjectWebCommon.GetResolver();
            WebApiConfig.Register(config);
            config.EnsureInitialized();

            return new HttpServer(config);
        }
    }
}
