using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ninject;
using Refaction.Service.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refaction.Service;
using Refaction.Tests;
using System.Diagnostics;
using Refaction.Service.Controllers;
using Refaction.Service.Repositories;

namespace Refaction.UnitTests.UnitTests
{
    /// <summary>
    /// Integration tests for 100% code-coverage of ProductsController
    /// </summary>
    [TestClass]
    public class ProductsControllerIntegrationTests : RefactionTestUsingFakesBase
    {
        public ProductsController CurrentProductsController
        {
            get { return NinjectKernel.Get<ProductsController>(); }
        }

        [TestMethod]
        public void ProductsController_CreateProductUsingEmptyDatabase()
        {
            UseEmptyDatabase();

            var product =
                new Product
                {
                    DeliveryPrice = 4.44M,
                    Description = "The created product",
                    Id = Guid.NewGuid(),
                    Name = "Product 3",
                    Price = 4.44M,
                };

            CurrentProductsController.CreateProduct(product);

            var createdProduct = CurrentProductsController.GetProduct(product.Id);

            Assert.IsTrue(ModelComparer.AreEquivalent(product, createdProduct));
        }

        [TestMethod]
        public void ProductsController_CreateProductUsingSampleData()
        {
            UseSampleDatabase();

            var product =
                new Product
                {
                    DeliveryPrice = 4.44M,
                    Description = "The created product",
                    Id = Guid.NewGuid(),
                    Name = "Product 3",
                    Price = 4.44M,
                };

            CurrentProductsController.CreateProduct(product);

            var createdProduct = CurrentProductsController.GetProduct(product.Id);

            Assert.IsTrue(ModelComparer.AreEquivalent(product, createdProduct));
        }

        [TestMethod]
        public void ProductsController_RetrieveAllProductsUsingEmptyDatabase_ShouldReturnZeroProducts()
        {
            UseEmptyDatabase();

            var products = CurrentProductsController.GetAllProducts();

            var productsArray = products.Items.ToArray();

            Assert.AreEqual(0, productsArray.Length);
        }

        [TestMethod]
        public void ProductsController_RetrieveAllProductsUsingSampleData()
        {
            UseSampleDatabase();

            var products = CurrentProductsController.GetAllProducts();

            var productsArray = products.Items.ToArray();

            Assert.AreEqual(3, productsArray.Length);

            Assert.IsTrue(ModelComparer.AreEquivalent(productsArray[0], SampleModels.Product0), "Product0 did not match");
            Assert.IsTrue(ModelComparer.AreEquivalent(productsArray[1], SampleModels.Product1), "Product1 did not match");
            Assert.IsTrue(ModelComparer.AreEquivalent(productsArray[2], SampleModels.Product2), "Product2 did not match");
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductsController_RetrieveProductByIdUsingEmptyDatabase_ShouldThrowNotFoundException()
        {
            UseEmptyDatabase();

            var product = CurrentProductsController.GetProduct(SampleModels.Product0.Id);
        }

        [TestMethod]
        public void ProductsController_RetrieveProductByIdUsingSampleData()
        {
            UseSampleDatabase();

            var product = CurrentProductsController.GetProduct(SampleModels.Product0.Id);

            Assert.IsTrue(ModelComparer.AreEquivalent(product, SampleModels.Product0), "Unexpected product returned");
        }

        [TestMethod]
        public void ProductsController_RetrieveProductByNameUsingEmptyDatabase_ShouldReturnZeroProducts()
        {
            UseEmptyDatabase();

            var products = CurrentProductsController.GetProductsByName("0");

            var productsArray = products.Items.ToArray();

            Assert.AreEqual(0, productsArray.Length);
        }

        [TestMethod]
        public void ProductsController_RetrieveProductByNameUsingSampleData()
        {
            UseSampleDatabase();

            var products = CurrentProductsController.GetProductsByName("0");

            var productsArray = products.Items.ToArray();

            Assert.AreEqual(1, productsArray.Length);
            Assert.IsTrue(ModelComparer.AreEquivalent(productsArray[0], SampleModels.Product0), "Unexpected product returned");
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductsController_UpdateUsingEmptyDatabase_ShouldThrowNotFound()
        {
            UseEmptyDatabase();

            var product = SampleModels.Product0;
            product.DeliveryPrice = 1234.56M;

            CurrentProductsController.UpdateProduct(product.Id, product);
        }

        [TestMethod]
        public void ProductsController_UpdateUsingSampleData()
        {
            UseSampleDatabase();

            var product = SampleModels.Product0;
            product.DeliveryPrice = 1234.56M;

            CurrentProductsController.UpdateProduct(product.Id, product);

            var updatedProduct = CurrentProductsController.GetProduct(product.Id);

            Assert.IsTrue(ModelComparer.AreEquivalent(product, updatedProduct));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductsController_DeleteUsingEmptyDatabase_ShouldThrowNotFound()
        {
            UseEmptyDatabase();

            var product = SampleModels.Product0;

            CurrentProductsController.DeleteProduct(product.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductsController_DeleteUsingSampleData()
        {
            UseSampleDatabase();

            var product = SampleModels.Product0;

            CurrentProductsController.DeleteProduct(product.Id);

            CurrentProductsController.GetProduct(product.Id);
        }

        ///////////////////////////////
        ///////////////////////////////
        ///////////////////////////////
        //  Product Options methods  //
        ///////////////////////////////
        ///////////////////////////////
        ///////////////////////////////

        [TestMethod]
        public void ProductOptionsController_CreateProductOptionUsingEmptyDatabase()
        {
            UseEmptyDatabase();

            var productOption =
                new ProductOption
                {
                    Description = "The created productOption",
                    Id = Guid.NewGuid(),
                    Name = "ProductOption 3",
                    ProductId = SampleModels.Product0.Id,
                };

            CurrentProductsController.CreateOption(productOption.Id, productOption);

            var createdProductOption = CurrentProductsController.GetOption(productOption.ProductId, productOption.Id);

            Assert.IsTrue(ModelComparer.AreEquivalent(productOption, createdProductOption));
        }


        [TestMethod]
        public void ProductOptionsController_RetrieveProductOptionsByProductIdUsingEmptyDatabase_ShouldReturnZeroProductOptions()
        {
            UseEmptyDatabase();

            var productOptions = CurrentProductsController.GetOptions(SampleModels.ProductOption0.Id);

            var productOptionsArray = productOptions.Items.ToArray();

            Assert.AreEqual(0, productOptionsArray.Length);
        }

        [TestMethod]
        public void ProductOptionsController_RetrieveProductOptionsByProductIdUsingSampleData()
        {
            UseSampleDatabase();

            var productOptions = CurrentProductsController.GetOptions(SampleModels.Product0.Id);

            var productOptionsArray = productOptions.Items.ToArray();

            Assert.AreEqual(2, productOptionsArray.Length);

            Assert.IsTrue(ModelComparer.AreEquivalent(productOptionsArray[0], SampleModels.ProductOption0));
            Assert.IsTrue(ModelComparer.AreEquivalent(productOptionsArray[1], SampleModels.ProductOption2));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductOptionsController_RetrieveProductOptionsByBothIdsUsingEmptyDatabase_ShouldReturnNullProductOption()
        {
            UseEmptyDatabase();

            var productOption = CurrentProductsController.GetOption(SampleModels.Product0.Id, SampleModels.ProductOption0.Id);
        }

        [TestMethod]
        public void ProductOptionsController_RetrieveProductOptionsByBothIdsUsingSampleData()
        {
            UseSampleDatabase();

            var productOption = CurrentProductsController.GetOption(SampleModels.Product0.Id, SampleModels.ProductOption0.Id);

            Assert.IsTrue(ModelComparer.AreEquivalent(productOption, SampleModels.ProductOption0));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductOptionsController_UpdateUsingEmptyDatabase_ShouldThrowNotFound()
        {
            UseEmptyDatabase();

            var productOption = SampleModels.ProductOption0;
            productOption.Name = "The updated product option";

            CurrentProductsController.UpdateOption(productOption.ProductId, productOption.Id, productOption);
        }

        [TestMethod]
        public void ProductOptionsController_UpdateUsingSampleData()
        {
            UseSampleDatabase();

            var productOption = SampleModels.ProductOption0;
            productOption.Name = "The updated product option";

            CurrentProductsController.UpdateOption(productOption.ProductId, productOption.Id, productOption);

            var updatedProductOption = CurrentProductsController.GetOption(productOption.ProductId, productOption.Id);

            Assert.IsTrue(ModelComparer.AreEquivalent(productOption, updatedProductOption));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductOptionsController_DeleteUsingEmptyDatabase_ShouldThrowNotFound()
        {
            UseEmptyDatabase();

            var productOption = SampleModels.ProductOption0;

            CurrentProductsController.DeleteOption(productOption.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductOptionsController_DeleteUsingSampleData()
        {
            UseSampleDatabase();

            var productOption = SampleModels.ProductOption0;

            CurrentProductsController.DeleteOption(productOption.Id);

            CurrentProductsController.GetOption(productOption.ProductId, productOption.Id);
        }

        [TestMethod]
        public void ProductOptionsController_DefaultConstructor()
        {
            // ensure 100% code coverage
            var controller = new ProductsController();
        }

        [TestMethod]
        public void ProductOptionsController_Dispose()
        {
            UseEmptyDatabase();

            // ensure 100% code coverage
            var controller = new ProductsController(CurrentDbContext, new ProductRepository(CurrentDbContext), new ProductOptionRepository(CurrentDbContext));
            controller.Dispose();
        }

    }
}
