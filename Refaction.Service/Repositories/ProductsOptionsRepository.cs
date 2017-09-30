using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using Refaction.Common.Patterns;
using Refaction.Data;
using Refaction.Data.Entities;
using Refaction.Service.Models;
using System.Web.Http;

namespace Refaction.Service.Repositories
{
    public class ProductOptionsRepository : RepositoryBase<ProductOption, Guid>
    {
        protected RefactionDbContext _db;

        public ProductOptionsRepository(RefactionDbContext db)
        {
            _db = db;
        }

        public override void Create(ProductOption model)
        {
            var productOption = 
                new ProductOptionEntity()
                {
                    Description = model.Description,
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    ProductId = model.ProductId
                };


            _db.ProductOptionsEntities.Add(productOption);
        }



        public override IEnumerable<ProductOption> Retrieve()
        {
            return
                _db.ProductOptionsEntities
                .Select(ModelSelectorExpression)
                .AsEnumerable()
                ;
        }

        public override ProductOption Retrieve(Guid id)
        {
            ProductOption result;

            var productOption = _db.ProductOptionsEntities.SingleOrDefault(entity => entity.Id == id);

            if (productOption == null)
            {
                result = null;
            }
            else
            {
                result = ModelSelectorFunc(productOption);
            }

            return result;
        }

        public IEnumerable<ProductOption> RetrieveByProductId(Guid id)
        {
            return
                _db.ProductOptionsEntities
                .Where(entity => entity.ProductId == id)
                .Select(ModelSelectorExpression)
                .AsEnumerable()
                ;
        }

        public ProductOption RetrieveByBothIds(Guid productId, Guid id)
        {
            return
                _db.ProductOptionsEntities
                .Where(entity =>
                    entity.ProductId == productId
                    && entity.Id == id
                )
                .Select(ModelSelectorExpression)
                .SingleOrDefault();
            ;
        }


        public override void Update(ProductOption model)
        {
            var productOption = _db.ProductOptionsEntities.Find(model.Id);

            if (productOption == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            productOption.Description = model.Description;
            productOption.Name = model.Name;
            productOption.ProductId = model.ProductId;
        }


        public override void Delete(Guid id)
        {
            var productOption = _db.ProductOptionsEntities.SingleOrDefault(entity => entity.Id == id);

            if (productOption == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            _db.ProductOptionsEntities.Remove(productOption);
        }

        public override void Delete(ProductOption model)
        {
            Delete(model);
        }



        public override bool Exists(Guid id)
        {
            return _db.ProductOptionsEntities.Any(entity => entity.Id == id);
        }



        protected readonly static Expression<Func<ProductOptionEntity, ProductOption>> ModelSelectorExpression =
            (ProductOptionEntity entity) =>
                new ProductOption()
                {
                    Id = entity.Id,
                    Description = entity.Description,
                    Name = entity.Name,
                    ProductId = entity.ProductId,
                };

        protected readonly static Func<ProductOptionEntity, ProductOption> ModelSelectorFunc = ModelSelectorExpression.Compile();
    }
}
