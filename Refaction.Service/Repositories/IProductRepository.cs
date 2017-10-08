using Refaction.Common.Patterns;
using Refaction.Data.Entities;
using Refaction.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Refaction.Service.Repositories
{
    public interface IProductRepository : IRepository<Product, Guid>, IDisposable
    {
        IEnumerable<Product> Retrieve(string name);
    }
}
