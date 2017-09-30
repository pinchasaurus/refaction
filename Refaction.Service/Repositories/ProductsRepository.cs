﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using Refaction.Common.Patterns;
using Refaction.Data;
using Refaction.Service.Models;
using System.Web.Http;
using System.Net.Http;
using Refaction.Data.Entities;

namespace Refaction.Service.Repositories
{
    public class ProductsRepository : RepositoryBase<Product, Guid>
    {
        protected RefactionDbContext _db;

        public ProductsRepository(RefactionDbContext db)
        {
            _db = db;
        }


        public override void Create(Product model)
        {
            var product =
                new ProductEntity()
                {
                    Id = Guid.NewGuid(),
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
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            product.DeliveryPrice = model.DeliveryPrice;
            product.Description = model.Description;
            product.Name = model.Name;
            product.Price = model.Price;
        }


        public override void Delete(Guid id)
        {
            var product = _db.ProductEntities.SingleOrDefault(entity => entity.Id == id);

            if (product == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            var productOptions = _db.ProductOptionsEntities.Where(productOption => productOption.ProductId == id);

            // cascading delete of all related product options
            _db.ProductOptionsEntities.RemoveRange(productOptions);

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


        protected readonly static Expression<Func<ProductEntity, Product>> ModelSelectorExpression =
            (ProductEntity entity) =>
                new Product()
                {
                    Id = entity.Id,
                    DeliveryPrice = entity.DeliveryPrice,
                    Description = entity.Description,
                    Name = entity.Name,
                    Price = entity.Price,
                };

        protected readonly static Func<ProductEntity, Product> ModelSelectorFunc = ModelSelectorExpression.Compile();
    }
}
