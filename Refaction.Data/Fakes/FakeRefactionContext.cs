using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;

using Refaction.Data;
using Refaction.Data.Entities;
using System.Data.Entity.Core.Objects;
using System.Threading;

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
            ProductEntities = new FakeProductDbSet(productEntities);
            ProductOptionEntities = new FakeProductOptionDbSet(productOptionEntities);
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
