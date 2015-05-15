using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Chame.WebService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            UnityConfig.RegisterComponents();

            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}
