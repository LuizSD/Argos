using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ArduinoSignalR.Startup))]
namespace ArduinoSignalR
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}