using NSwag.Annotations;
using Refaction.Data;
using Refaction.Data.Fakes;
using Refaction.Service.Models;
using Refaction.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

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
        [System.Web.Http.Description.ResponseType(typeof(Products))] // disambiguate response type to prevent confusion with swagger attribute of same name
        public IHttpActionResult GetAllProducts()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var items = _productRepository.Retrieve();

            var result = new Products(items);
            return Ok(result);
        }

        [Route]
        [HttpGet]
        [SwaggerIgnore] // Swagger does not support actions whose routes differ only by their query string
        public IHttpActionResult GetProductsByName(string name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var items = _productRepository.Retrieve(name);

            var result = new Products(items);

            return Ok(result);
        }

        [Route("{id}")]
        [HttpGet]
        [System.Web.Http.Description.ResponseType(typeof(Product))] // disambiguate response type to prevent confusion with swagger attribute of same name
        public IHttpActionResult GetProduct(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = _productRepository.Retrieve(id);

            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        [Route]
        [HttpPost]
        public IHttpActionResult CreateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            product.Id = Guid.NewGuid();

            _productRepository.Create(product);

            return Ok();
        }

        [Route("{id:guid}")]
        [HttpPut]
        public IHttpActionResult UpdateProduct(Guid id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            product.Id = id;

            _productRepository.Update(product);

            return Ok();
        }

        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteProduct(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _productRepository.Delete(id);

            return Ok();
        }

        [Route("{productId:guid}/options")]
        [HttpGet]
        [System.Web.Http.Description.ResponseType(typeof(ProductOptions))] // disambiguate response type to prevent confusion with swagger attribute of same name

        public IHttpActionResult GetOptions(Guid productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var items = _productOptionRepository.RetrieveByProductId(productId);

            var result = new ProductOptions(items);

            return Ok(result);
        }

        [Route("{productId:guid}/options/{id:guid}")]
        [HttpGet]
        [System.Web.Http.Description.ResponseType(typeof(ProductOption))] // disambiguate response type to prevent confusion with swagger attribute of same name
        public IHttpActionResult GetOption(Guid productId, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result =
                _productOptionRepository
                .RetrieveByBothIds(productId, id);

            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        [Route("{productId:guid}/options")]
        [HttpPost]
        public IHttpActionResult CreateOption(Guid productId, ProductOption option)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            option.Id = Guid.NewGuid();
            option.ProductId = productId;

            _productOptionRepository.Create(option);

            return Ok();
        }

        [Route("{productId}/options/{id:guid}")]
        [HttpPut]
        public IHttpActionResult UpdateOption(Guid productId, Guid id, ProductOption option)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            option.Id = id;
            option.ProductId = productId;

            _productOptionRepository.Update(option);

            return Ok();
        }

        [Route("{productId}/options/{id:guid}")]
        [HttpDelete]
        public IHttpActionResult DeleteOption(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _productOptionRepository.Delete(id);

            return Ok();
        }

    }
}
