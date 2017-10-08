using Moq;
using Refaction.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refaction.UnitTests.Mocks
{
    /// <summary>
    /// Strict mock for IProductRepository
    /// </summary>
    public class MockProductRepository : Mock<IProductRepository>
    {
        public MockProductRepository()
            : base(MockBehavior.Strict)
        {
        }
    }
}
