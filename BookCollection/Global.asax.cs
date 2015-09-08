using BookCollection.DAL;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BookCollection
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Could also be in DbConfiguration extending class
            // Resilience: search on 'throw' and retry policy is tested
            //DbInterception.Add(new BookInterceptorTransientErrors());

            // Logging to debug trace window
            DbInterception.Add(new BookInterceptorLogging());

            // Initialize IoC container/Unity
            UnityBootstrapper.Initialise();
            // Register our custom controller factory
            ControllerBuilder.Current.SetControllerFactory(typeof(ControllerFactory));
        }
    }

    public class UnityBootstrapper
    {
        // SOURCE: http://www.codeproject.com/Articles/797132/Dependency-Injection-in-MVC-Using-Unity-IoC-Contai

        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            return container;
        }
        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<IBookContext, BookContext>();
            //container.RegisterType<ILogger, FakeLogger>();
            MvcUnityContainer.Container = container;
            return container;
        }
    }

    public class ControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            try
            {
                if (controllerType == null)
                    throw new ArgumentNullException("controllerType");

                if (!typeof(IController).IsAssignableFrom(controllerType))
                    throw new ArgumentException(string.Format(
                        "Type requested is not a controller: {0}",
                        controllerType.Name),
                        "controllerType");

                return MvcUnityContainer.Container.Resolve(controllerType) as IController;
            }
            catch
            {
                return null;
            }

        }
    }

    public static class MvcUnityContainer
    {
        public static UnityContainer Container { get; set; }
    }
}
