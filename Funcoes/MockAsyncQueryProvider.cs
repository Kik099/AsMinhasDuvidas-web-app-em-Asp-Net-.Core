using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;


namespace AsMinhasDuvidas.Funcoes
{
    internal class MockAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _provider;

        internal MockAsyncQueryProvider(IQueryProvider provider)
        {
            _provider = provider;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new MockAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new MockAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _provider.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _provider.Execute<TResult>(expression);
        }

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
