using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Net.Http;

using Microsoft.Owin.Testing;
using Refaction.Service.Models;
using Refaction.Data;
using Refaction.Data.Fakes;
using Refaction.Common;
using Refaction.Service;
using Refaction.Tests;

namespace Refaction.UnitTests.UnitTests
{
    [TestClass]
    public class RefactionServiceIntegrationTests : RefactionUnitTestBase
    {
        TestServer _server;
        HttpClient _client;

        [TestInitialize]
        public void Initialize()
        {
            _server = TestServer.Create<Refaction.Service.OwinStartup>();
            _client = new HttpClient(_server.Handler);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _client.Dispose();
            _server.Dispose();
        }


        [TestMethod]
        public void RefactionService_CreateProductUsingEmptyDatabase()
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
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NoContent);

            var retrieveResponse = _client.GetAsync($"http://testserver/products").Result;
            Assert.IsTrue(retrieveResponse.StatusCode == System.Net.HttpStatusCode.OK);

            var products = retrieveResponse.Content.ReadAsAsync<Products>().Result;
            var productsArray = products.Items.ToArray();
            Assert.IsTrue(productsArray.Length == 1);

            // the returned product id will never match, so verify each field independently
            var createdProduct = productsArray[0];
            Assert.AreEqual(submittedProduct.DeliveryPrice, createdProduct.DeliveryPrice);
            Assert.AreEqual(submittedProduct.Description, createdProduct.Description);
            Assert.AreNotEqual(submittedProduct.Id, createdProduct.Id);
            Assert.AreEqual(submittedProduct.Name, createdProduct.Name);
            Assert.AreEqual(submittedProduct.Price, createdProduct.Price);
        }

        [TestMethod]
        public void RefactionService_CreateProductUsingSampleDatabase()
        {
            UseDatabaseWithSampleData();

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
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NoContent);

            var retrieveResponse = _client.GetAsync($"http://testserver/products").Result;
            Assert.IsTrue(retrieveResponse.StatusCode == System.Net.HttpStatusCode.OK);

            var products = retrieveResponse.Content.ReadAsAsync<Products>().Result;
            var productsArray = products.Items.ToArray();
            Assert.IsTrue(productsArray.Length == 4);
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.Product0, productsArray[0]));
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.Product1, productsArray[1]));
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.Product2, productsArray[2]));

            // the returned product id will never match, so verify each field independently
            var createdProduct = productsArray[3];
            Assert.AreEqual(submittedProduct.DeliveryPrice, createdProduct.DeliveryPrice);
            Assert.AreEqual(submittedProduct.Description, createdProduct.Description);
            Assert.AreNotEqual(submittedProduct.Id, createdProduct.Id);
            Assert.AreEqual(submittedProduct.Name, createdProduct.Name);
            Assert.AreEqual(submittedProduct.Price, createdProduct.Price);
        }

        [TestMethod]
        public void RefactionService_GetAllProductsUsingEmptyDatabase()
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
            UseDatabaseWithSampleData();

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
        public void RefactionService_GetProductByIdUsingEmptyDatabase()
        {
            UseEmptyDatabase();

            var response = _client.GetAsync($"http://testserver/products/{SampleModels.Product1.Id}").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void RefactionService_GetProductByIdUsingSampleData()
        {
            UseDatabaseWithSampleData();

            var response = _client.GetAsync($"http://testserver/products/{SampleModels.Product1.Id}").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var product = response.Content.ReadAsAsync<Product>().Result;
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.Product1, product));
        }

        [TestMethod]
        public void RefactionService_GetProductsByNameUsingEmptyDatabase()
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
            UseDatabaseWithSampleData();

            var response = _client.GetAsync("http://testserver/products?name=1").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var products = response.Content.ReadAsAsync<Products>().Result;
            var productsArray = products.Items.ToArray();
            Assert.IsTrue(productsArray.Length == 1);
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.Product1, productsArray[0]), "Product1");
        }

        [TestMethod]
        public void RefactionService_UpdateProductUsingEmptyDatabase()
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
            UseDatabaseWithSampleData();

            var submittedProduct = SampleModels.Product1;
            submittedProduct.DeliveryPrice = 1234.56M;

            var response = _client.PutAsJsonAsync<Product>($"http://testserver/products/{submittedProduct.Id}", submittedProduct).Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NoContent);

            var updatedResponse = _client.GetAsync($"http://testserver/products/{submittedProduct.Id}").Result;
            Assert.IsTrue(updatedResponse.StatusCode == System.Net.HttpStatusCode.OK);

            var updatedProduct = updatedResponse.Content.ReadAsAsync<Product>().Result;
            Assert.IsTrue(ModelComparer.AreEquivalent(submittedProduct, updatedProduct));
        }

        [TestMethod]
        public void RefactionService_DeleteProductUsingEmptyDatabase()
        {
            UseEmptyDatabase();

            var response = _client.DeleteAsync($"http://testserver/products/{SampleModels.Product1.Id}").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void RefactionService_DeleteProductUsingSampleData()
        {
            UseDatabaseWithSampleData();

            var response = _client.DeleteAsync($"http://testserver/products/{SampleModels.Product1.Id}").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NoContent);

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
        public void RefactionService_CreateProductOptionUsingEmptyDatabase()
        {
            UseEmptyDatabase();

            var submittedProductOption =
                new ProductOption
                {
                    Description = "The created product option",
                    Id = Guid.NewGuid(),
                    Name = "ProductOption 5",
                    ProductId = SampleModels.Product0.Id
                };

            var response = _client.PostAsJsonAsync($"http://testserver/product/{submittedProductOption.ProductId}/options", submittedProductOption).Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void RefactionService_CreateProductOptionUsingSampleDatabase()
        {
            UseDatabaseWithSampleData();

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
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NoContent);

            var retrieveResponse = _client.GetAsync($"http://testserver/products/{submittedProductOption.ProductId}/options").Result;
            Assert.IsTrue(retrieveResponse.StatusCode == System.Net.HttpStatusCode.OK);

            var productOptions = retrieveResponse.Content.ReadAsAsync<ProductOptions>().Result;
            var productOptionsArray = productOptions.Items.ToArray();
            Assert.IsTrue(productOptionsArray.Length == 3);
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.ProductOption0, productOptionsArray[0]));
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.ProductOption2, productOptionsArray[1]));

            // the returned productOption id will never match, so verify each field independently
            var createdProductOption = productOptionsArray[2];
            Assert.AreEqual(submittedProductOption.Description, createdProductOption.Description);
            Assert.AreNotEqual(submittedProductOption.Id, createdProductOption.Id);
            Assert.AreEqual(submittedProductOption.Name, createdProductOption.Name);
            Assert.AreEqual(submittedProductOption.ProductId, createdProductOption.ProductId);
        }

        [TestMethod]
        public void RefactionService_GetProductOptionsByProductIdUsingEmptyDatabase()
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
            UseDatabaseWithSampleData();

            var response = _client.GetAsync($"http://testserver/products/{SampleModels.Product1.Id}/options").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var productOptions = response.Content.ReadAsAsync<ProductOptions>().Result;
            var productOptionsArray = productOptions.Items.ToArray();
            Assert.IsTrue(productOptionsArray.Length == 2);
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.ProductOption1, productOptionsArray[0]), "ProductOption1");
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.ProductOption3, productOptionsArray[1]), "ProductOption3");
        }

        [TestMethod]
        public void RefactionService_GetProductOptionByBothIdsUsingEmptyDatabase()
        {
            UseEmptyDatabase();

            var response = _client.GetAsync($"http://testserver/products/{SampleModels.Product1.Id}/options/{SampleModels.ProductOption3.Id}").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void RefactionService_GetProductOptionByBothIdsUsingSampleData()
        {
            UseDatabaseWithSampleData();

            var response = _client.GetAsync($"http://testserver/products/{SampleModels.Product1.Id}/options/{SampleModels.ProductOption3.Id}").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

            var productOption = response.Content.ReadAsAsync<ProductOption>().Result;
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.ProductOption3, productOption));
        }

        [TestMethod]
        public void RefactionService_UpdateProductOptionUsingEmptyDatabase()
        {
            UseEmptyDatabase();

            var submittedProductOption = SampleModels.ProductOption1;
            submittedProductOption.Name = "The updated product option";

            var response = _client.PutAsJsonAsync<ProductOption>($"http://testserver/products/{submittedProductOption.ProductId}/options/{submittedProductOption.Id}", submittedProductOption).Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void RefactionService_UpdateProductOptionUsingSampleData()
        {
            UseDatabaseWithSampleData();

            var submittedProductOption = SampleModels.ProductOption1;
            submittedProductOption.Name = "The updated product option";

            var response = _client.PutAsJsonAsync<ProductOption>($"http://testserver/products/{submittedProductOption.ProductId}/options/{submittedProductOption.Id}", submittedProductOption).Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NoContent);

            var updatedResponse = _client.GetAsync($"http://testserver/products/{submittedProductOption.ProductId}/options/{submittedProductOption.Id}").Result;
            Assert.IsTrue(updatedResponse.StatusCode == System.Net.HttpStatusCode.OK);

            var updatedProductOption = updatedResponse.Content.ReadAsAsync<ProductOption>().Result;
            Assert.IsTrue(ModelComparer.AreEquivalent(submittedProductOption, updatedProductOption));
        }

        [TestMethod]
        public void RefactionService_DeleteProductOptionUsingEmptyDatabase()
        {
            UseEmptyDatabase();

            var response = _client.DeleteAsync($"http://testserver/products/{SampleModels.ProductOption1.Id}/options/{SampleModels.ProductOption1.Id}").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void RefactionService_DeleteProductOptionUsingSampleData()
        {
            UseDatabaseWithSampleData();

            var response = _client.DeleteAsync($"http://testserver/products/{SampleModels.Product1.Id}/options/{SampleModels.ProductOption1.Id}").Result;
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NoContent);

            var retrieveResponse = _client.GetAsync($"http://testserver/products/{SampleModels.Product1.Id}/options").Result;
            Assert.IsTrue(retrieveResponse.StatusCode == System.Net.HttpStatusCode.OK);

            var productOptions = retrieveResponse.Content.ReadAsAsync<ProductOptions>().Result;
            var productOptionsArray = productOptions.Items.ToArray();
            Assert.IsTrue(productOptionsArray.Length == 1);
            Assert.IsTrue(ModelComparer.AreEquivalent(SampleModels.ProductOption3, productOptionsArray[0]));
        }
    }
}
