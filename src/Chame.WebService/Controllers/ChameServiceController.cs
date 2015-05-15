using System;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.UI;

using log4net;
using Newtonsoft.Json;
using Chame.WebService;
using ChameHOT.WebService.Library.Models;
using Chame.WebService.Helper.Services;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using System.Web;

namespace Chame.WebService.Controllers
{
    /// <summary>
    /// Class that returns a Http response.
    /// </summary>
    [ChameExceptionFilter]
    [RoutePrefix("api")]
    public class ChameServiceController : ApiController
    {
        private readonly ILog logger;
        private readonly Dictionary<string, IChameService> services;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagesController"/> class.
        /// </summary>
        /// <param name="logger">a logger</param>
        /// <param name="arcadiaService">a Arcadia service component instance</param>
        public ChameServiceController(ILog logger)
        {
            var resolver = GlobalConfiguration.Configuration.DependencyResolver as Unity.WebApi.UnityDependencyResolver;
            services = resolver.GetServices(typeof(IChameService)).Cast<IChameService>().ToDictionary(s => s.ServiceName);
            this.logger = logger;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            foreach (var service in services)
            {
                service.Value.Dispose();
            }
        }

        /// <summary>
        /// POST api/GetChameImage?s={service name}.
        /// </summary>
        /// <param name="s">The Service Name</param>
        /// <returns>Response message</returns>
        [HttpPost]
        [Route("GetChameImage")]
        public async Task<HttpResponseMessage> GetChameHOTImage([FromUri]string s)
        {
            logger.Info(string.Format("Request for GetChameHOTImage {0}", s));

            if (!services.ContainsKey(s))
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            // get POST request body
            var body = await Request.Content.ReadAsStringAsync();
            logger.Info(string.Format("Request for GetChameHOTImage, body length: {0}", body.Length));

            // verify parameters
            if (string.IsNullOrEmpty(body))
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            // get service from all IoC container services
            var service = services[s];

            // process image by request body
            var responseContent = await service.ProcessImage(body);

            // create and return http response
            var response = new HttpResponseMessage() { StatusCode = responseContent.Status, Content = responseContent.Content };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(responseContent.MimeType);
            return response;
        }

        /// <summary>
        /// GET api/GetLog
        /// </summary>
        /// <returns>Response message</returns>
        [HttpGet, Route("GetLog")]
        public async Task<HttpResponseMessage> GetLog()
        {
            logger.Info("Request for GetLog");

            // create and return http response
            using (var fs = new FileStream(HttpContext.Current.Server.MapPath("~/App_Data/chame_log.log"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var sr = new StreamReader(fs))
                {
                    var log = sr.ReadToEnd();
                    var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(log) };
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                    return response;
                }
            }
        }

        /// <summary>
        /// GET api/GetCache?s={service name}
        /// </summary>
        /// <returns>Response message</returns>
        [HttpGet, Route("GetCache")]
        public async Task<HttpResponseMessage> GetCache([FromUri]string s)
        {
            logger.Info(string.Format("Request for GetCache {0}", s));

            if (!services.ContainsKey(s))
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            // get service from all IoC container services
            var service = services[s];

            var responseContent = await service.GetCache();

            // create and return http response
            var response = new HttpResponseMessage() { StatusCode = responseContent.Status, Content = responseContent.Content };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(responseContent.MimeType);
            return response;
        }

        /// <summary>
        /// GET api/GetCache?s={service name}&id={client id}
        /// </summary>
        /// <returns>Response message</returns>
        [HttpGet, Route("CleanCache")]
        public async Task<HttpResponseMessage> CleanCache([FromUri]string s, [FromUri]string id = null)
        {
            logger.Info(string.Format("Request for CleanCache {0}, id = {1}", s, id));

            if (!services.ContainsKey(s))
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            // get service from all IoC container services
            var service = services[s];

            var responseContent = await service.CleanCache(id);

            // create and return http response
            var response = new HttpResponseMessage() { StatusCode = responseContent.Status, Content = responseContent.Content };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(responseContent.MimeType);
            return response;
        }
    }
}
