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
    /// Strict mock for IQueryProvider
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MockQueryProvider<T> : Mock<IQueryProvider>
    {
        IQueryProvider _queryProvider;

        ICollection<Action<MockQueryProvider<T>>> oneQueryVerifierFuncs = new List<Action<MockQueryProvider<T>>>();
        public MockQueryProvider(IQueryProvider queryProvider)
            : base(MockBehavior.Strict)
        {
            _queryProvider = queryProvider;
        }

        public void ExpectQuery<TElementType>()
        {
            this.Setup(m => m.CreateQuery<TElementType>(It.IsAny<Expression>())).Returns((Expression e) => _queryProvider.CreateQuery<TElementType>(e));

            oneQueryVerifierFuncs.Add(v => v.Verify(m =>
                m.CreateQuery<TElementType>(It.IsAny<Expression>()),
                Times.Once())
            );
        }

        public void ExpectExecute<TElementType>()
        {
            this.Setup(m => m.Execute<TElementType>(It.IsAny<Expression>())).Returns((Expression e) => _queryProvider.Execute<TElementType>(e));

            oneQueryVerifierFuncs.Add(v => v.Verify(m =>
                m.Execute<TElementType>(It.IsAny<Expression>()),
                Times.Once())
            );
        }

        public void VerifyThisAndNestedMocks()
        {
            this.Verify();
        }

        public void VerifyQueriedOnce()
        {
            foreach (var verifierFunc in oneQueryVerifierFuncs)
            {
                verifierFunc.Invoke(this);
            }
        }
    }
}
