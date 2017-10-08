using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ninject.Parameters;
using Refaction.Data;
using Refaction.Data.Entities;
using Refaction.Data.Fakes;
using Refaction.Service.Repositories;
using Refaction.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
