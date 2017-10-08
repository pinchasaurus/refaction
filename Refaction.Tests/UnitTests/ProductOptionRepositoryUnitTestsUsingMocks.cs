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
    /// Unit tests for 100% code coverage of ProductOptionRepository 
    /// (using strict mocks to guarantee precise behavior)
    /// </summary>
    [TestClass]
    public class ProductOptionRepositoryUnitTestsUsingMocks : RefactionTestUsingMocksBase
    {
        IProductOptionRepository _productOptionRepository;

        public ProductOptionRepositoryUnitTestsUsingMocks()
        {
        }

        protected override void UseEmptyDatabase()
        {
            base.UseEmptyDatabase();

            _productOptionRepository = new ProductOptionRepository(this.MockRefactionDbContext.Object);
        }

        protected override void UseSampleDatabase()
        {
            base.UseSampleDatabase();

            _productOptionRepository = new ProductOptionRepository(this.MockRefactionDbContext.Object);
        }

        IProductOptionRepository CurrentProductOptionRepository
        {
            get { return _productOptionRepository; }
        }

        [TestMethod]
        public void ProductOptionRepository_CreateProductOption_UsingEmptyDatabase()
        {
            UseEmptyDatabase();

            CreateProductOption();

            this.MockRefactionDbContext.VerifyThisAndNestedMocks();
        }

        [TestMethod]
        public void ProductOptionRepository_CreateProductOption_UsingSampleDatabase()
        {
            UseSampleDatabase();

            CreateProductOption();

            // verify that no other entities were affected
            var data = MockRefactionDbContext.MockProductOptions.Data;
            Assert.IsTrue(data.Count() == 5);
            Assert.IsTrue(EntityComparer.AreEquivalent(data[0], SampleData.ProductOptionEntity0));
            Assert.IsTrue(EntityComparer.AreEquivalent(data[1], SampleData.ProductOptionEntity1));
            Assert.IsTrue(EntityComparer.AreEquivalent(data[2], SampleData.ProductOptionEntity2));
            Assert.IsTrue(EntityComparer.AreEquivalent(data[3], SampleData.ProductOptionEntity3));

            this.MockRefactionDbContext.VerifyThisAndNestedMocks();
        }

        protected void CreateProductOption()
        {
            var productOption =
                new ProductOption
                {
                    Id = Guid.NewGuid(),
                    Description = "The third product option",
                    Name = "ProductOption 3",
                    ProductId = SampleModels.Product1.Id
                };

            var expectedProductOptionEntity = EntityFactory.CreateProductOptionEntity(productOption);

            // expect add method call
            this.MockRefactionDbContext.MockProductOptions
                .Setup<ProductOptionEntity>(m =>
                    m.Add(It.Is<ProductOptionEntity>(e => EntityComparer.AreEquivalent(expectedProductOptionEntity, e))))
                    .Callback<ProductOptionEntity>(e => this.MockRefactionDbContext.MockProductOptions.Data.Add(e))
                    .Returns<ProductOptionEntity>(e => e)
                    .Verifiable();

            CurrentProductOptionRepository.Create(productOption);

            // verify that add method called once
            this.MockRefactionDbContext.MockProductOptions
                .Verify<ProductOptionEntity>(m =>
                    m.Add(It.Is<ProductOptionEntity>(actual => EntityComparer.AreEquivalent(expectedProductOptionEntity, actual))),
                    Times.Once());

            // verify that created entity matches submitted entity
            var createdProductOptionEntity = MockRefactionDbContext.MockProductOptions.Data.Single(x => x.Id == productOption.Id);
            var createdProductOption = ProductOptionRepository.ModelSelectorFunc(createdProductOptionEntity);
            Assert.IsTrue(ModelComparer.AreEquivalent(productOption, createdProductOption));
        }

        [TestMethod]
        public void ProductOptionRepository_RetrieveAllProductOptions_UsingEmptyDatabase()
        {
            UseEmptyDatabase();

            MockProductOptionQueryProvider.ExpectQuery<ProductOption>();

            var productOptions =
                CurrentProductOptionRepository.Retrieve()
                .ToArray();

            Assert.IsTrue(productOptions.Count() == 0);

            MockProductOptionQueryProvider.VerifyQueriedOnce();
            MockRefactionDbContext.VerifyThisAndNestedMocks();
        }

        [TestMethod]
        public void ProductOptionRepository_RetrieveAllProductOptions_UsingSampleDatabase()
        {
            UseSampleDatabase();

            MockProductOptionQueryProvider.ExpectQuery<ProductOption>();

            var productOptions =
                CurrentProductOptionRepository.Retrieve()
                .ToArray();

            Assert.IsTrue(productOptions.Count() == 4);
            Assert.IsTrue(ModelComparer.AreEquivalent(productOptions[0], SampleModels.ProductOption0));
            Assert.IsTrue(ModelComparer.AreEquivalent(productOptions[1], SampleModels.ProductOption1));
            Assert.IsTrue(ModelComparer.AreEquivalent(productOptions[2], SampleModels.ProductOption2));
            Assert.IsTrue(ModelComparer.AreEquivalent(productOptions[3], SampleModels.ProductOption3));

            MockProductOptionQueryProvider.VerifyQueriedOnce();
            MockRefactionDbContext.VerifyThisAndNestedMocks();
        }

        [TestMethod]
        public void ProductOptionRepository_RetrieveProductOptionById_UsingSampleDatabase()
        {
            UseSampleDatabase();

            MockProductOptionQueryProvider.ExpectExecute<ProductOptionEntity>();

            var productOption = CurrentProductOptionRepository.Retrieve(SampleModels.ProductOption1.Id);

            Assert.IsTrue(ModelComparer.AreEquivalent(productOption, SampleModels.ProductOption1));

            MockProductOptionQueryProvider.VerifyQueriedOnce();
            MockRefactionDbContext.VerifyThisAndNestedMocks();
        }

        [TestMethod]
        public void ProductOptionRepository_RetrieveProductOptionByIdFromEmptyDatabase_ShouldReturnNull()
        {
            UseEmptyDatabase();

            MockProductOptionQueryProvider.ExpectExecute<ProductOptionEntity>();

            var productOption = CurrentProductOptionRepository.Retrieve(SampleModels.ProductOption1.Id);

            Assert.IsNull(productOption);

            MockProductOptionQueryProvider.VerifyQueriedOnce();
            MockRefactionDbContext.VerifyThisAndNestedMocks();
        }

        [TestMethod]
        public void ProductOptionRepository_RetrieveProductOptionByProductId_UsingSampleDatabase()
        {
            UseSampleDatabase();

            MockProductOptionQueryProvider.ExpectQuery<ProductOptionEntity>();

            var productOptions = CurrentProductOptionRepository.RetrieveByProductId(SampleModels.Product1.Id);
            var productOptionsArray = productOptions.ToArray();
            Assert.IsTrue(productOptionsArray.Count() == 2);

            Assert.IsTrue(ModelComparer.AreEquivalent(productOptionsArray[0], SampleModels.ProductOption1));
            Assert.IsTrue(ModelComparer.AreEquivalent(productOptionsArray[1], SampleModels.ProductOption3));

            MockProductOptionQueryProvider.VerifyQueriedOnce();
            MockRefactionDbContext.VerifyThisAndNestedMocks();
        }

        [TestMethod]
        public void ProductOptionRepository_RetrieveProductOptionByBothIds_UsingSampleDatabase()
        {
            UseSampleDatabase();

            MockProductOptionQueryProvider.ExpectQuery<ProductOptionEntity>();

            var productOption = CurrentProductOptionRepository.RetrieveByBothIds(SampleModels.Product1.Id, SampleModels.ProductOption1.Id);

            Assert.IsTrue(ModelComparer.AreEquivalent(productOption, SampleModels.ProductOption1));

            MockProductOptionQueryProvider.VerifyQueriedOnce();
            MockRefactionDbContext.VerifyThisAndNestedMocks();
        }


        [TestMethod]
        public void ProductOptionRepository_UpdateProductOption()
        {
            UseSampleDatabase();

            var submittedProductOption = SampleModels.ProductOption1;
            submittedProductOption.Name = "The updated product";

            // expect find method call
            MockRefactionDbContext.MockProductOptions
                .Setup<ProductOptionEntity>(m =>
                    m.Find(It.Is<Guid>(actual => submittedProductOption.Id == actual)))
                    .Returns(MockRefactionDbContext.MockProductOptions.Data.Single(x => x.Id == submittedProductOption.Id))
                .Verifiable();

            CurrentProductOptionRepository.Update(submittedProductOption);

            // verify that find method was called once
            MockRefactionDbContext.MockProductOptions
                .Verify<ProductOptionEntity>(m =>
                    m.Find(It.Is<Guid>(actual => submittedProductOption.Id == actual)),
                    Times.Once());

            // verify that submitted productOption matches updated productOption
            var updatedProductOptionEntity = MockRefactionDbContext.MockProductOptions.Data.Single(x => x.Id == submittedProductOption.Id);
            var updatedProductOption = ProductOptionRepository.ModelSelectorFunc(updatedProductOptionEntity);
            Assert.IsTrue(ModelComparer.AreEquivalent(submittedProductOption, updatedProductOption));

            MockProductOptionQueryProvider.VerifyQueriedOnce();
            MockRefactionDbContext.VerifyThisAndNestedMocks();
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductOptionRepository_UpdateProductOptionInEmptyDatabase_ShouldThrowNotFoundException()
        {
            UseEmptyDatabase();

            var submittedProductOption = SampleModels.ProductOption1;
            submittedProductOption.Name = "The updated product";

            // expect find method call
            MockRefactionDbContext.MockProductOptions
                .Setup<ProductOptionEntity>(m =>
                    m.Find(It.Is<Guid>(actual => submittedProductOption.Id == actual)))
                    .Returns((ProductOptionEntity)null)
                    .Verifiable();

            CurrentProductOptionRepository.Update(submittedProductOption);

            // if the previous line did not throw an exception, then throw one here...
            throw new InvalidOperationException("Update method did not throw any exception");
        }

        [TestMethod]
        public void ProductOptionRepository_DeleteProductOption_UsingSampleDatabase()
        {
            UseSampleDatabase();

            MockProductOptionQueryProvider.ExpectExecute<ProductOptionEntity>();

            var expectedProductOptionEntity = SampleData.ProductOptionEntity1;

            // expect remove method call
            MockRefactionDbContext.MockProductOptions
                .Setup<ProductOptionEntity>(m =>
                    m.Remove(It.Is<ProductOptionEntity>(actual => EntityComparer.AreEquivalent(expectedProductOptionEntity, actual))))
                    .Callback<ProductOptionEntity>(x => MockRefactionDbContext.MockProductOptions.Data.Remove(x))
                    .Returns(MockRefactionDbContext.MockProductOptions.Data.Single(x => x.Id == SampleModels.ProductOption1.Id))
                    .Verifiable();

            CurrentProductOptionRepository.Delete(SampleModels.ProductOption1);

            // verify that productOption remove method was called once
            MockRefactionDbContext.MockProductOptions
                .Verify<ProductOptionEntity>(m =>
                    m.Remove(It.Is<ProductOptionEntity>(actual => EntityComparer.AreEquivalent(expectedProductOptionEntity, actual))),
                    Times.Once());

            // verify the no other product options were affected
            var productOptionEntities = MockRefactionDbContext.MockProductOptions.Data.ToArray();
            Assert.IsTrue(productOptionEntities.Count() == 3);
            Assert.IsTrue(EntityComparer.AreEquivalent(productOptionEntities[0], SampleData.ProductOptionEntity0));
            Assert.IsTrue(EntityComparer.AreEquivalent(productOptionEntities[1], SampleData.ProductOptionEntity2));
            Assert.IsTrue(EntityComparer.AreEquivalent(productOptionEntities[2], SampleData.ProductOptionEntity3));

            MockProductOptionQueryProvider.VerifyQueriedOnce();
            MockRefactionDbContext.VerifyThisAndNestedMocks();
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductOptionRepository_DeleteProductOptionFromEmptyDatabase_ShouldThrowNotFoundException()
        {
            UseEmptyDatabase();

            MockProductOptionQueryProvider.ExpectExecute<ProductOptionEntity>();

            CurrentProductOptionRepository.Delete(SampleModels.ProductOption1);

            // if the previous call did not throw an exception, then throw one here
            throw new NotImplementedException("Delete method did not throw any exception");
        }

        [TestMethod]
        public void ProductOptionRepository_ExistsProductOptionInEmptyDatabase_ShouldReturnFalse()
        {
            UseEmptyDatabase();

            MockProductOptionQueryProvider.ExpectExecute<bool>();

            var exists = CurrentProductOptionRepository.Exists(SampleModels.ProductOption1.Id);

            Assert.IsFalse(exists);

            MockProductOptionQueryProvider.VerifyQueriedOnce();
            MockRefactionDbContext.VerifyThisAndNestedMocks();
        }

        [TestMethod]
        public void ProductOptionRepository_ExistsProductOptionUsingSampleDatabase_ShouldReturnTrue()
        {
            UseSampleDatabase();

            MockProductOptionQueryProvider.ExpectExecute<bool>();

            var exists = CurrentProductOptionRepository.Exists(SampleModels.ProductOption1.Id);

            Assert.IsTrue(exists);

            MockProductOptionQueryProvider.VerifyQueriedOnce();
            MockRefactionDbContext.VerifyThisAndNestedMocks();
        }
    }
}
