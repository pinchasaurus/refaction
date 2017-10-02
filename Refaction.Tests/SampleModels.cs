using Refaction.Service.Models;
using Refaction.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refaction.Tests
{
    /// <summary>
    /// Models for testing with sample data
    /// </summary>
    public static class SampleModels
    {
        public static readonly Product Product0 = ProductRepository.ModelSelectorFunc(SampleData.ProductEntity0);
        public static readonly Product Product1 = ProductRepository.ModelSelectorFunc(SampleData.ProductEntity1);
        public static readonly Product Product2 = ProductRepository.ModelSelectorFunc(SampleData.ProductEntity2);

        public static readonly ProductOption ProductOption0 = ProductOptionRepository.ModelSelectorFunc(SampleData.ProductOptionEntity0);
        public static readonly ProductOption ProductOption1 = ProductOptionRepository.ModelSelectorFunc(SampleData.ProductOptionEntity1);
        public static readonly ProductOption ProductOption2 = ProductOptionRepository.ModelSelectorFunc(SampleData.ProductOptionEntity2);
        public static readonly ProductOption ProductOption3 = ProductOptionRepository.ModelSelectorFunc(SampleData.ProductOptionEntity3);
    }
}
