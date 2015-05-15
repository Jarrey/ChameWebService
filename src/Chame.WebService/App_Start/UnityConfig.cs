using log4net;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Microsoft.Practices.Unity.Configuration;
using Unity.WebApi;

namespace Chame.WebService
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.LoadConfiguration("ChameContainer");

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // register ILog instance
            log4net.Config.XmlConfigurator.Configure();
            ILog logger = LogManager.GetLogger("ImagesService");
            container.RegisterInstance(typeof(ILog), logger, new ContainerControlledLifetimeManager());
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}