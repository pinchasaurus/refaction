using Refaction.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refaction.Service
{
    public static class ModelComparer
    {
        public static bool AreEquivalent(Product expected, Product actual)
        {
            return
                expected.DeliveryPrice == actual.DeliveryPrice
                && expected.Description == actual.Description
                && expected.Id == actual.Id
                && expected.Name == actual.Name
                && expected.Price == actual.Price
                ;
        }

        public static bool AreEquivalent(ProductOption expected, ProductOption actual)
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
