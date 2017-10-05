using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Ninject.Modules;
using Refaction.Data;
using Refaction.Data.Fakes;
using Refaction.Common;
using Refaction.Service.Repositories;

namespace Refaction.Service
{
    /// <summary>
    /// Initializes Ninject kernel
    /// </summary>
    public class RefactionNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Rebind<IRefactionDbContext>()
                .To<RefactionDbContext>();

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.Rebind<IRefactionDbContext>()
                    .To<FakeRefactionDbContext>();
            }
#endif

            this.Rebind<IProductRepository>()
                .To<ProductRepository>();

            this.Rebind<IProductOptionRepository>()
                .To<ProductOptionRepository>();
        }
    }
}