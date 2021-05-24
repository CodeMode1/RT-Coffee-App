using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(RT_Coffee_App.Startup))]

namespace RT_Coffee_App
{
    public class Startup
    {
        // Method called when app starts
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            var hubConfig = new HubConfiguration();
            app.MapSignalR("/Coffee/signalr", hubConfig);

            // Deny all users that are not authorized, but here for all hubs :
            // Without the Authorize attribute
            // GlobalHost.HubPipeline.RequireAuthentication();
        }
    }
}
