using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Ninject.Modules;
using Refaction.Data;
using Refaction.Data.Fakes;
using Refaction.Common;

namespace Refaction.Service
{
    public class RefactionNinjectModule : NinjectModule
    {
        public override void Load()
        {
            NinjectHelper.RemovePriorBindings(this.Kernel, typeof(IRefactionDbContext));

            this.Bind<IRefactionDbContext>().To<RefactionDbContext>();

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                NinjectHelper.RemovePriorBindings(this.Kernel, typeof(IRefactionDbContext));

                this.Bind<IRefactionDbContext>().To<FakeRefactionDbContext>();
            }
#endif

        }
    }
}