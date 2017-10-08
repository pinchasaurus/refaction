using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Ninject;
using Refaction.Common;
using Refaction.Data;
using Refaction.Service;
using Refaction.Service.Controllers;
using Refaction.Service.Models;
using Refaction.Service.Repositories;
using Refaction.Tests;
using Refaction.UnitTests.Mocks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

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
        protected ProductsController _currentProductsController;

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

            var oldController = _currentProductsController;

            _currentProductsController = NinjectKernel.Get<ProductsController>();

            Assert.AreNotSame(oldController, _currentProductsController);
        }

        public ProductsController CurrentProductsController
        {
            get { return _currentProductsController; }
        }

        [TestMethod]
        public void ProductsController_CreateProduct_UsingEmptyDatabase()
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

            var actionResult = CurrentProductsController.CreateProduct(product);

            var contentResult = actionResult as OkResult;
            Assert.IsNotNull(contentResult);

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
        public void ProductsController_CreateProduct_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            UseEmptyDatabase();

            CurrentProductsController.ModelState.AddModelError("productId", "fakeError");
            var actionResult = CurrentProductsController.CreateProduct(null);

            var contentResult = actionResult as BadRequestResult;
            Assert.IsNotNull(contentResult);
        }

        [TestMethod]
        public void ProductsController_RetrieveAllProducts_UsingEmptyDatabase()
        {
            UseEmptyDatabase();

            // expect call to retrieve method
            this.MockProductRepository
                .Setup(m => m.Retrieve())
                .Returns(_emptyProductArray)
                .Verifiable();

            var actionResult = CurrentProductsController.GetAllProducts();

            var contentResult = actionResult as OkNegotiatedContentResult<Products>;
            Assert.IsNotNull(contentResult);

            // verify that retrieve method was called once
            this.MockProductRepository
                .Verify(m => m.Retrieve(),
                Times.Once());
        }

        [TestMethod]
        public void ProductsController_RetrieveAllProducts_UsingSampleDatabase()
        {
            UseSampleDatabase();

            // expect call to retrieve method
            this.MockProductRepository
                .Setup(m => m.Retrieve())
                .Returns(new Product[] { SampleModels.Product0, SampleModels.Product1, SampleModels.Product2 })
                .Verifiable();

            var actionResult = CurrentProductsController.GetAllProducts();

            var contentResult = actionResult as OkNegotiatedContentResult<Products>;
            Assert.IsNotNull(contentResult);

            // verify that retrieve method was called once
            this.MockProductRepository
                .Verify(m => m.Retrieve(),
                Times.Once());
        }

        [TestMethod]
        public void ProductsController_RetrieveAllProducts_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            UseEmptyDatabase();

            CurrentProductsController.ModelState.AddModelError("productId", "fakeError");
            var actionResult = CurrentProductsController.GetAllProducts();

            var contentResult = actionResult as BadRequestResult;
            Assert.IsNotNull(contentResult);
        }

        [TestMethod]
        public void ProductsController_RetrieveProductById_UsingEmptyDatabase()
        {
            UseEmptyDatabase();

            // expect retrieve method to be called
            this.MockProductRepository
                .Setup(m => m.Retrieve(It.Is<Guid>(v => v == SampleModels.Product1.Id)))
                .Returns((Product)null)
                .Verifiable();

            var actionResult = CurrentProductsController.GetProduct(SampleModels.Product1.Id);

            var contentResult = actionResult as NotFoundResult;
            Assert.IsNotNull(contentResult);

            // verify retrieve method called once
            this.MockProductRepository
                .Verify(m => m.Retrieve(It.Is<Guid>(v => v == SampleModels.Product1.Id)),
                Times.Once());
        }

        [TestMethod]
        public void ProductsController_RetrieveProductById_UsingSampleDatabase()
        {
            UseSampleDatabase();

            // expect retrieve method to be called
            this.MockProductRepository
                .Setup(m => m.Retrieve(It.Is<Guid>(v => v == SampleModels.Product1.Id)))
                .Returns(SampleModels.Product1)
                .Verifiable();

            var actionResult = CurrentProductsController.GetProduct(SampleModels.Product1.Id);

            var contentResult = actionResult as OkNegotiatedContentResult<Product>;
            Assert.IsNotNull(contentResult);

            // verify retrieve method called once
            this.MockProductRepository
                .Verify(m => m.Retrieve(It.Is<Guid>(v => v == SampleModels.Product1.Id)),
                Times.Once());
        }

        [TestMethod]
        public void ProductsController_RetrieveProductById_ReturnsBadRequestResult_WhenModelStatIsInvalid()
        {
            UseEmptyDatabase();

            CurrentProductsController.ModelState.AddModelError("productId", "fakeError");
            var actionResult = CurrentProductsController.GetProduct(Guid.Empty);

            var contentResult = actionResult as BadRequestResult;
            Assert.IsNotNull(contentResult);
        }

        [TestMethod]
        public void ProductsController_RetrieveProductByName_UsingEmptyDatabase()
        {
            UseEmptyDatabase();

            // expect retrieve method to be called
            this.MockProductRepository
                .Setup(m => m.Retrieve(It.Is<string>(v => v == "1")))
                .Returns(new Product[] { SampleModels.Product1 })
                .Verifiable();

            var actionResult = CurrentProductsController.GetProductsByName("1");

            var contentResult = actionResult as OkNegotiatedContentResult<Products>;
            Assert.IsNotNull(contentResult);

            // verify retrieve method called once
            this.MockProductRepository
                .Verify(m => m.Retrieve(It.Is<string>(v => v == "1")),
                Times.Once());
        }

        [TestMethod]
        public void ProductsController_RetrieveProductByName_UsingSampleDatabase()
        {
            UseSampleDatabase();

            // expect retrieve method call
            this.MockProductRepository
                .Setup(m => m.Retrieve(It.Is<string>(v => v == "1")))
                .Returns(new Product[] { SampleModels.Product1 })
                .Verifiable();

            var actionResult = CurrentProductsController.GetProductsByName("1");

            var contentResult = actionResult as OkNegotiatedContentResult<Products>;
            Assert.IsNotNull(contentResult);

            // verify retrieve method called once
            this.MockProductRepository
                .Verify(m => m.Retrieve(It.Is<string>(v => v == "1")),
                Times.Once());
        }

        [TestMethod]
        public void ProductsController_RetrieveProductByName_ShouldReturnBadRequestResult_WhenModelStateIsInvalid()
        {
            UseEmptyDatabase();

            CurrentProductsController.ModelState.AddModelError("productId", "fakeError");
            var actionResult = CurrentProductsController.GetProductsByName(null);

            var contentResult = actionResult as BadRequestResult;
            Assert.IsNotNull(contentResult);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductsController_Update_UsingEmptyDatabase_ShouldThrowNotFound()
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

            var actionResult = CurrentProductsController.UpdateProduct(product.Id, product);

            var contentResult = actionResult as NotFoundResult;
            Assert.IsNotNull(contentResult);
        }

        [TestMethod]
        public void ProductsController_Update_UsingSampleDatabase()
        {
            UseSampleDatabase();

            var product = SampleModels.Product0;
            product.Description = "The updated product";

            var copyOfProduct = new Product(product);

            // expect update method call
            this.MockProductRepository
                .Setup(m => m.Update(It.Is<Product>(v => ModelComparer.AreEquivalent(v, product))))
                .Verifiable();

            var actionResult = CurrentProductsController.UpdateProduct(product.Id, product);

            var contentResult = actionResult as OkResult;
            Assert.IsNotNull(contentResult);

            // ensure that submitted product was not altered
            Assert.IsTrue(ModelComparer.AreEquivalent(copyOfProduct, product));

            // verify update method called once
            this.MockProductRepository
                .Verify(m => m.Update(It.Is<Product>(v => ModelComparer.AreEquivalent(v, product))),
                Times.Once());
        }

        [TestMethod]
        public void ProductsController_Update_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            UseEmptyDatabase();

            CurrentProductsController.ModelState.AddModelError("productId", "fakeError");
            var actionResult = CurrentProductsController.UpdateProduct(Guid.Empty, null);

            var contentResult = actionResult as BadRequestResult;
            Assert.IsNotNull(contentResult);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductsController_Delete_UsingEmptyDatabase_ShouldThrowNotFoundException()
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
        public void ProductsController_Delete_UsingSampleDatabase()
        {
            UseSampleDatabase();

            var product = SampleModels.Product1;

            // expect update method call
            this.MockProductRepository
                .Setup(m => m.Delete(It.Is<Guid>(v => v == product.Id)))
                .Verifiable();

            var actionResult = CurrentProductsController.DeleteProduct(product.Id);

            var contentResult = actionResult as OkResult;
            Assert.IsNotNull(contentResult);

            // verify update method called once
            this.MockProductRepository
                .Verify(m => m.Delete(It.Is<Guid>(v => v == product.Id)),
                Times.Once());
        }

        [TestMethod]
        public void ProductsController_Delete_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            UseEmptyDatabase();

            CurrentProductsController.ModelState.AddModelError("productId", "fakeError");
            var actionResult = CurrentProductsController.DeleteProduct(Guid.Empty);

            var contentResult = actionResult as BadRequestResult;
            Assert.IsNotNull(contentResult);
        }


        /////////////////////////////////
        /////////////////////////////////
        /////////////////////////////////
        ////  Product Options methods  //
        /////////////////////////////////
        /////////////////////////////////
        /////////////////////////////////


        [TestMethod]
        public void ProductsController_CreateProductOption_UsingEmptyDatabase()
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

            var actionResult = CurrentProductsController.CreateOption(productOption.ProductId, productOption);

            var contentResult = actionResult as OkResult;
            Assert.IsNotNull(contentResult);

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
        public void ProductsController_CreateProductOption_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            UseEmptyDatabase();

            CurrentProductsController.ModelState.AddModelError("productId", "fakeError");
            var actionResult = CurrentProductsController.CreateOption(Guid.Empty, null);

            var contentResult = actionResult as BadRequestResult;
            Assert.IsNotNull(contentResult);
        }

        [TestMethod]
        public void ProductsController_RetrieveAllProductOptions_UsingEmptyDatabase()
        {
            UseEmptyDatabase();

            // expect call to retrieve method
            this.MockProductOptionRepository
                .Setup(m => m.RetrieveByProductId(It.Is<Guid>(v => v == SampleModels.Product1.Id)))
                .Returns(_emptyProductOptionArray)
                .Verifiable();

            var actionResult = CurrentProductsController.GetOptions(SampleModels.Product1.Id);

            var contentResult = actionResult as OkNegotiatedContentResult<ProductOptions>;
            Assert.IsNotNull(contentResult);

            // verify that retrieve method was called once
            this.MockProductOptionRepository
                .Verify(m => m.RetrieveByProductId(It.Is<Guid>(v => v == SampleModels.Product1.Id)),
                Times.Once());
        }

        [TestMethod]
        public void ProductsController_RetrieveAllProductOptions_UsingSampleDatabase()
        {
            UseSampleDatabase();

            // expect call to retrieve method
            this.MockProductOptionRepository
                .Setup(m => m.RetrieveByProductId(It.Is<Guid>(v => v == SampleModels.Product1.Id)))
                .Returns(new ProductOption[] { SampleModels.ProductOption1, SampleModels.ProductOption3 })
                .Verifiable();

            var actionResult = CurrentProductsController.GetOptions(SampleModels.Product1.Id);

            var contentResult = actionResult as OkNegotiatedContentResult<ProductOptions>;
            Assert.IsNotNull(contentResult);

            // verify that retrieve method was called once
            this.MockProductOptionRepository
                .Verify(m => m.RetrieveByProductId(It.Is<Guid>(v => v == SampleModels.Product1.Id)),
                Times.Once());
        }

        [TestMethod]
        public void ProductsController_RetrieveAllProductOptions_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            UseEmptyDatabase();

            CurrentProductsController.ModelState.AddModelError("productId", "fakeError");
            var actionResult = CurrentProductsController.GetOptions(Guid.Empty);

            var contentResult = actionResult as BadRequestResult;
            Assert.IsNotNull(contentResult);
        }


        [TestMethod]
        public void ProductsController_RetrieveProductOptionById_UsingEmptyDatabase_ShouldThrowNotFoundException()
        {
            UseEmptyDatabase();

            // expect call to retrieve method
            this.MockProductOptionRepository
                .Setup(m => m.RetrieveByBothIds(It.Is<Guid>(g => g == SampleModels.Product1.Id), It.Is<Guid>(g => g == SampleModels.ProductOption3.Id)))
                .Returns((ProductOption)null)
                .Verifiable();

            var actionResult = CurrentProductsController.GetOption(SampleModels.Product1.Id, SampleModels.ProductOption3.Id);

            var contentResult = actionResult as NotFoundResult;
            Assert.IsNotNull(contentResult);
        }

        [TestMethod]
        public void ProductsController_RetrieveProductOptionById_UsingSampleDatabase()
        {
            UseSampleDatabase();

            // expect call to retrieve method
            this.MockProductOptionRepository
                .Setup(m => m.RetrieveByBothIds(It.Is<Guid>(g => g == SampleModels.Product1.Id), It.Is<Guid>(g => g == SampleModels.ProductOption3.Id)))
                .Returns(SampleModels.ProductOption3)
                .Verifiable();

            var actionResult = CurrentProductsController.GetOption(SampleModels.Product1.Id, SampleModels.ProductOption3.Id);

            var contentResult = actionResult as OkNegotiatedContentResult<ProductOption>;
            Assert.IsNotNull(contentResult);

            // verify retrieve method called once
            this.MockProductOptionRepository
                .Verify(m => m.RetrieveByBothIds(It.Is<Guid>(g => g == SampleModels.Product1.Id), It.Is<Guid>(g => g == SampleModels.ProductOption3.Id)),
                Times.Once());
        }

        [TestMethod]
        public void ProductsController_RetrieveProductOptionById_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            UseEmptyDatabase();

            CurrentProductsController.ModelState.AddModelError("productId", "fakeError");
            var actionResult = CurrentProductsController.GetOption(Guid.Empty, Guid.Empty);

            var contentResult = actionResult as BadRequestResult;
            Assert.IsNotNull(contentResult);
        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductsController_UpdateProductOption_UsingEmptyDatabase_ShouldThrowNotFound()
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
        public void ProductsController_UpdateProductOption_UsingSampleDatabase()
        {
            UseSampleDatabase();

            var productOption = SampleModels.ProductOption1;
            productOption.Description = "The updated productOption";

            var copyOfProductOption = new ProductOption(productOption);

            // expect update method call
            this.MockProductOptionRepository
                .Setup(m => m.Update(It.Is<ProductOption>(v => ModelComparer.AreEquivalent(v, productOption))))
                .Verifiable();

            var actionResult = CurrentProductsController.UpdateOption(productOption.ProductId, productOption.Id, productOption);

            var contentResult = actionResult as OkResult;
            Assert.IsNotNull(contentResult);

            // ensure that submitted productOption was not altered
            Assert.IsTrue(ModelComparer.AreEquivalent(copyOfProductOption, productOption));

            // verify update method called once
            this.MockProductOptionRepository
                .Verify(m => m.Update(It.Is<ProductOption>(v => ModelComparer.AreEquivalent(v, productOption))),
                Times.Once());
        }

        [TestMethod]
        public void ProductsController_UpdateProductOption_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            UseEmptyDatabase();

            CurrentProductsController.ModelState.AddModelError("productId", "fakeError");
            var actionResult = CurrentProductsController.UpdateOption(Guid.Empty, Guid.Empty, null);

            var contentResult = actionResult as BadRequestResult;
            Assert.IsNotNull(contentResult);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductsController_DeleteProductOption_UsingEmptyDatabase_ShouldThrowNotFound()
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
        public void ProductsController_DeleteProductOption_UsingSampleDatabase()
        {
            UseSampleDatabase();

            var productOption = SampleModels.ProductOption1;

            // expect update method call
            this.MockProductOptionRepository
                .Setup(m => m.Delete(It.Is<Guid>(v => v == productOption.Id)))
                .Verifiable();

            var actionResult = CurrentProductsController.DeleteOption(productOption.Id);

            var contentResult = actionResult as OkResult;
            Assert.IsNotNull(contentResult);

            // verify update method called once
            this.MockProductOptionRepository
                .Verify(m => m.Delete(It.Is<Guid>(v => v == productOption.Id)),
                Times.Once());
        }

        [TestMethod]
        public void ProductsController_DeleteProductOption_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            UseEmptyDatabase();

            CurrentProductsController.ModelState.AddModelError("productId", "fakeError");
            var actionResult = CurrentProductsController.DeleteOption(Guid.Empty);
        }


        [TestMethod]
        public void ProductsController_Dispose()
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
        public void ProductsController_DefaultConstructor()
        {
            // ensure 100% code coverage
            var controller = new ProductsController();
        }
    }
}
