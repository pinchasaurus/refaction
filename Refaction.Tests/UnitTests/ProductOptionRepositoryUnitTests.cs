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
    /// Unit tests for 100% code-coverage of ProductOptionRepository
    /// </summary>
    [TestClass]
    public class ProductOptionRepositoryUnitTests
    {
        IRefactionDbContext _db;
        ProductOptionRepository _productOptionRepository;

        [TestInitialize]
        public void InitializeDatabaseWithNoData()
        {
            _db = new FakeRefactionDbContext();
            _productOptionRepository = new ProductOptionRepository(_db);
        }

        public void InitializeDatabaseWithSampleData()
        {
            _db = new FakeRefactionDbContext(SampleData.ProductEntities, SampleData.ProductOptionEntities);
            _productOptionRepository = new ProductOptionRepository(_db);
        }

        [TestMethod]
        public void ProductOptionRepository_CreateProductOptionInEmptyDatabase()
        {
            var model =
                new ProductOption
                {
                    Id = Guid.NewGuid(),
                    Description = "The third product",
                    Name = "Product 3",
                    ProductId = Guid.NewGuid(),
                };

            // act
            _productOptionRepository.Create(model);

            var productOptionEntities = _db.ProductOptionEntities.Local;

            // assert
            Assert.IsTrue(productOptionEntities.Count == 1);

            var productOptionEntity = productOptionEntities[0];

            var entity = ProductOptionRepository.ModelSelectorFunc(productOptionEntity);

            Assert.AreEqual(model.Description, entity.Description);
            Assert.AreEqual(model.Id, entity.Id);
            Assert.AreEqual(model.Name, entity.Name);
            Assert.AreEqual(model.ProductId, entity.ProductId);
        }

        [TestMethod]
        public void ProductOptionRepository_RetrieveAllProductOptionsUsingSampleData()
        {
            InitializeDatabaseWithSampleData();

            var productOptions =
                _productOptionRepository.Retrieve()
                .ToArray();

            Assert.IsTrue(productOptions.Count() == 2);

            Assert.IsTrue(ModelComparer.AreEquivalent(productOptions[0], SampleModels.ProductOption0));
            Assert.IsTrue(ModelComparer.AreEquivalent(productOptions[1], SampleModels.ProductOption1));
        }

        [TestMethod]
        public void ProductOptionRepository_RetrieveProductOptionByIdUsingSampleData()
        {
            InitializeDatabaseWithSampleData();

            var model = _productOptionRepository.Retrieve(SampleModels.ProductOption1.Id);

            Assert.IsTrue(ModelComparer.AreEquivalent(model, SampleModels.ProductOption1));
        }

        [TestMethod]
        public void ProductOptionRepository_RetrieveProductOptionByIdFromEmptyDatabase_ShouldReturnNull()
        {
            var productOption = _productOptionRepository.Retrieve(SampleModels.ProductOption1.Id);

            Assert.IsNull(productOption);
        }

        [TestMethod]
        public void ProductOptionRepository_RetrieveProductOptionByProductIdUsingSampleData()
        {
            InitializeDatabaseWithSampleData();

            var productId = SampleModels.Product1.Id;

            var productOptions = _productOptionRepository.RetrieveByProductId(productId);

            Assert.IsTrue(productOptions.Count() == 1);

            var productOption = productOptions.Single();

            Assert.IsTrue(ModelComparer.AreEquivalent(productOption, SampleModels.ProductOption1));
        }

        [TestMethod]
        public void ProductOptionRepository_RetrieveProductOptionByProductIdFromEmptyDatabase_ShouldReturnNull()
        {
            var productId = SampleModels.Product1.Id;

            var productOptions = _productOptionRepository.RetrieveByProductId(productId);

            Assert.IsTrue(productOptions.Count() == 0);
        }

        [TestMethod]
        public void ProductOptionRepository_RetrieveProductOptionByBothIdsUsingSampleData()
        {
            InitializeDatabaseWithSampleData();

            var productId = SampleModels.Product1.Id;

            var productOption = _productOptionRepository.RetrieveByBothIds(productId, SampleModels.ProductOption1.Id);

            Assert.IsTrue(ModelComparer.AreEquivalent(productOption, SampleModels.ProductOption1));
        }

        [TestMethod]
        public void ProductOptionRepository_RetrieveProductOptionByBothIdsFromEmptyDatabase_ShouldReturnNull()
        {
            var productId = SampleModels.Product1.Id;

            var productOption = _productOptionRepository.RetrieveByBothIds(productId, SampleModels.ProductOption1.Id);

            Assert.IsNull(productOption);
        }


        [TestMethod]
        public void ProductOptionRepository_UpdateProductOption()
        {
            InitializeDatabaseWithSampleData();

            var productOption = new ProductOption(SampleModels.ProductOption1);

            productOption.Description = "The updated product";

            _productOptionRepository.Update(productOption);

            var updatedProductOption = _productOptionRepository.Retrieve(productOption.Id);

            Assert.IsTrue(ModelComparer.AreEquivalent(productOption, updatedProductOption));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductOptionRepository_UpdateProductOptionInEmptyDatabase_ShouldThrowNotFoundException()
        {
            var productOption = new ProductOption(SampleModels.ProductOption1);

            productOption.Name = "The updated product";

            _productOptionRepository.Update(productOption);
        }

        [TestMethod]
        public void ProductOptionRepository_DeleteProductOptionUsingSampleData()
        {
            InitializeDatabaseWithSampleData();

            _productOptionRepository.Delete(SampleModels.ProductOption1);

            // verify products
            var productOptionEntities = _db.ProductOptionEntities.Local;
            Assert.IsTrue(productOptionEntities.Count == 1);

            productOptionEntities = _db.ProductOptionEntities.Local;
            var remainingProductOptionEntity = productOptionEntities[0];
            Assert.IsTrue(EntityComparer.AreEquivalent(remainingProductOptionEntity, SampleData.ProductOptionEntity0));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException_NotFoundException))]
        public void ProductOptionRepository_DeleteProductFromEmptyDatabase_ShouldThrowNotFoundException()
        {
            _productOptionRepository.Delete(SampleModels.ProductOption1);
        }

        [TestMethod]
        public void ProductOptionRepository_ExistsProductInEmptyDatabase_ShouldReturnFalse()
        {
            var result = _productOptionRepository.Exists(SampleModels.ProductOption1.Id);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ProductOptionRepository_ExistsProductUsingSampleData_ShouldReturnTrue()
        {
            InitializeDatabaseWithSampleData();

            var result = _productOptionRepository.Exists(SampleModels.ProductOption1.Id);

            Assert.IsTrue(result);
        }
    }
}
