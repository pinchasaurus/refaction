using Refaction.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refaction.Data
{
    public static class EntityComparer
    {
        public static bool AreEquivalent(ProductEntity expected, ProductEntity actual)
        {
            return
                expected.DeliveryPrice == actual.DeliveryPrice
                && expected.Description == actual.Description
                && expected.Id == actual.Id
                && expected.Name == actual.Name
                && expected.Price == actual.Price
                ;
        }

        public static bool AreEquivalent(ProductOptionEntity expected, ProductOptionEntity actual)
        {
            return
                expected.Description == actual.Description
                && expected.Id == actual.Id
                && expected.Name == actual.Name
                && expected.ProductId == actual.ProductId
                ;
        }
    }
}
