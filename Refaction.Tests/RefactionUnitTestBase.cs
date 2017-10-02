using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ninject;
using Ninject.Parameters;

using Refaction.Data;
using Refaction.Tests;
using Refaction.Data.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refaction.Data.Entities;

namespace Refaction.UnitTests
{
    public abstract class RefactionUnitTestBase : NinjectUnitTestBase
    {
        protected IRefactionDbContext CurrentDbContext
        {
            get { return CurrentNinjectKernel.Get<IRefactionDbContext>(); }
        }

        public void UseEmptyDatabase()
        {
            RemovePriorBindings(typeof(IRefactionDbContext));

            CurrentNinjectKernel.Bind<IRefactionDbContext>()
                .To<FakeRefactionDbContext>()
                .InSingletonScope();
        }

        protected void UseDatabaseWithSampleData()
        {
            RemovePriorBindings(typeof(IRefactionDbContext));

            CurrentNinjectKernel.Bind<IRefactionDbContext>()
                .To<FakeRefactionDbContext>()
                .InSingletonScope()
                .WithConstructorArgument<IEnumerable<ProductEntity>>(SampleData.ProductEntities)
                .WithConstructorArgument<IEnumerable<ProductOptionEntity>>(SampleData.ProductOptionEntities)
                ;
        }
    }
}
