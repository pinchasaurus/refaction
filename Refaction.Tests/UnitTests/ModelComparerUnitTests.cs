using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refaction.Service;
using Refaction.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refaction.UnitTests.UnitTests
{
    /// <summary>
    /// Unit tests for 100% code coverage of ModelComparer
    /// </summary>
    [TestClass]
    public class ModelComparerUnitTests : TestBase
    {
        [TestMethod]
        public void ModelComparer_AreEquivalentProducts()
        {
            // ensure 100% code coverage by testing every property

            var product =
                new Product
                {
                    DeliveryPrice = 9.99M,
                    Description = "The product to test",
                    Id = Guid.NewGuid(),
                    Name = "Test product",
                    Price = 8.88M
                };

            var other = new Product(product);

            Assert.IsTrue(ModelComparer.AreEquivalent(product, other));

            other.Id = Guid.NewGuid();
            Assert.IsFalse(ModelComparer.AreEquivalent(product, other));
            other.Id = product.Id;

            other.DeliveryPrice = product.DeliveryPrice + 1;
            Assert.IsFalse(ModelComparer.AreEquivalent(product, other));
            other.DeliveryPrice = product.DeliveryPrice;

            other.Description = other.Description + " other";
            Assert.IsFalse(ModelComparer.AreEquivalent(product, other));
            other.Description = product.Description;

            other.Name = other.Name + " other";
            Assert.IsFalse(ModelComparer.AreEquivalent(product, other));
            other.Name = product.Name;

            other.Price = other.Price + 1;
            Assert.IsFalse(ModelComparer.AreEquivalent(product, other));
            other.Id = product.Id;
        }

            [TestMethod]
        public void ModelComparer_AreEquivalentProductOptions()
        {
            var option =
                new ProductOption
                {
                    Description = "The product to test",
                    Id = Guid.NewGuid(),
                    Name = "Test product",
                    ProductId = Guid.NewGuid(),
                };

            var other = new ProductOption(option);

            Assert.IsTrue(ModelComparer.AreEquivalent(option, other));

            other.Id = Guid.NewGuid();
            Assert.IsFalse(ModelComparer.AreEquivalent(option, other));
            other.Id = option.Id;

            other.Description = other.Description + " other";
            Assert.IsFalse(ModelComparer.AreEquivalent(option, other));
            other.Description = option.Description;

            other.Name = other.Name + " other";
            Assert.IsFalse(ModelComparer.AreEquivalent(option, other));
            other.Name = option.Name;

            other.ProductId = Guid.NewGuid();
            Assert.IsFalse(ModelComparer.AreEquivalent(option, other));
            other.Id = option.Id;
        }
}
}
