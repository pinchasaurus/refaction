using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Refaction.UnitTests.Mocks
{
    /// <summary>
    /// Strict mock for IQueryable<T>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MockQueryable<T> : Mock<IQueryable<T>>
    {
        public MockQueryProvider<T> MockQueryProvider;

        public MockQueryable(IQueryable<T> queryable)
            : base(MockBehavior.Strict)
        {
            MockQueryProvider = new MockQueryProvider<T>(queryable.Provider);

            this.Setup(m => m.ElementType).Returns(queryable.ElementType);
            this.Setup(m => m.Expression).Returns(queryable.Expression);
            this.Setup(m => m.Provider).Returns(MockQueryProvider.Object);
        }

        public void VerifyThisAndNestedMocks()
        {
            this.Verify();
            MockQueryProvider.Verify();
        }
    }
}
