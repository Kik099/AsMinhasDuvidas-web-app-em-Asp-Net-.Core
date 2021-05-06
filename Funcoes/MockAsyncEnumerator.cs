using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace AsMinhasDuvidas.Funcoes
{
    internal class MockAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;

        public MockAsyncEnumerator(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        public T Current => _enumerator.Current;

        public void Dispose()
        {
            _enumerator.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            return Task.FromResult(_enumerator.MoveNext());
        }

        public ValueTask<bool> MoveNextAsync()
        {
            throw new NotImplementedException();
        }
    }
}
