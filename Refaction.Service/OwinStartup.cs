using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Owin;
using Microsoft.Owin;
using System.Web.Http;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using System.Reflection;

[assembly: OwinStartup(typeof(Refaction.Service.OwinStartup))]

namespace Refaction.Service
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            // register routes and formatters
            WebApiConfig.Register(config);

            // enable Web API
            appBuilder.UseWebApi(config);

            // enable Ninject for Web API controllers
            appBuilder.UseNinjectMiddleware(CreateKernel).UseNinjectWebApi(config);
        }

        private static StandardKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            return kernel;
        }
    }
}