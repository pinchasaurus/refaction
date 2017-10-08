using Refaction.Common.Patterns;
using Refaction.Data;
using Refaction.Data.Entities;
using Refaction.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Refaction.Service.Repositories
{
    /// <summary>
    /// CRUD operations for Product models
    /// </summary>
    public class ProductRepository : RepositoryBase<Product, Guid>, IProductRepository
    {
        protected IRefactionDbContext _db;

        public ProductRepository(IRefactionDbContext db)
        {
            _db = db;
        }

        public override void Create(Product model)
        {
            var product =
                new ProductEntity()
                {
                    Id = model.Id,
                    DeliveryPrice = model.DeliveryPrice,
                    Description = model.Description,
                    Name = model.Name,
                    Price = model.Price
                };


            _db.ProductEntities.Add(product);
        }

        public override IEnumerable<Product> Retrieve()
        {
            return
                _db.ProductEntities
                .Select(ModelSelectorExpression)
                .AsEnumerable()
                ;
        }

        public override Product Retrieve(Guid id)
        {
            Product result;

            var product = _db.ProductEntities.SingleOrDefault(entity => entity.Id == id);

            if (product == null)
            {
                result = null;
            }
            else
            {
                result = ModelSelectorFunc(product);
            }

            return result;
        }

        public IEnumerable<Product> Retrieve(string name)
        {
            var lowerName = name.ToLower();

            var result =
                _db.ProductEntities
                .Where(entity => entity.Name.ToLower().Contains(lowerName))
                .Select(ModelSelectorExpression)
                .AsEnumerable()
                ;

            return result;
        }


        public override void Update(Product model)
        {
            var product = _db.ProductEntities.Find(model.Id);

            if (product == null)
            {
                throw new HttpResponseException_NotFoundException();
            }

            product.DeliveryPrice = model.DeliveryPrice;
            product.Description = model.Description;
            product.Name = model.Name;
            product.Price = model.Price;
        }
        
        public override void Delete(Guid id)
        {
            var product = 
                _db.ProductEntities
                .SingleOrDefault(entity => entity.Id == id);

            if (product == null)
            {
                throw new HttpResponseException_NotFoundException();
            }

            var productOptions = 
                _db.ProductOptionEntities
                .Where(productOption => productOption.ProductId == id)
                .ToList();

            // cascading delete of all related product options
            foreach (var productOption in productOptions)
            {
                _db.ProductOptionEntities.Remove(productOption);
            }

            // delete product
            _db.ProductEntities.Remove(product);
        }
                public override void Delete(Product model)
        {
            Delete(model.Id);
        }
        
        public override bool Exists(Guid id)
        {
            return _db.ProductEntities.Any(entity => entity.Id == id);
        }
        
        // Expression for converting an entity into a model
        public readonly static Expression<Func<ProductEntity, Product>> ModelSelectorExpression =
            (ProductEntity entity) =>
                new Product()
                {
                    Id = entity.Id,
                    DeliveryPrice = entity.DeliveryPrice,
                    Description = entity.Description,
                    Name = entity.Name,
                    Price = entity.Price,
                };

        // Expression for converting an entity into a model
        public readonly static Func<ProductEntity, Product> ModelSelectorFunc = ModelSelectorExpression.Compile();
    }
}
