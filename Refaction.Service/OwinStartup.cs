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
using Refaction.Data;
using Refaction.Common;
using Refaction.Data.Fakes;

[assembly: OwinStartup(typeof(Refaction.Service.OwinStartup))]

namespace Refaction.Service
{
    /// <summary>
    /// Startup class for OWIN server
    /// </summary>
    public class OwinStartup
    {
        public static IKernel NinjectKernel;

        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            // register routes and formatters
            WebApiConfig.Register(config);

            // enable Ninject for Web API controllers
            appBuilder.UseNinjectMiddleware(GetOrCreateKernel);
            appBuilder.UseNinjectWebApi(config);

            // always do this last
            appBuilder.UseWebApi(config);
        }

        private static IKernel GetOrCreateKernel()
        {
            // During testing, the Ninject kernel will be assigned by the test layer.
            // If the Ninject kernel has not been assigned, then we need to create one.
            if (NinjectKernel == null)
            {
                var kernel = new StandardKernel();
                kernel.Load(Assembly.GetExecutingAssembly());

                NinjectKernel = kernel;
            }

            return NinjectKernel;
        }
    }
}