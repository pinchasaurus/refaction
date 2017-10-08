using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using Refaction.Data;
using Refaction.Data.Entities;
using Refaction.Service;
using Refaction.Service.Models;
using Refaction.Service.Repositories;
using Refaction.UnitTests;
using Refaction.UnitTests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Refaction.Tests.UnitTests
{
    /// <summary>
    /// Unit tests for 100% code coverage of ProductRepository 
    /// (using strict mocks to guarantee precise behavior)
    /// </summary>
    [TestClass]
    public class ProductRepositoryUnitTestsUsingMocks : RefactionTestUsingMocksBase
    {
        IProductRepository _productRepository;

        public ProductRepositoryUnitTestsUsingMocks()
        {
        }

        protected override void UseEmptyDatabase()
        {
            base.UseEmptyDatabase();

            _productRepository = new ProductRepository(this.MockRefactionDbContext.Object);
        }

        protected override void UseSampleDatabase()
        {
            base.UseSampleDatabase();

            _productRepository = new ProductRepository(this.MockRefactionDbContext.Object);
        }


        IProductRepository CurrentProductRepository
        {
            get { return _productRepository; }
        }


        [TestMethod]
        public void ProductRepository_CreateProduct_UsingEmptyDatabase()
        {
            UseEmptyDatabase();

            CreateProduct();

            this.MockRefactionDbContext.VerifyThisAndNestedMocks();
        }

        [TestMethod]
        public void ProductRepository_CreateProduct_UsingSampleDatabase()
        {
            UseSampleDatabase();

            CreateProduct();

            // verify that no other entities were affected
            var data = MockRefactionDbContext.MockProducts.Data;
            Assert.IsTrue(data.Count() == 4);
            Assert.IsTrue(EntityComparer.AreEquivalent(data[0], SampleData.ProductEntity0));
            Assert.IsTrue(EntityComparer.AreEquivalent(data[1], SampleData.ProductEntity1));
            Assert.IsTrue(EntityComparer.AreEquivalent(data[2], SampleData.ProductEntity2));

            this.MockRefactionDbContext.VerifyThisAndNestedMocks();
        }

        protected void CreateProduct()
        {
            var product =
                new Product
                {
                    Id = Guid.NewGuid(),
                    DeliveryPrice = 4.44M,
                    Description = "The fourth product",
                    Name = "Product 4",
                    Price = 4.44M
                };

            var expectedProductEntity = EntityFactory.CreateProductEntity(product);

            // expect add method call
            this.MockRefactionDbContext.MockProducts
                .Setup<ProductEntity>(m =>
                    m.Add(It.Is<ProductEntity>(e => EntityComparer.AreEquivalent(expectedProductEntity, e))))
                    .Callback<ProductEntity>(e => this.MockRefactionDbContext.MockProducts.Data.Add(e))
                    .Returns<ProductEntity>(e => e)
                    .Verifiable();

            CurrentProductRepository.Create(product);

            // verify that add method called once
            this.MockRefactionDbContext.MockProducts
                .Verify<ProductEntity>(m =>
                    m.Add(It.Is<ProductEntity>(actual => EntityComparer.AreEquivalent(expectedProductEntity, actual))),
                    Times.Once());

            // verify that created entity matches submitted entity
            var createdProductEntity = MockRefactionDbContext.MockProducts.Data.Single(x => x.Id == product.Id);
            var createdProduct = ProductRepository.ModelSelectorFunc(createdProductEntity);
            Assert.IsTrue(ModelComparer.AreEquivalent(product, createdProduct));
        }

        [TestMethod]
        public void ProductRepository_RetrieveAllProducts_UsingEmptyDatabase()
        {
            UseEmptyDatabase();

            MockProductQueryProvider.ExpectQuery<Product>();

            var products =
                CurrentProductRepository.Retrieve()
                .ToArray();

            Assert.IsTrue(products.Count() == 0);

            MockProductQueryProvider.VerifyQueriedOnce();
            MockRefactionDbContext.VerifyThisAndNestedMocks();
        }

        [TestMethod]
        public void ProductRepository_RetrieveAllProducts_UsingSampleDatabase()
        {
            UseSampleDatabase();

            MockProductQueryProvider.ExpectQuery<Product>();

            var products =
                CurrentProductRepository.Retrieve()
                .ToArray();

            Assert.IsTrue(products.Count() == 3);
            Assert.IsTrue(ModelComparer.AreEquivalent(products[0], SampleModels.Product0));
            Assert.IsTrue(ModelComparer.AreEquivalent(products[1], SampleModels.Product1));
            Assert.IsTrue(ModelComparer.AreEquivalent(products[2], SampleModels.Product2));

            MockProductQueryProvider.VerifyQueriedOnce();
            MockRefactionDbContext.VerifyThisAndNestedMocks();
        }

        [TestMethod]
        public void ProductRepository_RetrieveProductById_UsingSampleDatabase()
        {
            UseSampleDatabase();

            MockProductQueryProvider.ExpectExecute<ProductEntity>();

            var product = CurrentProductRepository.Retrieve(SampleModels.Product1.Id);

            Assert.IsTrue(ModelComparer.AreEquivalent(product, SampleModels.Product1));

            MockProductQueryProvider.VerifyQueriedOnce();
            MockRefactionDbContext.VerifyThisAndNestedMocks();
        }

        [TestMethod]
        public void ProductRepository_RetrieveProductByIdFromEmptyDatabase_ShouldReturnNull()
        {
            UseEmptyDatabase();

            MockProductQueryProvider.ExpectExecute<ProductEntity>();

            var product = CurrentProductRepository.Retrieve(SampleModels.Product1.Id);

            Assert.IsNull(product);

            MockProductQueryProvider.VerifyQueriedOnce();
            MockRefactionDbContext.VerifyThisAndNestedMocks();
        }

        [TestMethod]
        public void ProductRepository_RetrieveProductByName_UsingSampleDatabase()
        {
            UseSampleDatabase();

            MockProductQueryProvider.ExpectQuery<ProductEntity>();

            var products = CurrentProductRepository.Retrieve("1");

            Assert.IsTrue(products.Count() == 1);

            var product = products.Single();

            Assert.IsTrue(ModelComparer.AreEquivalent(product, SampleModels.Product1));

            MockProductQueryProvider.VerifyQueriedOnce();
            MockRefactionDbContext.VerifyThisAndNestedMocks();
        }

        [TestMethod]
        public void ProductRepository_UpdateProduct()
        {
            UseSampleDatabase();

            var submittedProduct = SampleModels.Product1;
            submittedProduct.Description = "The updated product";

            // expect find method call
            MockRefactionDbContext.MockProducts
                .Setup<ProductEntity>(m =>
                    m.Find(It.Is<Guid>(actual => submittedProduct.Id == actual)))
                    .Returns(MockRefactionDbContext.MockProducts.Data.Single(x => x.Id == submittedProduct.Id))
                .Verifiable();

            CurrentProductRepository.Update(submittedProduct);

            // verify that find method was called once
            MockRefactionDbContext.MockProducts
                .Verify<ProductEntity>(m =>
                    m.Find(It.Is<Guid>(actual => submittedProduct.Id == actual)),
                    Times.Once());

            // verify that submitted product matches updated product
            var updatedProductEntity = MockRefactionDbContext.MockProducts.Data.Single(x => x.Id == submittedProduct.Id);
            var updatedProduct = ProductRepository.ModelSelectorFunc(updatedProductEntity);
            Assert.IsTrue(ModelComparer.AreEquivalent(submittedProduct, updatedProduct));

            MockProductQueryProvider.VerifyQueriedOnce();
            MockRefactionDbContext.VerifyThisAndNestedMocks();
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductRepository_UpdateProductInEmptyDatabase_ShouldThrowNotFoundException()
        {
            UseEmptyDatabase();

            var submittedProduct = SampleModels.Product1;
            submittedProduct.Description = "The updated product";

            // expect find method call
            MockRefactionDbContext.MockProducts
                .Setup<ProductEntity>(m =>
                    m.Find(It.Is<Guid>(actual => submittedProduct.Id == actual)))
                    .Returns((ProductEntity)null)
                    .Verifiable();

            CurrentProductRepository.Update(submittedProduct);
        }

        [TestMethod]
        public void ProductRepository_DeleteProduct_UsingSampleDatabase()
        {
            UseSampleDatabase();

            MockProductQueryProvider.ExpectExecute<ProductEntity>();

            MockProductOptionQueryProvider.ExpectQuery<ProductOptionEntity>();
            MockProductOptionQueryProvider.ExpectExecute<ProductOptionEntity>();

            var expectedProductEntity = SampleData.ProductEntity1;

            // expect remove method call
            MockRefactionDbContext.MockProducts
                .Setup<ProductEntity>(m =>
                    m.Remove(It.Is<ProductEntity>(actual => EntityComparer.AreEquivalent(expectedProductEntity, actual))))
                    .Callback<ProductEntity>(x => MockRefactionDbContext.MockProducts.Data.Remove(x))
                    .Returns(MockRefactionDbContext.MockProducts.Data.Single(x => x.Id == SampleModels.Product1.Id))
                    .Verifiable();

            // expect remove method call on product options too
            MockRefactionDbContext.MockProductOptions
                .Setup<ProductOptionEntity>(m =>
                    m.Remove(It.IsAny<ProductOptionEntity>()))
                    .Callback<ProductOptionEntity>(x => MockRefactionDbContext.MockProductOptions.Data.Remove(x))
                    .Returns((ProductOptionEntity e) => e)
                    .Verifiable();

            CurrentProductRepository.Delete(SampleModels.Product1);

            // verify that product remove method was called once
            MockRefactionDbContext.MockProducts
                .Verify<ProductEntity>(m =>
                    m.Remove(It.Is<ProductEntity>(actual => EntityComparer.AreEquivalent(expectedProductEntity, actual))),
                    Times.Once());

            // verify that product options remove method was called twice
            MockRefactionDbContext.MockProductOptions
                .Verify<ProductOptionEntity>(m =>
                    m.Remove(It.IsAny<ProductOptionEntity>()),
                    Times.Exactly(2));

            // verify the no other products were affected
            var productEntities = MockRefactionDbContext.MockProducts.Data.ToArray();
            Assert.IsTrue(productEntities.Count() == 2);
            Assert.IsTrue(EntityComparer.AreEquivalent(productEntities[0], SampleData.ProductEntity0));
            Assert.IsTrue(EntityComparer.AreEquivalent(productEntities[1], SampleData.ProductEntity2));

            // verify the no other products options were affected
            var productOptionEntities = MockRefactionDbContext.MockProductOptions.Data.ToArray();
            Assert.IsTrue(productOptionEntities.Count() == 2);
            Assert.IsTrue(EntityComparer.AreEquivalent(productOptionEntities[0], SampleData.ProductOptionEntity0));
            Assert.IsTrue(EntityComparer.AreEquivalent(productOptionEntities[1], SampleData.ProductOptionEntity2));

            MockProductQueryProvider.VerifyQueriedOnce();
            MockRefactionDbContext.VerifyThisAndNestedMocks();
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductRepository_DeleteProductFromEmptyDatabase_ShouldThrowNotFoundException()
        {
            UseEmptyDatabase();

            MockProductQueryProvider.ExpectExecute<ProductEntity>();

            CurrentProductRepository.Delete(SampleModels.Product1);
        }

        [TestMethod]
        public void ProductRepository_ExistsProductInEmptyDatabase_ShouldReturnFalse()
        {
            UseEmptyDatabase();

            MockProductQueryProvider.ExpectExecute<bool>();

            var exists = CurrentProductRepository.Exists(SampleModels.Product1.Id);

            Assert.IsFalse(exists);

            MockProductQueryProvider.VerifyQueriedOnce();
            MockRefactionDbContext.VerifyThisAndNestedMocks();
        }

        [TestMethod]
        public void ProductRepository_ExistsProductUsingSampleDatabase_ShouldReturnTrue()
        {
            UseSampleDatabase();

            MockProductQueryProvider.ExpectExecute<bool>();

            var exists = CurrentProductRepository.Exists(SampleModels.Product1.Id);

            Assert.IsTrue(exists);

            MockProductQueryProvider.VerifyQueriedOnce();
            MockRefactionDbContext.VerifyThisAndNestedMocks();
        }
    }
}
