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
    class FakeProductOptionDbSet : FakeDbSet<ProductOptionEntity>
    {
        public FakeProductOptionDbSet()
        {
        }

        public FakeProductOptionDbSet(IEnumerable<ProductOptionEntity> productOptionEntities)
         : base(productOptionEntities)
        {
        }

        public override ProductOptionEntity Find(params object[] keyValues)
        {
            return Local.SingleOrDefault(productOption => productOption.Id == (Guid)keyValues[0]);
        }
    }
}
