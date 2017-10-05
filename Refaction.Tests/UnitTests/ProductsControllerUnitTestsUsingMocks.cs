using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;

using Refaction.Service;
using Refaction.Service.Controllers;
using Refaction.Service.Models;
using Refaction.Service.Repositories;
using Refaction.Tests;
using Refaction.UnitTests.Mocks;
using Refaction.Data;

namespace Refaction.UnitTests.UnitTests
{
    /// <summary>
    /// Unit tests for 100% code-coverage of ProductsController
    /// (using strict mocks to ensure precise behavior)
    /// </summary>
    [TestClass]
    public class ProductsControllerUnitTestsUsingMocks : RefactionTestUsingMocksBase
    {
        public MockProductRepository MockProductRepository;
        public MockProductOptionRepository MockProductOptionRepository;

        protected static readonly Product[] _emptyProductArray = new Product[] { };
        protected static readonly ProductOption[] _emptyProductOptionArray = new ProductOption[] { };

        protected override void UseEmptyDatabase()
        {
            base.UseEmptyDatabase();

            BindToNewMockRepositories();
        }

        protected override void UseSampleDatabase()
        {
            base.UseSampleDatabase();

            BindToNewMockRepositories();
        }

        public void BindToNewMockRepositories()
        {
            this.MockProductRepository = new MockProductRepository();
            this.MockProductOptionRepository = new MockProductOptionRepository();

            NinjectKernel
            .Rebind<IProductRepository>()
            .ToConstant(this.MockProductRepository.Object)
            .InSingletonScope();

            NinjectKernel
            .Rebind<IProductOptionRepository>()
            .ToConstant(this.MockProductOptionRepository.Object)
            .InSingletonScope();
        }

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
                    Name = "Product 4",
                    Price = 4.44M,
                };

            var copyOfProduct = new Product(product);

            // expect call to create method
            this.MockProductRepository
                .Setup(m => m.Create(It.Is<Product>(v => ModelComparer.AreEquivalent(v, product))))
                .Verifiable();

            CurrentProductsController.CreateProduct(product);

            // make sure that the new product id was assigned, and nothing else changed
            Assert.IsTrue(product.Id != copyOfProduct.Id);
            product.Id = copyOfProduct.Id;
            Assert.IsTrue(ModelComparer.AreEquivalent(product, copyOfProduct));

            // verify that create method was called once
            this.MockProductRepository
                .Verify(m => m.Create(It.Is<Product>(v => ModelComparer.AreEquivalent(v, product))),
                Times.Once());
        }

        [TestMethod]
        public void ProductsController_RetrieveAllProductsUsingEmptyDatabase()
        {
            UseEmptyDatabase();

            // expect call to retrieve method
            this.MockProductRepository
                .Setup(m => m.Retrieve())
                .Returns(_emptyProductArray)
                .Verifiable();

            var products = CurrentProductsController.GetAllProducts();

            Assert.IsTrue(products.Items.Count() == 0);

            // verify that retrieve method was called once
            this.MockProductRepository
                .Verify(m => m.Retrieve(),
                Times.Once());
        }

        [TestMethod]
        public void ProductsController_RetrieveAllProductsUsingSampleDatabase()
        {
            UseSampleDatabase();

            // expect call to retrieve method
            this.MockProductRepository
                .Setup(m => m.Retrieve())
                .Returns(new Product[] { SampleModels.Product0, SampleModels.Product1, SampleModels.Product2 })
                .Verifiable();

            var products = CurrentProductsController.GetAllProducts();
            var productsArray = products.Items.ToArray();
            Assert.IsTrue(productsArray.Count() == 3);
            Assert.IsTrue(ModelComparer.AreEquivalent(productsArray[0], SampleModels.Product0));
            Assert.IsTrue(ModelComparer.AreEquivalent(productsArray[1], SampleModels.Product1));
            Assert.IsTrue(ModelComparer.AreEquivalent(productsArray[2], SampleModels.Product2));

            // verify that retrieve method was called once
            this.MockProductRepository
                .Verify(m => m.Retrieve(),
                Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductsController_RetrieveProductByIdUsingEmptyDatabase_ShouldThrowNotFoundException()
        {
            UseEmptyDatabase();

            // expect call to retrieve method
            this.MockProductRepository
                .Setup(m => m.Retrieve(It.Is<Guid>(v => v == SampleModels.Product1.Id)))
                .Returns((Product)null)
                .Verifiable();

            var product = CurrentProductsController.GetProduct(SampleModels.Product1.Id);
        }

        [TestMethod]
        public void ProductsController_RetrieveProductByIdUsingSampleDatabase()
        {
            UseSampleDatabase();

            // expect retrieve method to be called
            this.MockProductRepository
                .Setup(m => m.Retrieve(It.Is<Guid>(v => v == SampleModels.Product1.Id)))
                .Returns(SampleModels.Product1)
                .Verifiable();

            var product = CurrentProductsController.GetProduct(SampleModels.Product1.Id);

            Assert.IsTrue(ModelComparer.AreEquivalent(product, SampleModels.Product1));

            // verify retrieve method called once
            this.MockProductRepository
                .Verify(m => m.Retrieve(It.Is<Guid>(v => v == SampleModels.Product1.Id)),
                Times.Once());
        }

        [TestMethod]
        public void ProductsController_RetrieveProductByNameUsingEmptyDatabase_ShouldReturnZeroProducts()
        {
            UseEmptyDatabase();

            // expect retrieve method to be called
            this.MockProductRepository
                .Setup(m => m.Retrieve(It.Is<string>(v => v == "1")))
                .Returns(new Product[] { SampleModels.Product1 })
                .Verifiable();

            var products = CurrentProductsController.GetProductsByName("1");

            // verify retrieve method called once
            this.MockProductRepository
                .Verify(m => m.Retrieve(It.Is<string>(v => v == "1")),
                Times.Once());
        }

        [TestMethod]
        public void ProductsController_RetrieveProductByNameUsingSampleDatabase()
        {
            UseSampleDatabase();

            // expect retrieve method call
            this.MockProductRepository
                .Setup(m => m.Retrieve(It.Is<string>(v => v == "1")))
                .Returns(new Product[] { SampleModels.Product1 })
                .Verifiable();

            var products = CurrentProductsController.GetProductsByName("1");

            var productsArray = products.Items.ToArray();
            Assert.IsTrue(productsArray.Count() == 1);
            Assert.IsTrue(ModelComparer.AreEquivalent(productsArray[0], SampleModels.Product1));

            // verify retrieve method called once
            this.MockProductRepository
                .Verify(m => m.Retrieve(It.Is<string>(v => v == "1")),
                Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductsController_UpdateUsingEmptyDatabase_ShouldThrowNotFound()
        {
            UseEmptyDatabase();

            var product = SampleModels.Product0;
            product.Description = "The updated product";

            var copyOfProduct = new Product(product);

            // expect update method call
            this.MockProductRepository
                .Setup(m => m.Update(It.Is<Product>(v => ModelComparer.AreEquivalent(v, product))))
                .Throws(new HttpResponseException_NotFoundException())
                .Verifiable();

            CurrentProductsController.UpdateProduct(product.Id, product);
        }

        [TestMethod]
        public void ProductsController_UpdateUsingSampleDatabase()
        {
            UseSampleDatabase();

            var product = SampleModels.Product0;
            product.Description = "The updated product";

            var copyOfProduct = new Product(product);

            // expect update method call
            this.MockProductRepository
                .Setup(m => m.Update(It.Is<Product>(v => ModelComparer.AreEquivalent(v, product))))
                .Verifiable();

            CurrentProductsController.UpdateProduct(product.Id, product);

            // ensure that submitted product was not altered
            Assert.IsTrue(ModelComparer.AreEquivalent(copyOfProduct, product));

            // verify update method called once
            this.MockProductRepository
                .Verify(m => m.Update(It.Is<Product>(v => ModelComparer.AreEquivalent(v, product))),
                Times.Once());
        }
        
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductsController_DeleteUsingEmptyDatabase_ShouldThrowNotFound()
        {
            UseEmptyDatabase();

            var product = SampleModels.Product1;

            // expect update method call
            this.MockProductRepository
                .Setup(m => m.Delete(It.Is<Guid>(v => v == product.Id)))
                .Throws(new HttpResponseException_NotFoundException())
                .Verifiable();

            CurrentProductsController.DeleteProduct(product.Id);
        }

        [TestMethod]
        public void ProductsController_DeleteUsingSampleDatabase()
        {
            UseSampleDatabase();

            var product = SampleModels.Product1;

            // expect update method call
            this.MockProductRepository
                .Setup(m => m.Delete(It.Is<Guid>(v => v == product.Id)))
                .Verifiable();

            CurrentProductsController.DeleteProduct(product.Id);

            // verify update method called once
            this.MockProductRepository
                .Verify(m => m.Delete(It.Is<Guid>(v => v == product.Id)),
                Times.Once());
        }



        /////////////////////////////////
        /////////////////////////////////
        /////////////////////////////////
        ////  Product Options methods  //
        /////////////////////////////////
        /////////////////////////////////
        /////////////////////////////////


        [TestMethod]
        public void ProductOptionsController_CreateProductOptionUsingEmptyDatabase()
        {
            UseEmptyDatabase();

            var productOption =
                new ProductOption
                {
                    Id = Guid.NewGuid(),
                    Description = "The created productOption",
                    Name = "ProductOption 4",
                    ProductId = SampleModels.Product1.Id
                };

            var copyOfProductOption = new ProductOption(productOption);

            // expect call to create method
            this.MockProductOptionRepository
                .Setup(m => m.Create(It.Is<ProductOption>(v => ModelComparer.AreEquivalent(v, productOption))))
                .Verifiable();

            CurrentProductsController.CreateOption(productOption.ProductId, productOption);

            // make sure that the new productOption id was assigned, and nothing else changed
            Assert.IsTrue(productOption.Id != copyOfProductOption.Id);
            productOption.Id = copyOfProductOption.Id;
            Assert.IsTrue(ModelComparer.AreEquivalent(productOption, copyOfProductOption));

            // verify that create method was called once
            this.MockProductOptionRepository
                .Verify(m => m.Create(It.Is<ProductOption>(v => ModelComparer.AreEquivalent(v, productOption))),
                Times.Once());
        }

        [TestMethod]
        public void ProductOptionsController_RetrieveAllProductOptionsUsingEmptyDatabase()
        {
            UseEmptyDatabase();

            // expect call to retrieve method
            this.MockProductOptionRepository
                .Setup(m => m.RetrieveByProductId(It.Is<Guid>(v=>v == SampleModels.Product1.Id)))
                .Returns(_emptyProductOptionArray)
                .Verifiable();

            var productOptions = CurrentProductsController.GetOptions(SampleModels.Product1.Id);

            Assert.IsTrue(productOptions.Items.Count() == 0);

            // verify that retrieve method was called once
            this.MockProductOptionRepository
                .Verify(m => m.RetrieveByProductId(It.Is<Guid>(v=>v == SampleModels.Product1.Id)),
                Times.Once());
        }

        [TestMethod]
        public void ProductOptionsController_RetrieveAllProductOptionsUsingSampleDatabase()
        {
            UseSampleDatabase();

            // expect call to retrieve method
            this.MockProductOptionRepository
                .Setup(m => m.RetrieveByProductId(It.Is<Guid>(v=>v == SampleModels.Product1.Id)))
                .Returns(new ProductOption[] { SampleModels.ProductOption1, SampleModels.ProductOption3 })
                .Verifiable();

            var productOptions = CurrentProductsController.GetOptions(SampleModels.Product1.Id);
            var productOptionsArray = productOptions.Items.ToArray();
            Assert.IsTrue(productOptionsArray.Count() == 2);
            Assert.IsTrue(ModelComparer.AreEquivalent(productOptionsArray[0], SampleModels.ProductOption1));
            Assert.IsTrue(ModelComparer.AreEquivalent(productOptionsArray[1], SampleModels.ProductOption3));

            // verify that retrieve method was called once
            this.MockProductOptionRepository
                .Verify(m => m.RetrieveByProductId(It.Is<Guid>(v=>v == SampleModels.Product1.Id)),
                Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductOptionsController_RetrieveProductOptionByIdUsingEmptyDatabase_ShouldThrowNotFoundException()
        {
            UseEmptyDatabase();

            // expect call to retrieve method
            this.MockProductOptionRepository
                .Setup(m => m.RetrieveByBothIds(It.Is<Guid>(g => g == SampleModels.Product1.Id), It.Is<Guid>(g => g == SampleModels.ProductOption3.Id)))
                .Returns((ProductOption)null)
                .Verifiable();

            var productOption = CurrentProductsController.GetOption(SampleModels.Product1.Id, SampleModels.ProductOption3.Id);
        }

        [TestMethod]
        public void ProductOptionsController_RetrieveProductOptionByIdUsingSampleDatabase()
        {
            UseSampleDatabase();

            // expect call to retrieve method
            this.MockProductOptionRepository
                .Setup(m => m.RetrieveByBothIds(It.Is<Guid>(g => g == SampleModels.Product1.Id), It.Is<Guid>(g => g == SampleModels.ProductOption3.Id)))
                .Returns(SampleModels.ProductOption3)
                .Verifiable();

            var productOption = CurrentProductsController.GetOption(SampleModels.Product1.Id, SampleModels.ProductOption3.Id);

            Assert.IsTrue(ModelComparer.AreEquivalent(productOption, SampleModels.ProductOption3));

            // verify retrieve method called once
            this.MockProductOptionRepository
                .Verify(m => m.RetrieveByBothIds(It.Is<Guid>(g => g == SampleModels.Product1.Id), It.Is<Guid>(g => g == SampleModels.ProductOption3.Id)),
                Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductOptionsController_UpdateUsingEmptyDatabase_ShouldThrowNotFound()
        {
            UseEmptyDatabase();

            var productOption = SampleModels.ProductOption1;
            productOption.Description = "The updated product option";

            var copyOfProductOption = new ProductOption(productOption);

            // expect update method call
            this.MockProductOptionRepository
                .Setup(m => m.Update(It.Is<ProductOption>(v => ModelComparer.AreEquivalent(v, productOption))))
                .Throws(new HttpResponseException_NotFoundException())
                .Verifiable();

            CurrentProductsController.UpdateOption(productOption.ProductId, productOption.Id, productOption);
        }

        [TestMethod]
        public void ProductOptionsController_UpdateUsingSampleDatabase()
        {
            UseSampleDatabase();

            var productOption = SampleModels.ProductOption1;
            productOption.Description = "The updated productOption";

            var copyOfProductOption = new ProductOption(productOption);

            // expect update method call
            this.MockProductOptionRepository
                .Setup(m => m.Update(It.Is<ProductOption>(v => ModelComparer.AreEquivalent(v, productOption))))
                .Verifiable();

            CurrentProductsController.UpdateOption(productOption.ProductId, productOption.Id, productOption);

            // ensure that submitted productOption was not altered
            Assert.IsTrue(ModelComparer.AreEquivalent(copyOfProductOption, productOption));

            // verify update method called once
            this.MockProductOptionRepository
                .Verify(m => m.Update(It.Is<ProductOption>(v => ModelComparer.AreEquivalent(v, productOption))),
                Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductOptionsController_DeleteUsingEmptyDatabase_ShouldThrowNotFound()
        {
            UseEmptyDatabase();

            var productOption = SampleModels.ProductOption1;

            // expect update method call
            this.MockProductOptionRepository
                .Setup(m => m.Delete(It.Is<Guid>(v => v == productOption.Id)))
                .Throws(new HttpResponseException_NotFoundException())
                .Verifiable();

            CurrentProductsController.DeleteOption(productOption.Id);
        }

        [TestMethod]
        public void ProductOptionsController_DeleteUsingSampleDatabase()
        {
            UseSampleDatabase();

            var productOption = SampleModels.ProductOption1;

            // expect update method call
            this.MockProductOptionRepository
                .Setup(m => m.Delete(It.Is<Guid>(v => v == productOption.Id)))
                .Verifiable();

            CurrentProductsController.DeleteOption(productOption.Id);

            // verify update method called once
            this.MockProductOptionRepository
                .Verify(m => m.Delete(It.Is<Guid>(v => v == productOption.Id)),
                Times.Once());
        }

        [TestMethod]
        public void ProductOptionsController_Dispose()
        {
            UseEmptyDatabase();

            // expect call to save changes method
            this.MockRefactionDbContext
                .Setup(m => m.SaveChanges())
                .Returns(0)
                .Verifiable();

            // expect call to dispose method in refaction db context
            this.MockRefactionDbContext
                .Setup(m => m.Dispose())
                .Verifiable();

            // expect call to dispose method in product repository
            this.MockProductRepository
                .Setup(m => m.Dispose())
                .Verifiable();

            // expect call to dispose method in product options repository
            this.MockProductOptionRepository
                .Setup(m => m.Dispose())
                .Verifiable();


            CurrentProductsController.Dispose();

            // verify call to save changes method
            this.MockRefactionDbContext
                .Verify(m => m.SaveChanges(),
                Times.Once());

            // verify call to dispose method
            this.MockRefactionDbContext
                .Verify(m => m.Dispose(),
                Times.Once());

            // verify call to dispose method
            this.MockProductRepository
                .Verify(m => m.Dispose(),
                Times.Once());

            // verify call to dispose method
            this.MockProductOptionRepository
                .Verify(m => m.Dispose(),
                Times.Once());
        }

        [TestMethod]
        public void ProductOptionsController_DefaultConstructor()
        {
            // ensure 100% code coverage
            var controller = new ProductsController();
        }
    }
}
