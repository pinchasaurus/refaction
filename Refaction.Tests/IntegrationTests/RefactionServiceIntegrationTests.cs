using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refaction.Common;
using Refaction.Data;
using Refaction.Data.Fakes;
using Refaction.Service;
using Refaction.Service.Models;
using Refaction.Service.Repositories;
using Refaction.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Refaction.UnitTests.UnitTests
{
    /// <summary>
    /// End-to-end testing of Refaction Web API service 
    /// using OWIN test server and FakeRefactionDbContext (in memory)
    /// </summary>
    [TestClass]
    public class RefactionServiceIntegrationTests : RefactionTestUsingFakesBase
    {
        TestServer _server;
        HttpClient _client;

        /// <summary>
        /// For each test, spin up an OWIN test server and connect an HTTP client to it
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _server = TestServer.Create<Refaction.Service.OwinStartup>();
            _client = new HttpClient(_server.Handler);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            _client.Dispose();
            _server.Dispose();
        }

        [TestMethod]
        public void RefactionService_CreateProduct_UsingEmptyDatabase()
        {
            UseEmptyDatabase();

            var submittedProduct =
                new Product
                {
                    DeliveryPrice = 4.44M,
                    Description = "The created product",
                    Id = Guid.NewGuid(),
                    Name = "Product 4",
                    Price = 4.44M
                };

            var response = _client.PostAsJsonAsync("http://testserver/products", submittedProduct).Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var retrieveResponse = _client.GetAsync($"http://testserver/products").Result;
            Assert.IsTrue(retrieveResponse.StatusCode == System.Net.HttpStatusCode.OK);

            var products = retrieveResponse.Content.ReadAsAsync<Products>().Result;
            var productsArray = products.Items.ToArray();
            Assert.IsTrue(productsArray.Length == 1);

            // verify the created product
            var createdProduct = productsArray[0];
            Assert.AreNotEqual(submittedProduct.Id, createdProduct.Id);
            createdProduct.Id = submittedProduct.Id;
            Assert.IsTrue(ModelComparer.AreEquivalent(submittedProduct, createdProduct));
        }

        [TestMethod]
        public void RefactionService_CreateProduct_UsingSampleDatabase()
        {
            UseSampleDatabase();

            var submittedProduct =
                new Product
                {
                    DeliveryPrice = 4.44M,
                    Description = "The created product",
                    Id = Guid.NewGuid(),
                    Name = "Product 4",
                    Price = 4.44M
                };

            var response = _client.PostAsJsonAsync("http://testserver/products", submittedProduct).Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var retrieveResponse = _client.GetAsync($"http://testserver/products").Result;
            Assert.IsTrue(retrieveResponse.StatusCode == System.Net.HttpStatusCode.OK);

            var products = retrieveResponse.Content.ReadAsAsync<Products>().Result;
            var productsArray = products.Items.ToArray();
            Assert.IsTrue(productsArray.Length == 4);
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.Product0, productsArray[0]));
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.Product1, productsArray[1]));
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.Product2, productsArray[2]));

            var createdProduct = productsArray[3];
            Assert.AreNotEqual(submittedProduct.Id, createdProduct.Id);
            createdProduct.Id = submittedProduct.Id;
            Assert.IsTrue(ModelComparer.AreEquivalent(submittedProduct, createdProduct));
        }

        [TestMethod]
        public void RefactionService_GetAllProducts_UsingEmptyDatabase()
        {
            UseEmptyDatabase();

            var response = _client.GetAsync("http://testserver/products").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var products = response.Content.ReadAsAsync<Products>().Result;
            var productsArray = products.Items.ToArray();
            Assert.IsTrue(productsArray.Length == 0);
        }

        [TestMethod]
        public void RefactionService_GetAllProductsUsingSampleData()
        {
            UseSampleDatabase();

            var response = _client.GetAsync("http://testserver/products").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var products = response.Content.ReadAsAsync<Products>().Result;
            var productsArray = products.Items.ToArray();
            Assert.IsTrue(productsArray.Length == 3);
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.Product0, productsArray[0]), "Product0");
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.Product1, productsArray[1]), "Product1");
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.Product2, productsArray[2]), "Product2");
        }

        [TestMethod]
        public void RefactionService_GetProductById_UsingEmptyDatabase()
        {
            UseEmptyDatabase();

            var response = _client.GetAsync($"http://testserver/products/{SampleModels.Product1.Id}").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void RefactionService_GetProductByIdUsingSampleData()
        {
            UseSampleDatabase();

            var response = _client.GetAsync($"http://testserver/products/{SampleModels.Product1.Id}").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var product = response.Content.ReadAsAsync<Product>().Result;
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.Product1, product));
        }

        [TestMethod]
        public void RefactionService_GetProductsByName_UsingEmptyDatabase()
        {
            UseEmptyDatabase();

            var response = _client.GetAsync("http://testserver/products?name=1").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var products = response.Content.ReadAsAsync<Products>().Result;
            var productsArray = products.Items.ToArray();
            Assert.IsTrue(productsArray.Length == 0);
        }

        [TestMethod]
        public void RefactionService_GetProductsByNameUsingSampleData()
        {
            UseSampleDatabase();

            var response = _client.GetAsync("http://testserver/products?name=1").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var products = response.Content.ReadAsAsync<Products>().Result;
            var productsArray = products.Items.ToArray();
            Assert.IsTrue(productsArray.Length == 1);
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.Product1, productsArray[0]), "Product1");
        }

        [TestMethod]
        public void RefactionService_UpdateProduct_UsingEmptyDatabase()
        {
            UseEmptyDatabase();

            var product = SampleModels.Product1;
            product.DeliveryPrice = 1234.56M;

            var response = _client.PutAsJsonAsync<Product>($"http://testserver/products/{SampleModels.Product1.Id}", product).Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void RefactionService_UpdateProductUsingSampleData()
        {
            UseSampleDatabase();

            var submittedProduct = SampleModels.Product1;
            submittedProduct.DeliveryPrice = 1234.56M;

            var response = _client.PutAsJsonAsync<Product>($"http://testserver/products/{submittedProduct.Id}", submittedProduct).Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var updatedResponse = _client.GetAsync($"http://testserver/products/{submittedProduct.Id}").Result;
            Assert.IsTrue(updatedResponse.StatusCode == System.Net.HttpStatusCode.OK);

            var updatedProduct = updatedResponse.Content.ReadAsAsync<Product>().Result;
            Assert.IsTrue(ModelComparer.AreEquivalent(submittedProduct, updatedProduct));
        }

        [TestMethod]
        public void RefactionService_DeleteProduct_UsingEmptyDatabase()
        {
            UseEmptyDatabase();

            var response = _client.DeleteAsync($"http://testserver/products/{SampleModels.Product1.Id}").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void RefactionService_DeleteProductUsingSampleData()
        {
            UseSampleDatabase();

            var response = _client.DeleteAsync($"http://testserver/products/{SampleModels.Product1.Id}").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var retrieveResponse = _client.GetAsync($"http://testserver/products").Result;
            Assert.IsTrue(retrieveResponse.StatusCode == System.Net.HttpStatusCode.OK);

            var products = retrieveResponse.Content.ReadAsAsync<Products>().Result;
            var productsArray = products.Items.ToArray();
            Assert.IsTrue(productsArray.Length == 2);
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.Product0, productsArray[0]));
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.Product2, productsArray[1]));
        }

        ////////////////////////////
        ////////////////////////////
        ////////////////////////////
        //  Product Option Tests  //
        ////////////////////////////
        ////////////////////////////
        ////////////////////////////

        [TestMethod]
        public void RefactionService_CreateProductOption_UsingEmptyDatabase()
        {
            UseEmptyDatabase();

            var submittedProductOption =
                new ProductOption
                {
                    Description = "The created product option",
                    Id = Guid.NewGuid(),
                    Name = "ProductOption 5",
                };

            var response = _client.PostAsJsonAsync($"http://testserver/product/{SampleModels.Product0.Id}/options", submittedProductOption).Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void RefactionService_CreateProductOption_UsingSampleDatabase()
        {
            UseSampleDatabase();

            var submittedProductOption =
                new ProductOption
                {
                    Description = "The created productOption",
                    Id = Guid.NewGuid(),
                    Name = "ProductOption 4",
                    ProductId = SampleModels.Product0.Id,
                };

            var location = $"http://testserver/product/{submittedProductOption.ProductId}/options";

            var response = _client.PostAsJsonAsync($"http://testserver/products/{submittedProductOption.ProductId}/options", submittedProductOption).Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var retrieveResponse = _client.GetAsync($"http://testserver/products/{submittedProductOption.ProductId}/options").Result;
            Assert.IsTrue(retrieveResponse.StatusCode == System.Net.HttpStatusCode.OK);

            var productOptions = retrieveResponse.Content.ReadAsAsync<ProductOptions>().Result;
            var productOptionsArray = productOptions.Items.ToArray();

            // by design, the product id for the product options model will never be serialized
            // force the product id assignment here...
            foreach (var productOption in productOptionsArray)
            {
                productOption.ProductId = submittedProductOption.ProductId;
            }

            Assert.IsTrue(productOptionsArray.Length == 3);
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.ProductOption0, productOptionsArray[0]));
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.ProductOption2, productOptionsArray[1]));

            var createdProductOption = productOptionsArray[2];
            Assert.AreNotEqual(submittedProductOption.Id, createdProductOption.Id);
            createdProductOption.Id = submittedProductOption.Id;
            Assert.IsTrue(ModelComparer.AreEquivalent(submittedProductOption, createdProductOption));
        }

        [TestMethod]
        public void RefactionService_GetProductOptionsByProductId_UsingEmptyDatabase()
        {
            UseEmptyDatabase();

            var response = _client.GetAsync($"http://testserver/products/{SampleModels.Product1.Id}/options").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var productOptions = response.Content.ReadAsAsync<ProductOptions>().Result;
            var productOptionsArray = productOptions.Items.ToArray();
            Assert.IsTrue(productOptionsArray.Length == 0);
        }

        [TestMethod]
        public void RefactionService_GetProductOptionsUsingSampleData()
        {
            UseSampleDatabase();

            var response = _client.GetAsync($"http://testserver/products/{SampleModels.Product1.Id}/options").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var productOptions = response.Content.ReadAsAsync<ProductOptions>().Result;
            var productOptionsArray = productOptions.Items.ToArray();

            // by design, the product id for the product options model will never be serialized
            // force the product id assignment here...
            foreach (var productOption in productOptionsArray)
            {
                productOption.ProductId = SampleModels.Product1.Id;
            }

            Assert.IsTrue(productOptionsArray.Length == 2);
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.ProductOption1, productOptionsArray[0]), "ProductOption1");
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.ProductOption3, productOptionsArray[1]), "ProductOption3");
        }

        [TestMethod]
        public void RefactionService_GetProductOptionByBothIds_UsingEmptyDatabase()
        {
            UseEmptyDatabase();

            var response = _client.GetAsync($"http://testserver/products/{SampleModels.Product1.Id}/options/{SampleModels.ProductOption3.Id}").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void RefactionService_GetProductOptionByBothIdsUsingSampleData()
        {
            UseSampleDatabase();

            var response = _client.GetAsync($"http://testserver/products/{SampleModels.Product1.Id}/options/{SampleModels.ProductOption3.Id}").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var productOption = response.Content.ReadAsAsync<ProductOption>().Result;

            // by design, the product id for the product options model will never be serialized
            // force the product id assignment here...
            productOption.ProductId = SampleModels.Product1.Id;

            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.ProductOption3, productOption));
        }

        [TestMethod]
        public void RefactionService_UpdateProductOption_UsingEmptyDatabase()
        {
            UseEmptyDatabase();

            var submittedProductOption = SampleModels.ProductOption1;
            submittedProductOption.Name = "The updated product option";

            var response = _client.PutAsJsonAsync<ProductOption>($"http://testserver/products/{SampleModels.Product1.Id}/options/{submittedProductOption.Id}", submittedProductOption).Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void RefactionService_UpdateProductOptionUsingSampleData()
        {
            UseSampleDatabase();

            var submittedProductOption = SampleModels.ProductOption1;
            submittedProductOption.Name = "The updated product option";

            var response = _client.PutAsJsonAsync<ProductOption>($"http://testserver/products/{submittedProductOption.ProductId}/options/{submittedProductOption.Id}", submittedProductOption).Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var updatedResponse = _client.GetAsync($"http://testserver/products/{submittedProductOption.ProductId}/options/{submittedProductOption.Id}").Result;
            Assert.IsTrue(updatedResponse.StatusCode == System.Net.HttpStatusCode.OK);

            var updatedProductOption = updatedResponse.Content.ReadAsAsync<ProductOption>().Result;

            // by design, the product id for the product options model will never be serialized
            // force the product id assignment here...
            updatedProductOption.ProductId = submittedProductOption.ProductId;

            Assert.IsTrue(ModelComparer.AreEquivalent(submittedProductOption, updatedProductOption));
        }

        [TestMethod]
        public void RefactionService_DeleteProductOption_UsingEmptyDatabase()
        {
            UseEmptyDatabase();

            var response = _client.DeleteAsync($"http://testserver/products/{SampleModels.ProductOption1.Id}/options/{SampleModels.ProductOption1.Id}").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void RefactionService_DeleteProductOptionUsingSampleData()
        {
            UseSampleDatabase();

            var response = _client.DeleteAsync($"http://testserver/products/{SampleModels.Product1.Id}/options/{SampleModels.ProductOption1.Id}").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var retrieveResponse = _client.GetAsync($"http://testserver/products/{SampleModels.Product1.Id}/options").Result;
            Assert.IsTrue(retrieveResponse.StatusCode == System.Net.HttpStatusCode.OK);

            var productOptions = retrieveResponse.Content.ReadAsAsync<ProductOptions>().Result;
            var productOptionsArray = productOptions.Items.ToArray();

            // by design, the product id for the product options model will never be serialized
            // force the product id assignment here...
            foreach (var productOption in productOptionsArray)
            {
                productOption.ProductId = SampleModels.Product1.Id;
            }

            Assert.IsTrue(productOptionsArray.Length == 1);
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.ProductOption3, productOptionsArray[0]));
        }
    }
}
