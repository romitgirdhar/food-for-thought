using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(fft_mobileapp.Startup))]

namespace fft_mobileapp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}