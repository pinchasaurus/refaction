using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;

using Refaction.Data;
using Refaction.Data.Entities;
using Refaction.Data.Fakes;

namespace Refaction.UnitTests.Mocks
{
    /// <summary>
    /// Strict mock for IRefactionDbContext
    /// </summary>
    public class MockRefactionDbContext : Mock<IRefactionDbContext>
    {
        public readonly MockDbSet<ProductEntity> MockProducts;
        public readonly MockDbSet<ProductOptionEntity> MockProductOptions;

        public MockRefactionDbContext()
            : this(new ProductEntity[] { }, new ProductOptionEntity[] { } )
        {
        }

        public MockRefactionDbContext(IEnumerable<ProductEntity> productEntities, IEnumerable<ProductOptionEntity> productOptionEntities)
            : base(MockBehavior.Strict)
        {
            var copyOfProductEntities = productEntities.Select(productEntity => FakeRefactionDbContext.CreateCopy(productEntity));
            var copyOfProductOptionEntities = productOptionEntities.Select(productOptionEntity => FakeRefactionDbContext.CreateCopy(productOptionEntity));

            MockProducts = new MockDbSet<ProductEntity>(copyOfProductEntities);
            MockProductOptions = new MockDbSet<ProductOptionEntity>(copyOfProductOptionEntities);

            this.Setup(m => m.ProductEntities).Returns(MockProducts.Object);
            this.Setup(m => m.ProductOptionEntities).Returns(MockProductOptions.Object);
        }

        public void VerifyThisAndNestedMocks()
        {
            this.Verify();
            MockProducts.VerifyThisAndNestedMocks();
            MockProductOptions.VerifyThisAndNestedMocks();
        }
    }
}
