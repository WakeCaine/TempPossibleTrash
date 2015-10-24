using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ESO_Project.Startup))]
namespace ESO_Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
