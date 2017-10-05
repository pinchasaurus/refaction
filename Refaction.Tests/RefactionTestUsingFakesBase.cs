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
using Refaction.Service.Repositories;

namespace Refaction.UnitTests
{
    /// <summary>
    /// Creates an IRefactionDbContext for derived classes
    /// </summary>
    /// <remarks>
    /// Designed for use with integration tests (uses in-memory Fakes)
    /// </remarks>
    public abstract class RefactionTestUsingFakesBase : NinjectTestBase
    {
        public RefactionTestUsingFakesBase()
        {
            Refaction.Service.OwinStartup.NinjectKernel = this.NinjectKernel;
        }

        protected IRefactionDbContext CurrentDbContext
        {
            get { return NinjectKernel.Get<IRefactionDbContext>(); }
        }

        protected virtual void UseEmptyDatabase()
        {
            NinjectKernel.Rebind<IRefactionDbContext>()
                .To<FakeRefactionDbContext>()
                .InSingletonScope();

            RebindRepositories();
        }

        protected virtual void UseSampleDatabase()
        {
            NinjectKernel.Rebind<IRefactionDbContext>()
                .To<FakeRefactionDbContext>()
                .InSingletonScope()
                .WithConstructorArgument<IEnumerable<ProductEntity>>(SampleData.ProductEntities)
                .WithConstructorArgument<IEnumerable<ProductOptionEntity>>(SampleData.ProductOptionEntities)
                ;

            RebindRepositories();
        }

        public void RebindRepositories()
        {
            NinjectKernel
            .Rebind<IProductRepository>()
            .To<ProductRepository>()
            .InSingletonScope();

            NinjectKernel
            .Rebind<IProductOptionRepository>()
            .To<ProductOptionRepository>()
            .InSingletonScope();
        }
    }
}
