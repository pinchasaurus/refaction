﻿using System;
using System.Net;
using System.Web.Http;
using System.Collections.Generic;

using System.Linq;

using Refaction.Data;
using Refaction.Service.Models;
using Refaction.Service.Repositories;

namespace Refaction.Service.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        // create new db and repository for every request
        RefactionDbContext _db;

        ProductsRepository _products;
        ProductOptionsRepository _productOptions;

        ProductsController()
        {
            _db = new RefactionDbContext();

            _products = new ProductsRepository(_db);
            _productOptions = new ProductOptionsRepository(_db);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.SaveChanges();

                _products.Dispose();
                _productOptions.Dispose();

                _db.Dispose();
            }

            base.Dispose(disposing);
        }

        [Route]
        [HttpGet]
        public Products GetAll()
        {
            var items = _products.Retrieve();

            return new Products(items);
        }

        [Route]
        [HttpGet]
        public Products SearchByName(string name)
        {
            var items = _products.Retrieve(name);

            return new Products(items);
        }

        [Route("{id}")]
        [HttpGet]
        public Product GetProduct(Guid id)
        {
            var result = _products.Retrieve(id);

            if (result == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                return result;
            }
        }

        [Route]
        [HttpPost]
        public void Create(Product product)
        {
            _products.Create(product);
        }

        [Route("{id}")]
        [HttpPut]
        public void Update(Guid id, Product product)
        {
            product.Id = id;

            _products.Update(product);
        }

        [Route("{id}")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            _products.Delete(id);
        }

        [Route("{productId}/options")]
        [HttpGet]
        public ProductOptions GetOptions(Guid productId)
        {
            var items = _productOptions.RetrieveByProductId(productId);

            return new ProductOptions(items);
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            var result =
                _productOptions
                .RetrieveByBothIds(productId, id);

            if (result == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                return result;
            }
        }

        [Route("{productId}/options")]
        [HttpPost]
        public void CreateOption(Guid productId, ProductOption option)
        {
            option.ProductId = productId;

            _productOptions.Create(option);
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public void UpdateOption(Guid id, ProductOption option)
        {
            option.Id = id;

            _productOptions.Update(option);
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public void DeleteOption(Guid id)
        {
            _productOptions.Delete(id);
        }

    }
}
