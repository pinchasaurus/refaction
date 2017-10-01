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
    public class ProductOptionRepository : RepositoryBase<ProductOption, Guid>
    {
        protected IRefactionDbContext _db;

        public ProductOptionRepository(IRefactionDbContext db)
        {
            _db = db;
        }

        public override void Create(ProductOption model)
        {
            var productOption = 
                new ProductOptionEntity()
                {
                    Id = model.Id,
                    Description = model.Description,
                    Name = model.Name,
                    ProductId = model.ProductId
                };


            _db.ProductOptionEntities.Add(productOption);
        }



        public override IEnumerable<ProductOption> Retrieve()
        {
            return
                _db.ProductOptionEntities
                .Select(ModelSelectorExpression)
                .AsEnumerable()
                ;
        }

        public override ProductOption Retrieve(Guid id)
        {
            ProductOption result;

            var productOption = _db.ProductOptionEntities.SingleOrDefault(entity => entity.Id == id);

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
                _db.ProductOptionEntities
                .Where(entity => entity.ProductId == id)
                .Select(ModelSelectorExpression)
                .AsEnumerable()
                ;
        }

        public ProductOption RetrieveByBothIds(Guid productId, Guid id)
        {
            return
                _db.ProductOptionEntities
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
            var productOption = _db.ProductOptionEntities.Find(model.Id);

            if (productOption == null)
            {
                throw new HttpResponseException_NotFoundException();
            }

            productOption.Description = model.Description;
            productOption.Name = model.Name;
            productOption.ProductId = model.ProductId;
        }


        public override void Delete(Guid id)
        {
            var productOption = _db.ProductOptionEntities.SingleOrDefault(entity => entity.Id == id);

            if (productOption == null)
            {
                throw new HttpResponseException_NotFoundException();
            }

            _db.ProductOptionEntities.Remove(productOption);
        }

        public override void Delete(ProductOption model)
        {
            Delete(model.Id);
        }



        public override bool Exists(Guid id)
        {
            return _db.ProductOptionEntities.Any(entity => entity.Id == id);
        }



        public readonly static Expression<Func<ProductOptionEntity, ProductOption>> ModelSelectorExpression =
            (ProductOptionEntity entity) =>
                new ProductOption()
                {
                    Id = entity.Id,
                    Description = entity.Description,
                    Name = entity.Name,
                    ProductId = entity.ProductId,
                };

        public readonly static Func<ProductOptionEntity, ProductOption> ModelSelectorFunc = ModelSelectorExpression.Compile();
    }
}
