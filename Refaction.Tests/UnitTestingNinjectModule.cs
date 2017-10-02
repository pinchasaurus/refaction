using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Refaction.Data;
using Refaction.Data.Fakes;
using Ninject.Modules;

namespace Refaction.UnitTests
{
    public class RefactionNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IRefactionDbContext>().To<FakeRefactionDbContext>();
        }
    }
}
