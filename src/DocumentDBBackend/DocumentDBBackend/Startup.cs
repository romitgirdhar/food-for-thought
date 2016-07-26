using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DocumentDBBackend.Startup))]

namespace DocumentDBBackend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}