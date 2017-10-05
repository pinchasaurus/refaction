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
using Refaction.UnitTests.Mocks;
using Refaction.Service.Repositories;

namespace Refaction.UnitTests
{
    /// <summary>
    /// Creates a Mock data layer for derived classes
    /// </summary>
    public abstract class RefactionTestUsingMocksBase : NinjectTestBase
    {
        public MockRefactionDbContext MockRefactionDbContext;

        public RefactionTestUsingMocksBase()
        {
            Refaction.Service.OwinStartup.NinjectKernel = this.NinjectKernel;
        }

        protected virtual void UseEmptyDatabase()
        {
            this.MockRefactionDbContext = new MockRefactionDbContext();

            NinjectKernel
            .Rebind<IRefactionDbContext>()
            .ToConstant(this.MockRefactionDbContext.Object)
            .InSingletonScope();
        }

        protected virtual void UseSampleDatabase()
        {
            this.MockRefactionDbContext = new MockRefactionDbContext(SampleData.ProductEntities, SampleData.ProductOptionEntities);

            NinjectKernel
            .Rebind<IRefactionDbContext>()
            .ToConstant(this.MockRefactionDbContext.Object)
            .InSingletonScope();
        }

        protected MockQueryProvider<ProductEntity> MockProductQueryProvider
        {
            get
            {
                return this.MockRefactionDbContext.MockProducts.MockQueryable.MockQueryProvider;
            }
        }

        protected MockQueryProvider<ProductOptionEntity> MockProductOptionQueryProvider
        {
            get
            {
                return this.MockRefactionDbContext.MockProductOptions.MockQueryable.MockQueryProvider;
            }
        }
    }
}
