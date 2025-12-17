using AutoMapper;
using SmartFlashcards_API.App_Start;
using System.Web.Http;

namespace SmartFlashcards_API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static IMapper MapperInstance;

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ApiMappingProfile>();
            });

            MapperInstance = config.CreateMapper();
        }
    }
}
