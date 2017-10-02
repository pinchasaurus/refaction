﻿using System;
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
using Refaction.UnitTests;

using Ninject;
using Ninject.Parameters;

namespace Refaction.Tests.UnitTests
{
    /// <summary>
    /// Unit tests for 100% code-coverage of ProductRepository
    /// </summary>
    [TestClass]
    public class ProductRepositoryUnitTests : RefactionUnitTestBase
    {
        ProductRepository CurrentProductRepository
        {
            get { return NinjectKernel.Get<ProductRepository>(); }
        }

        [TestMethod]
        public void ProductRepository_CreateProductInEmptyDatabase()
        {
            UseEmptyDatabase();

            var newProduct =
                new Product
                {
                    Id = Guid.NewGuid(),
                    DeliveryPrice = 3.33M,
                    Description = "The third product",
                    Name = "Product 3",
                    Price = 3.33M
                };

            CurrentProductRepository.Create(newProduct);

            var productEntities = CurrentDbContext.ProductEntities.Local;

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
            UseDatabaseWithSampleData();

            var products =
                CurrentProductRepository.Retrieve()
                .ToArray();

            Assert.IsTrue(products.Count() == 3);

            Assert.IsTrue(ModelComparer.AreEquivalent(products[0], SampleModels.Product0));
            Assert.IsTrue(ModelComparer.AreEquivalent(products[1], SampleModels.Product1));
            Assert.IsTrue(ModelComparer.AreEquivalent(products[2], SampleModels.Product2));
        }

        [TestMethod]
        public void ProductRepository_RetrieveProductByIdUsingSampleData()
        {
            UseDatabaseWithSampleData();

            var product = CurrentProductRepository.Retrieve(SampleModels.Product1.Id);

            Assert.IsTrue(ModelComparer.AreEquivalent(product, SampleModels.Product1));
        }

        [TestMethod]
        public void ProductRepository_RetrieveProductByIdFromEmptyDatabase_ShouldReturnNull()
        {
            UseEmptyDatabase();

            var product = CurrentProductRepository.Retrieve(SampleModels.Product1.Id);

            Assert.IsNull(product);
        }

        [TestMethod]
        public void ProductRepository_RetrieveProductByNameUsingSampleData()
        {
            UseDatabaseWithSampleData();

            var products = CurrentProductRepository.Retrieve("1");

            Assert.IsTrue(products.Count() == 1);

            var product = products.Single();

            Assert.IsTrue(ModelComparer.AreEquivalent(product, SampleModels.Product1));
        }

        [TestMethod]
        public void ProductRepository_UpdateProduct()
        {
            UseDatabaseWithSampleData();

            var product = new Product(SampleModels.Product1);
            product.DeliveryPrice = 1234.56M;

            CurrentProductRepository.Update(product);

            var updatedProduct = CurrentProductRepository.Retrieve(product.Id);

            Assert.IsTrue(ModelComparer.AreEquivalent(product, updatedProduct));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductRepository_UpdateProductInEmptyDatabase_ShouldThrowNotFoundException()
        {
            UseEmptyDatabase();

            var product = new Product(SampleModels.Product1);

            product.DeliveryPrice = 1234.56M;

            CurrentProductRepository.Update(product);
        }

        [TestMethod]
        public void ProductRepository_DeleteProductUsingSampleData()
        {
            UseDatabaseWithSampleData();

            CurrentProductRepository.Delete(SampleModels.Product1);

            // verify products
            var productEntities = CurrentDbContext.ProductEntities.Local;
            Assert.IsTrue(productEntities.Count == 2);

            productEntities = CurrentDbContext.ProductEntities.Local;
            Assert.IsTrue(EntityComparer.AreEquivalent(productEntities[0], SampleData.ProductEntity0));
            Assert.IsTrue(EntityComparer.AreEquivalent(productEntities[1], SampleData.ProductEntity2));

            // verify product options
            var productOptionEntities = CurrentDbContext.ProductOptionEntities.Local;
            Assert.IsTrue(productOptionEntities.Count == 2);

            Assert.IsTrue(EntityComparer.AreEquivalent(productOptionEntities[0], SampleData.ProductOptionEntity0));
            Assert.IsTrue(EntityComparer.AreEquivalent(productOptionEntities[1], SampleData.ProductOptionEntity2));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductRepository_DeleteProductFromEmptyDatabase_ShouldThrowNotFoundException()
        {
            UseEmptyDatabase();

            CurrentProductRepository.Delete(SampleModels.Product1);
        }

        [TestMethod]
        public void ProductRepository_ExistsProductInEmptyDatabase_ShouldReturnFalse()
        {
            UseEmptyDatabase();

            var exists = CurrentProductRepository.Exists(SampleModels.Product1.Id);

            Assert.IsFalse(exists);
        }

        [TestMethod]
        public void ProductRepository_ExistsProductUsingSampleData_ShouldReturnTrue()
        {
            UseDatabaseWithSampleData();

            var exists = CurrentProductRepository.Exists(SampleModels.Product1.Id);

            Assert.IsTrue(exists);
        }
    }
}
