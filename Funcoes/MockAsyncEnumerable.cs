using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace AsMinhasDuvidas.Funcoes
{
    internal class MockAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public MockAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IAsyncEnumerator<T> GetEnumerator()
        {
            return new MockAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

       

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        IQueryProvider IQueryable.Provider => new MockAsyncQueryProvider<T>(this);
    }
}
