using Refaction.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refaction.Tests
{
    /// <summary>
    /// Entities for testing in-memory database
    /// </summary>
    public static class SampleData
    {
        public static readonly ProductEntity[] ProductEntities = new ProductEntity[]
        {
            new ProductEntity
            {
                Id = Guid.Parse("{F2A6002A-F657-4B8C-A520-AD745EA995B8}"),
                DeliveryPrice = 1.11M,
                Description = "The product with an index of zero",
                Name = "Product 0",
                Price = 1.11M
            },
            new ProductEntity
            {
                Id = Guid.Parse("{590454AC-EC6F-4C4E-A41A-8A368E5C385B}"),
                DeliveryPrice = 2.22M,
                Description = "The product with an index of one",
                Name = "Product 1",
                Price = 2.2M
            }
        };

        public static readonly ProductOptionEntity[] ProductOptionEntities = new ProductOptionEntity[] 
        {
            new ProductOptionEntity
            {
                Id = Guid.Parse("{92FA19B7-AF7E-4B81-92B2-3E94B2624975}"),
                Description = "The option with an index of zero",
                Name = "Option 0",
                ProductId = Guid.Parse("{F2A6002A-F657-4B8C-A520-AD745EA995B8}")
            },
            new ProductOptionEntity
            {
                Id = Guid.Parse("{E58562A6-D882-49E8-B21F-1DAE3BFC4416}"),
                Description = "The option with an index of one",
                Name = "Option 1",
                ProductId = Guid.Parse("{590454AC-EC6F-4C4E-A41A-8A368E5C385B}"),
            }
        };

        public static readonly ProductEntity ProductEntity0 = ProductEntities[0];
        public static readonly ProductEntity ProductEntity1 = ProductEntities[1];

        public static readonly ProductOptionEntity ProductOptionEntity0 = ProductOptionEntities[0];
        public static readonly ProductOptionEntity ProductOptionEntity1 = ProductOptionEntities[1];
    }
}
