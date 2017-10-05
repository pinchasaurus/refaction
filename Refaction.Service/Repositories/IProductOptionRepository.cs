using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Refaction.Common.Patterns;
using Refaction.Service.Models;
using System.Linq.Expressions;
using Refaction.Data.Entities;

namespace Refaction.Service.Repositories
{
    public interface IProductOptionRepository : IRepository<ProductOption, Guid>, IDisposable
    {
        IEnumerable<ProductOption> RetrieveByProductId(Guid id);

        ProductOption RetrieveByBothIds(Guid productId, Guid id);
    }
}
