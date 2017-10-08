using Refaction.Data;
using Refaction.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Refaction.Data.Fakes
{
    /// <summary>
    /// An in-memory DbContext for testing Entity Framework per https://romiller.com/2012/02/14/testing-with-a-fake-dbcontext
    /// </summary>
    public class FakeRefactionDbContext : IRefactionDbContext
    {
        static readonly IEnumerable<ProductEntity> _emptyProductArray = new ProductEntity[] { };
        static readonly IEnumerable<ProductOptionEntity> _emptyProductOptionArray = new ProductOptionEntity[] { };

        public FakeRefactionDbContext()
            : this(_emptyProductArray, _emptyProductOptionArray)
        {
        }

        public FakeRefactionDbContext(IEnumerable<ProductEntity> productEntities, IEnumerable<ProductOptionEntity> productOptionEntities)
        {
            var copyOfProductEntities = productEntities.Select(productEntity => CreateCopy(productEntity));
            var copyOfProductOptionEntities = productOptionEntities.Select(productOptionEntity => CreateCopy(productOptionEntity));

            ProductEntities = new FakeProductDbSet(copyOfProductEntities);
            ProductOptionEntities = new FakeProductOptionDbSet(copyOfProductOptionEntities);
        }

        public static ProductEntity CreateCopy(ProductEntity other)  
        {
            // Entities should not have a copy constructor, but we need to create copies of sample entities during testing, so do it here.
            return new ProductEntity
            {
                DeliveryPrice = other.DeliveryPrice,
                Description = other.Description,
                Id = other.Id,
                Name = other.Name,
                Price = other.Price,
            };
        }

        public static ProductOptionEntity CreateCopy(ProductOptionEntity other)
        {
            // Entities should not have a copy constructor, but we need to create copies of sample entities during testing, so do it here.
            return new ProductOptionEntity
            {
                Description = other.Description,
                Id = other.Id,
                Name = other.Name,
                ProductId = other.ProductId,
            };
        }

        public IDbSet<ProductEntity> ProductEntities { get; set; }

        public IDbSet<ProductOptionEntity> ProductOptionEntities { get; set; }

        public int SaveChanges()
        {
            return 0;
        }

        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult(0);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            // do nothing, allow derived classes to override Dispose()
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~FakeRefactionDbContext()
        {
            Dispose(false);
        }

        #endregion
    }
}
