using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;

using Refaction.Data.Entities;

namespace Refaction.Data
{
    public interface IRefactionDbContext : IDbContext, IDisposable
    {
        IDbSet<ProductEntity> ProductEntities { get; set; }

        IDbSet<ProductOptionEntity> ProductOptionEntities { get; set; }
    }
}
