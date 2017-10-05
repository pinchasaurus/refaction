using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refaction.Data;
using Refaction.Data.Entities;
using Refaction.Data.Fakes;
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
    /// Unit tests for 100% code coverage of EntityComparer
    /// </summary>
    [TestClass]
    public class EntityComparerUnitTests : TestBase
    {
        [TestMethod]
        public void EntityComparer_AreEquivalentProducts()
        {
            // ensure 100% code coverage by testing every property

            var product =
                new ProductEntity
                {
                    DeliveryPrice = 9.99M,
                    Description = "The product to test",
                    Id = Guid.NewGuid(),
                    Name = "Test product",
                    Price = 8.88M
                };

            var other = FakeRefactionDbContext.CreateCopy(product);

            Assert.IsTrue(EntityComparer.AreEquivalent(product, other));

            other.Id = Guid.NewGuid();
            Assert.IsFalse(EntityComparer.AreEquivalent(product, other));
            other.Id = product.Id;

            other.DeliveryPrice = product.DeliveryPrice + 1;
            Assert.IsFalse(EntityComparer.AreEquivalent(product, other));
            other.DeliveryPrice = product.DeliveryPrice;

            other.Description = other.Description + " other";
            Assert.IsFalse(EntityComparer.AreEquivalent(product, other));
            other.Description = product.Description;

            other.Name = other.Name + " other";
            Assert.IsFalse(EntityComparer.AreEquivalent(product, other));
            other.Name = product.Name;

            other.Price = other.Price + 1;
            Assert.IsFalse(EntityComparer.AreEquivalent(product, other));
            other.Id = product.Id;
        }

        [TestMethod]
        public void EntityComparer_AreEquivalentProductOptions()
        {
            var option =
                new ProductOptionEntity
                {
                    Description = "The product to test",
                    Id = Guid.NewGuid(),
                    Name = "Test product",
                    ProductId = Guid.NewGuid(),
                };

            var other = FakeRefactionDbContext.CreateCopy(option);

            Assert.IsTrue(EntityComparer.AreEquivalent(option, other));

            other.Id = Guid.NewGuid();
            Assert.IsFalse(EntityComparer.AreEquivalent(option, other));
            other.Id = option.Id;

            other.Description = other.Description + " other";
            Assert.IsFalse(EntityComparer.AreEquivalent(option, other));
            other.Description = option.Description;

            other.Name = other.Name + " other";
            Assert.IsFalse(EntityComparer.AreEquivalent(option, other));
            other.Name = option.Name;

            other.ProductId = Guid.NewGuid();
            Assert.IsFalse(EntityComparer.AreEquivalent(option, other));
            other.Id = option.Id;
        }
    }
}
