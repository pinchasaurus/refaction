using Refaction.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refaction.Data.Fakes
{
    /// <summary>
    /// An in-memory DbSet for testing Entity Framework per https://romiller.com/2012/02/14/testing-with-a-fake-dbcontext
    /// </summary>
    class FakeProductDbSet : FakeDbSet<ProductEntity>
    {
        public FakeProductDbSet()
        {
        }

        public FakeProductDbSet(IEnumerable<ProductEntity> productEntities)
         : base(productEntities)
        {
        }

        public override ProductEntity Find(params object[] keyValues)
        {
            return Local.SingleOrDefault(product => product.Id == (Guid)keyValues[0]);
        }
    }
}
