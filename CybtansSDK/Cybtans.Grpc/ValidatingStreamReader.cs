using Grpc.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.Grpc
{
    internal class ValidatingStreamReader<TRequest> : IAsyncStreamReader<TRequest>
    {
        private readonly IAsyncStreamReader<TRequest> _innerReader;
        private readonly Action<TRequest> _validator;

        public ValidatingStreamReader(IAsyncStreamReader<TRequest> innerReader, Action<TRequest> validator)
        {
            _innerReader = innerReader;
            _validator = validator;
        }

        public async Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            var success = await _innerReader.MoveNext(cancellationToken).ConfigureAwait(false);
            if (success)
            {
                _validator(Current);
            }

            return success;
        }

        public TRequest Current => _innerReader.Current;
    }
}
