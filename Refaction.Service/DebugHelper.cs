using Ninject;
using Refaction.Data;
using Refaction.Data.Entities;
using Refaction.Data.Fakes;
using Refaction.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Refaction.Service
{
    public static class DebugHelper
    {
        // create a fake DbContext for debugging (in-memory database)
        static FakeRefactionDbContext _db = new FakeRefactionDbContext(SampleData.ProductEntities, SampleData.ProductOptionEntities);

        /// <summary>
        /// Creates the Ninject kernel and bind it to the appropriate DbContext and repositories
        /// </summary>
        /// <returns></returns>
        public static void BindFakeDbContext(IKernel kernel)
        {
            kernel.Rebind<IRefactionDbContext>()
                .ToConstant(_db);
        }

        /// <summary>
        /// Entities for testing in-memory database (copied from testing layer)
        /// </summary>
        static class SampleData
        {
            internal static readonly ProductEntity[] ProductEntities = new ProductEntity[]
            {
                new ProductEntity
                {
                    Id = Guid.Parse("{F2A6002A-F657-4B8C-A520-AD745EA995B8}"),
                    DeliveryPrice = 0.00M,
                    Description = "The product with an index of zero",
                    Name = "Product 0",
                    Price = 0.00M
                },
                new ProductEntity
                {
                    Id = Guid.Parse("{590454AC-EC6F-4C4E-A41A-8A368E5C385B}"),
                    DeliveryPrice = 1.11M,
                    Description = "The product with an index of one",
                    Name = "Product 1",
                    Price = 1.11M
                },
                new ProductEntity
                {
                    Id = Guid.Parse("{2736EB73-C4AC-41B3-8722-2374DB229443}"),
                    DeliveryPrice = 2.22M,
                    Description = "The product with an index of two",
                    Name = "Product 2",
                    Price = 2.22M
                },
            };

            // To prevent modification of the sample entities, always return a copy of the underlying entities
            static ProductEntity ProductEntity0 { get { return FakeRefactionDbContext.CreateCopy(ProductEntities[0]); } }
            static ProductEntity ProductEntity1 { get { return FakeRefactionDbContext.CreateCopy(ProductEntities[1]); } }
            static ProductEntity ProductEntity2 { get { return FakeRefactionDbContext.CreateCopy(ProductEntities[2]); } }

            internal static readonly ProductOptionEntity[] ProductOptionEntities = new ProductOptionEntity[]
            {
                new ProductOptionEntity
                {
                    Id = Guid.Parse("{92FA19B7-AF7E-4B81-92B2-3E94B2624975}"),
                    Description = "The option with an index of zero",
                    Name = "Option 0",
                    ProductId = SampleData.ProductEntity0.Id,
                },
                new ProductOptionEntity
                {
                    Id = Guid.Parse("{E58562A6-D882-49E8-B21F-1DAE3BFC4416}"),
                    Description = "The option with an index of one",
                    Name = "Option 1",
                    ProductId = SampleData.ProductEntity1.Id,
                },
                new ProductOptionEntity
                {
                    Id = Guid.Parse("{4008FA16-3AB5-4647-814E-EFEB01080460}"),
                    Description = "The option with an index of two",
                    Name = "Option 2",
                    ProductId = SampleData.ProductEntity0.Id,
                },
                new ProductOptionEntity
                {
                    Id = Guid.Parse("{9B2CF5B0-A7B3-4CF6-B6B6-01580E908961}"),
                    Description = "The option with an index of three",
                    Name = "Option 3",
                    ProductId = SampleData.ProductEntity1.Id,
                },
            };
        }
    }
}