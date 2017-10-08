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
    /// Strict mock for IProductOptionRepository
    /// </summary>
    public class MockProductOptionRepository : Mock<IProductOptionRepository>
    {
        public MockProductOptionRepository()
            : base(MockBehavior.Strict)
        {
        }
    }
}
