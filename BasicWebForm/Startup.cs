using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BasicWebForm.Startup))]
namespace BasicWebForm
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
