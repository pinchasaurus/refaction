using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using System.Threading.Tasks;
using System.Reflection;

using System.Web.Cors;
using System.Web.Http;

using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Cors;

using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;

using NSwag.AspNet.Owin;

using Refaction.Common;
using Refaction.Data;
using Refaction.Data.Entities;
using Refaction.Data.Fakes;
using Refaction.Service.Repositories;

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

            EnableCors(appBuilder);

            EnableNinject(appBuilder, config);

            EnableSwaggerUi(appBuilder);

            // always do this last
            appBuilder.UseWebApi(config);
        }

        /// <summary>
        /// Enable Cross-Origin Resource Sharing
        /// </summary>
        void EnableCors(IAppBuilder appBuilder)
        {
            var corsOptions = GetCorsOptions();
            appBuilder.UseCors(corsOptions);
        }

        /// <summary>
        /// Enable depedency injection using Ninject
        /// </summary>
        private void EnableNinject(IAppBuilder appBuilder, HttpConfiguration config)
        {
            // enable Ninject for Web API controllers
            appBuilder.UseNinjectMiddleware(GetOrCreateKernel);
            appBuilder.UseNinjectWebApi(config);
        }

        /// <summary>
        /// Enable Swagger UI using NSwag
        /// </summary>
        /// <param name="appBuilder"></param>
        private void EnableSwaggerUi(IAppBuilder appBuilder)
        {
            var swaggerUiSettings =
                new SwaggerUiSettings()
                {
                    Title = "Refaction",
                    Description = "Refaction Web API Service",
                    DocExpansion = "list",
                    DefaultUrlTemplate = "/products/{productId}/productOption/{productOptionId}",
                };

            appBuilder.UseSwaggerUi(this.GetType().Assembly, swaggerUiSettings);
        }

        static IKernel GetOrCreateKernel()
        {
            // During testing, the Ninject kernel will be assigned by the test layer.
            // If the Ninject kernel has not been assigned, then we need to create one.
            if (NinjectKernel == null)
            {
                NinjectKernel = CreateNinjectKernel();
            }

            return NinjectKernel;
        }

        /// <summary>
        /// Creates the Ninject kernel and bind it to the appropriate DbContext and repositories
        /// </summary>
        /// <returns></returns>
        static IKernel CreateNinjectKernel()
        {
            var result = new StandardKernel();
            result.Load(Assembly.GetExecutingAssembly());

            if (System.Diagnostics.Debugger.IsAttached)
            {
                DebugHelper.BindFakeDbContext(result);
            }
            else
            {
                result.Rebind<IRefactionDbContext>()
                    .To<RefactionDbContext>();
            }

            result.Rebind<IProductRepository>()
                .To<ProductRepository>();

            result.Rebind<IProductOptionRepository>()
                .To<ProductOptionRepository>();

            return result;
        }

        CorsOptions GetCorsOptions()
        {
            var corsPolicy = new CorsPolicy
            {
                AllowAnyMethod = true,
                AllowAnyHeader = true
            };

            const string originsAppSettingKey = "System.Web.Cors.CorsPolicy.Origins";

            var origins = ConfigurationManager.AppSettings[originsAppSettingKey];

            if (origins == null)
            {
                corsPolicy.AllowAnyOrigin = true;
            }
            else
            {
                var originsArray = origins.Split(';');

                foreach (var origin in originsArray)
                {
                    corsPolicy.Origins.Add(origin.Trim());
                }
            }

            var corsOptions = new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = request => Task.FromResult(corsPolicy)
                }
            };

            return corsOptions;
        }

    }



}