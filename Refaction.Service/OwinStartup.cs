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