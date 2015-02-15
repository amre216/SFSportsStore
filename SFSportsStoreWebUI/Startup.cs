using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SFSportsStoreWebUI.Startup))]
namespace SFSportsStoreWebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
