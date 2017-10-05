using System;
using System.Net;
using System.Web.Http;
using System.Collections.Generic;

using System.Linq;

using Refaction.Data;
using Refaction.Service.Models;
using Refaction.Service.Repositories;
using Refaction.Data.Fakes;

namespace Refaction.Service.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        // create new db and repository for every request
        IRefactionDbContext _db;

        IProductRepository _productRepository;
        IProductOptionRepository _productOptionRepository;

        public ProductsController()
        {
        }

        public ProductsController(IRefactionDbContext db, IProductRepository productRepository, IProductOptionRepository productOptionRepository)
        {
            // These dependencies are injected by Ninject
            _db = db;

            _productRepository = productRepository;
            _productOptionRepository = productOptionRepository;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.SaveChanges();

                _productRepository.Dispose();
                _productOptionRepository.Dispose();

                _db.Dispose();
            }

            base.Dispose(disposing);
        }

        [Route]
        [HttpGet]
        public Products GetAllProducts()
        {
            var items = _productRepository.Retrieve();

            return new Products(items);
        }

        [Route]
        [HttpGet]
        public Products GetProductsByName(string name)
        {
            var items = _productRepository.Retrieve(name);

            return new Products(items);
        }

        [Route("{id}")]
        [HttpGet]
        public Product GetProduct(Guid id)
        {
            var result = _productRepository.Retrieve(id);

            if (result == null)
            {
                throw new HttpResponseException_NotFoundException();
            }
            else
            {
                return result;
            }
        }

        [Route]
        [HttpPost]
        public void CreateProduct(Product product)
        {
            product.Id = Guid.NewGuid();

            _productRepository.Create(product);
        }

        [Route("{id}")]
        [HttpPut]
        public void UpdateProduct(Guid id, Product product)
        {
            product.Id = id;

            _productRepository.Update(product);
        }

        [Route("{id}")]
        [HttpDelete]
        public void DeleteProduct(Guid id)
        {
            _productRepository.Delete(id);
        }

        [Route("{productId}/options")]
        [HttpGet]
        public ProductOptions GetOptions(Guid productId)
        {
            var items = _productOptionRepository.RetrieveByProductId(productId);

            return new ProductOptions(items);
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            var result =
                _productOptionRepository
                .RetrieveByBothIds(productId, id);

            if (result == null)
            {
                throw new HttpResponseException_NotFoundException();
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
            option.Id = Guid.NewGuid();
            option.ProductId = productId;

            _productOptionRepository.Create(option);
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public void UpdateOption(Guid productId, Guid id, ProductOption option)
        {
            option.Id = id;
            option.ProductId = productId;

            _productOptionRepository.Update(option);
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public void DeleteOption(Guid id)
        {
            _productOptionRepository.Delete(id);
        }

    }
}
