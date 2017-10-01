using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using Refaction.Data.Entities;

namespace Refaction.Data
{
    public partial class RefactionDbContext : DbContext, IRefactionDbContext
    {
        public RefactionDbContext()
            : base("name=RefactionDatabase")
        {
            Database.SetInitializer<RefactionDbContext>(null);

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.Database.Log = (text) => System.Diagnostics.Debug.WriteLine(text);
            }
#endif
        }

        public virtual IDbSet<ProductEntity> ProductEntities { get; set; } // appended "Entities" to avoid confusion with model of same name

        public virtual IDbSet<ProductOptionEntity> ProductOptionEntities { get; set; } // appended "Entities" to avoid confusion with model of same name

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Map the table names here so the entity classes do not depend on System.Data.Entity
            modelBuilder.Entity<ProductEntity>().ToTable("Product");
            modelBuilder.Entity<ProductOptionEntity>().ToTable("ProductOption");

        }
    }
}
