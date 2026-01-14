using AutoMapper;
using SmartFlashcards_API.App_Start;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SmartFlashcards_API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static IMapper MapperInstance;

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ApiMappingProfile>();
            });

            MapperInstance = config.CreateMapper();

        }

    }
}
