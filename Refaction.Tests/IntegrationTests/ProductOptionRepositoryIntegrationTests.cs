using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Refaction.Data;
using Refaction.Data.Entities;
using Refaction.Data.Fakes;
using Refaction.Service;
using Refaction.Service.Models;
using Refaction.Service.Repositories;
using Refaction.UnitTests;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace Refaction.Tests.UnitTests
{
    /// <summary>
    /// Integration tests for 100% code-coverage of ProductOptionRepository
    /// </summary>
    [TestClass]
    public class ProductOptionRepositoryIntegrationTests : RefactionTestUsingFakesBase
    {
        ProductOptionRepository CurrentProductOptionsRepository
        {
            get { return NinjectKernel.Get<ProductOptionRepository>(); }
        }

        [TestMethod]
        public void ProductOptionRepository_CreateProductOptionInEmptyDatabase()
        {
            UseEmptyDatabase();

            var model =
                new ProductOption
                {
                    Id = Guid.NewGuid(),
                    Description = "The third product",
                    Name = "Product 3",
                    ProductId = Guid.NewGuid(),
                };

            // act
            CurrentProductOptionsRepository.Create(model);

            var productOptionEntities = CurrentDbContext.ProductOptionEntities.Local;

            // assert
            Assert.IsTrue(productOptionEntities.Count == 1);

            var productOptionEntity = productOptionEntities[0];

            var createdModel = ProductOptionRepository.ModelSelectorFunc(productOptionEntity);

            Assert.IsTrue(ModelComparer.AreEquivalent(model, createdModel));
        }

        [TestMethod]
        public void ProductOptionRepository_RetrieveAllProductOptionsUsingSampleData()
        {
            UseSampleDatabase();

            var productOptions =
                CurrentProductOptionsRepository.Retrieve()
                .ToArray();

            Assert.IsTrue(productOptions.Count() == 4);

            Assert.IsTrue(ModelComparer.AreEquivalent(productOptions[0], SampleModels.ProductOption0));
            Assert.IsTrue(ModelComparer.AreEquivalent(productOptions[1], SampleModels.ProductOption1));
            Assert.IsTrue(ModelComparer.AreEquivalent(productOptions[2], SampleModels.ProductOption2));
            Assert.IsTrue(ModelComparer.AreEquivalent(productOptions[3], SampleModels.ProductOption3));
        }

        [TestMethod]
        public void ProductOptionRepository_RetrieveProductOptionByIdUsingSampleData()
        {
            UseSampleDatabase();

            var model = CurrentProductOptionsRepository.Retrieve(SampleModels.ProductOption1.Id);

            Assert.IsTrue(ModelComparer.AreEquivalent(model, SampleModels.ProductOption1));
        }

        [TestMethod]
        public void ProductOptionRepository_RetrieveProductOptionByIdFromEmptyDatabase_ShouldReturnNull()
        {
            UseEmptyDatabase();

            var productOption = CurrentProductOptionsRepository.Retrieve(SampleModels.ProductOption1.Id);

            Assert.IsNull(productOption);
        }

        [TestMethod]
        public void ProductOptionRepository_RetrieveProductOptionByProductIdUsingSampleData()
        {
            UseSampleDatabase();

            var productOptions = CurrentProductOptionsRepository.RetrieveByProductId(SampleModels.Product0.Id);

            Assert.IsTrue(productOptions.Count() == 2);

            var productOptionsArray = productOptions.ToArray();

            Assert.IsTrue(ModelComparer.AreEquivalent(productOptionsArray[0], SampleModels.ProductOption0));
            Assert.IsTrue(ModelComparer.AreEquivalent(productOptionsArray[1], SampleModels.ProductOption2));
        }

        [TestMethod]
        public void ProductOptionRepository_RetrieveProductOptionByProductIdFromEmptyDatabase_ShouldReturnNull()
        {
            UseEmptyDatabase();

            var productId = SampleModels.Product1.Id;

            var productOptions = CurrentProductOptionsRepository.RetrieveByProductId(productId);

            Assert.IsTrue(productOptions.Count() == 0);
        }

        [TestMethod]
        public void ProductOptionRepository_RetrieveProductOptionByBothIdsUsingSampleData()
        {
            UseSampleDatabase();

            var productId = SampleModels.Product1.Id;

            var productOption = CurrentProductOptionsRepository.RetrieveByBothIds(productId, SampleModels.ProductOption1.Id);

            Assert.IsTrue(ModelComparer.AreEquivalent(productOption, SampleModels.ProductOption1));
        }

        [TestMethod]
        public void ProductOptionRepository_RetrieveProductOptionByBothIdsFromEmptyDatabase_ShouldReturnNull()
        {
            UseEmptyDatabase();

            var productId = SampleModels.Product1.Id;

            var productOption = CurrentProductOptionsRepository.RetrieveByBothIds(productId, SampleModels.ProductOption1.Id);

            Assert.IsNull(productOption);
        }


        [TestMethod]
        public void ProductOptionRepository_UpdateProductOption()
        {
            UseSampleDatabase();

            var productOption = new ProductOption(SampleModels.ProductOption1);

            productOption.Description = "The updated product";

            CurrentProductOptionsRepository.Update(productOption);

            var updatedProductOption = CurrentProductOptionsRepository.Retrieve(productOption.Id);

            Assert.IsTrue(ModelComparer.AreEquivalent(productOption, updatedProductOption));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductOptionRepository_UpdateProductOptionInEmptyDatabase_ShouldThrowNotFoundException()
        {
            UseEmptyDatabase();

            var productOption = new ProductOption(SampleModels.ProductOption1);

            productOption.Name = "The updated product";

            CurrentProductOptionsRepository.Update(productOption);
        }

        [TestMethod]
        public void ProductOptionRepository_DeleteProductOptionUsingSampleData()
        {
            UseSampleDatabase();

            CurrentProductOptionsRepository.Delete(SampleModels.ProductOption1);

            var productOptionEntities = CurrentDbContext.ProductOptionEntities.Local;
            Assert.IsTrue(productOptionEntities.Count == 3);

            productOptionEntities = CurrentDbContext.ProductOptionEntities.Local;
            Assert.IsTrue(EntityComparer.AreEquivalent(productOptionEntities[0], SampleData.ProductOptionEntity0));
            Assert.IsTrue(EntityComparer.AreEquivalent(productOptionEntities[1], SampleData.ProductOptionEntity2));
            Assert.IsTrue(EntityComparer.AreEquivalent(productOptionEntities[2], SampleData.ProductOptionEntity3));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductOptionRepository_DeleteProductFromEmptyDatabase_ShouldThrowNotFoundException()
        {
            UseEmptyDatabase();

            CurrentProductOptionsRepository.Delete(SampleModels.ProductOption1);
        }

        [TestMethod]
        public void ProductOptionRepository_ExistsProductInEmptyDatabase_ShouldReturnFalse()
        {
            UseEmptyDatabase();

            var result = CurrentProductOptionsRepository.Exists(SampleModels.ProductOption1.Id);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ProductOptionRepository_ExistsProductUsingSampleData_ShouldReturnTrue()
        {
            UseSampleDatabase();

            var result = CurrentProductOptionsRepository.Exists(SampleModels.ProductOption1.Id);

            Assert.IsTrue(result);
        }
    }
}
