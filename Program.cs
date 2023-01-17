using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Web.Http;

namespace DataAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            UI.InitUserDialogue();
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        
    }
    public static class WebApiConfig
    {
        public static string? DBConnectionString { get; set; }
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }

}
