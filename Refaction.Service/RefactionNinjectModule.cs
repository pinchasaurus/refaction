using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Ninject.Modules;
using Refaction.Data;
using Refaction.Data.Fakes;

namespace Refaction.Service
{
    public class RefactionNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IRefactionDbContext>().To<RefactionDbContext>();

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.Bind<IRefactionDbContext>().To<FakeRefactionDbContext>();
            }
#endif

        }
    }
}