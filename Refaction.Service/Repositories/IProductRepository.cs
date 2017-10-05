using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Refaction.Common.Patterns;
using Refaction.Service.Models;
using Refaction.Data.Entities;
using System.Linq.Expressions;

namespace Refaction.Service.Repositories
{
    public interface IProductRepository : IRepository<Product, Guid>, IDisposable
    {
        IEnumerable<Product> Retrieve(string name);
    }
}
