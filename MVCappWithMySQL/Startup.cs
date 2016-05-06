using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCappWithMySQL.Startup))]
namespace MVCappWithMySQL
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
