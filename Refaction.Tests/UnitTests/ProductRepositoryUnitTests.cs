using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Data.Entity;
using Refaction.Data.Entities;
using Refaction.Data;
using Refaction.Service.Repositories;
using Refaction.Service.Models;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using Refaction.Service;
using Refaction.Data.Fakes;

namespace Refaction.Tests.UnitTests
{
    /// <summary>
    /// Unit tests for 100% code-coverage of ProductRepository
    /// </summary>
    [TestClass]
    public class ProductRepositoryUnitTests
    {
        IRefactionDbContext _db;
        ProductRepository _productRepository;

        [TestInitialize]
        public void InitializeDatabaseWithNoData()
        {
            _db = new FakeRefactionDbContext();
            _productRepository = new ProductRepository(_db);
        }

        public void InitializeDatabaseWithSampleData()
        {
            _db = new FakeRefactionDbContext(SampleData.ProductEntities, SampleData.ProductOptionEntities);
            _productRepository = new ProductRepository(_db);
        }

        [TestMethod]
        public void ProductRepository_CreateProductInEmptyDatabase()
        {
            var newProduct =
                new Product
                {
                    Id = Guid.NewGuid(),
                    DeliveryPrice = 3.33M,
                    Description = "The third product",
                    Name = "Product 3",
                    Price = 3.33M
                };

            _productRepository.Create(newProduct);

            var productEntities = _db.ProductEntities.Local;

            Assert.IsTrue(productEntities.Count == 1);

            var productEntity = productEntities[0];

            var entity = ProductRepository.ModelSelectorFunc(productEntity);

            Assert.AreEqual(newProduct.DeliveryPrice, entity.DeliveryPrice);
            Assert.AreEqual(newProduct.Description, entity.Description);
            Assert.AreEqual(newProduct.Id, entity.Id);
            Assert.AreEqual(newProduct.Name, entity.Name);
            Assert.AreEqual(newProduct.Price, entity.Price);
        }

        [TestMethod]
        public void ProductRepository_RetrieveAllProductsUsingSampleData()
        {
            InitializeDatabaseWithSampleData();

            var products =
                _productRepository.Retrieve()
                .ToArray();

            Assert.IsTrue(products.Count() == 2);

            Assert.IsTrue(ModelComparer.AreEquivalent(products[0], SampleModels.Product0));
            Assert.IsTrue(ModelComparer.AreEquivalent(products[1], SampleModels.Product1));
        }

        [TestMethod]
        public void ProductRepository_RetrieveProductByIdUsingSampleData()
        {
            InitializeDatabaseWithSampleData();

            var product = _productRepository.Retrieve(SampleModels.Product1.Id);

            Assert.IsTrue(ModelComparer.AreEquivalent(product, SampleModels.Product1));
        }

        [TestMethod]
        public void ProductRepository_RetrieveProductByIdFromEmptyDatabase_ShouldReturnNull()
        {
            var product = _productRepository.Retrieve(SampleModels.Product1.Id);

            Assert.IsNull(product);
        }

        [TestMethod]
        public void ProductRepository_RetrieveProductByNameUsingSampleData()
        {
            InitializeDatabaseWithSampleData();

            var products = _productRepository.Retrieve("1");

            Assert.IsTrue(products.Count() == 1);

            var product = products.Single();

            Assert.IsTrue(ModelComparer.AreEquivalent(product, SampleModels.Product1));
        }

        [TestMethod]
        public void ProductRepository_UpdateProduct()
        {
            InitializeDatabaseWithSampleData();

            var product = new Product(SampleModels.Product1);
            product.DeliveryPrice = 1234.56M;

            _productRepository.Update(product);

            var updatedProduct = _productRepository.Retrieve(product.Id);

            Assert.IsTrue(ModelComparer.AreEquivalent(product, updatedProduct));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductRepository_UpdateProductInEmptyDatabase_ShouldThrowNotFoundException()
        {
            var product = new Product(SampleModels.Product1);

            product.DeliveryPrice = 1234.56M;

            _productRepository.Update(product);
        }

        [TestMethod]
        public void ProductRepository_DeleteProductUsingSampleData()
        {
            InitializeDatabaseWithSampleData();

            _productRepository.Delete(SampleModels.Product1);

            // verify products
            var productEntities = _db.ProductEntities.Local;
            Assert.IsTrue(productEntities.Count == 1);

            productEntities = _db.ProductEntities.Local;
            var remainingProductEntity = productEntities[0];
            Assert.IsTrue(EntityComparer.AreEquivalent(remainingProductEntity, SampleData.ProductEntity0));

            // verify product options
            var productOptionEntities = _db.ProductOptionEntities.Local;
            Assert.IsTrue(productOptionEntities.Count == 1);

            var remainingProductOption = productOptionEntities[0];
            Assert.IsTrue(EntityComparer.AreEquivalent(remainingProductOption, SampleData.ProductOptionEntity0));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductRepository_DeleteProductFromEmptyDatabase_ShouldThrowNotFoundException()
        {
            _productRepository.Delete(SampleModels.Product1);
        }

        [TestMethod]
        public void ProductRepository_ExistsProductInEmptyDatabase_ShouldReturnFalse()
        {
            var exists = _productRepository.Exists(SampleModels.Product1.Id);

            Assert.IsFalse(exists);
        }

        [TestMethod]
        public void ProductRepository_ExistsProductUsingSampleData_ShouldReturnTrue()
        {
            InitializeDatabaseWithSampleData();

            var exists = _productRepository.Exists(SampleModels.Product1.Id);

            Assert.IsTrue(exists);
        }
    }
}
