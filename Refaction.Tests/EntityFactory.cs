using Refaction.Data.Entities;
using Refaction.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refaction.UnitTests
{
    /// <summary>
    /// Copies other entities
    /// </summary>
    /// <remarks>
    /// Generally, entities should not be copied, but we need this capability for testing purposes.
    /// </remarks>
    public class EntityFactory
    {
        public static ProductEntity CreateProductEntity(Product product)
        {
            return
                new ProductEntity
                {
                    Id = product.Id,
                    DeliveryPrice = product.DeliveryPrice,
                    Description = product.Description,
                    Name = product.Name,
                    Price = product.Price
                };
        }

        public static ProductOptionEntity CreateProductOptionEntity(ProductOption productOption)
        {
            return
                new ProductOptionEntity
                {
                    Id = productOption.Id,
                    Description = productOption.Description,
                    Name = productOption.Name,
                    ProductId = productOption.ProductId
                };
        }
    }
}
