using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MpangazithaBash.Startup))]
namespace MpangazithaBash
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
