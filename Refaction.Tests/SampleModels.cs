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
        // To prevent modification of the sample models, always return a copy of the underlying models

        internal static Product Product0 { get { return ProductRepository.ModelSelectorFunc(SampleData.ProductEntity0); } }
        internal static Product Product1 { get { return ProductRepository.ModelSelectorFunc(SampleData.ProductEntity1); } }
        internal static Product Product2 { get { return ProductRepository.ModelSelectorFunc(SampleData.ProductEntity2); } }

        internal static ProductOption ProductOption0 { get { return ProductOptionRepository.ModelSelectorFunc(SampleData.ProductOptionEntity0); } }
        internal static ProductOption ProductOption1 { get { return ProductOptionRepository.ModelSelectorFunc(SampleData.ProductOptionEntity1); } }
        internal static ProductOption ProductOption2 { get { return ProductOptionRepository.ModelSelectorFunc(SampleData.ProductOptionEntity2); } }
        internal static ProductOption ProductOption3 { get { return ProductOptionRepository.ModelSelectorFunc(SampleData.ProductOptionEntity3); } }
    }
}
