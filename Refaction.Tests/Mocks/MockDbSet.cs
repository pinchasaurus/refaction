using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;

namespace Refaction.UnitTests.Mocks
{
    /// <summary>
    /// Strict mock for IDbSet
    /// </summary>
    public class MockDbSet<T> : Mock<IDbSet<T>>
        where T : class
    {
        public readonly IList<T> Data;

        public MockQueryable<T> MockQueryable;

        public MockDbSet(IEnumerable<T> data) 
            : base(MockBehavior.Strict)
        {
            Data = new List<T>(data);
            MockQueryable = new MockQueryable<T>(Data.AsQueryable());

            this.As<IQueryable<T>>().Setup(m => m.Provider).Returns(MockQueryable.Object.Provider);
            this.As<IQueryable<T>>().Setup(m => m.Expression).Returns(MockQueryable.Object.Expression);
            this.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(MockQueryable.Object.ElementType);
            this.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => MockQueryable.Object.GetEnumerator());
        }

        public void VerifyThisAndNestedMocks()
        {
            this.Verify();
            MockQueryable.Verify();
        }
    }
}
