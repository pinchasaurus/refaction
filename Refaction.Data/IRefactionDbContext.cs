using Refaction.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refaction.Data
{
    public interface IRefactionDbContext : IDbContext, IDisposable
    {
        IDbSet<ProductEntity> ProductEntities { get; set; }

        IDbSet<ProductOptionEntity> ProductOptionEntities { get; set; }
    }
}
