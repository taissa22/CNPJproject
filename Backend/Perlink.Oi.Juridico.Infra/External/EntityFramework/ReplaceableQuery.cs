using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Perlink.Oi.Juridico.Infra.External
{
    internal class ReplaceableQuery<T> : IQueryable<T>, IOrderedQueryable<T>, IAsyncEnumerable<T>
    {
        private readonly ReplaceableQueryProvider provider;

        public ReplaceableQuery(ReplaceableQueryProvider provider, Expression expression)
        {
            this.provider = provider;
            this.Expression = expression;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return provider.ExecuteQuery<T>(Expression).GetEnumerator();
        }

        IAsyncEnumerator<T> IAsyncEnumerable<T>.GetEnumerator()
        {
            return provider.ExecuteAsync<T>(Expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType => typeof(T);

        public Expression Expression { get; }

        public IQueryProvider Provider => provider;
    }
}