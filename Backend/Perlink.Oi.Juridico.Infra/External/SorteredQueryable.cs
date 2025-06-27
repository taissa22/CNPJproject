using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Perlink.Oi.Juridico.Infra.External
{
    internal class SorteredQueryable<TSource> : ISorteredQueryable<TSource>
    {
        private readonly IOrderedQueryable<TSource> queryable;

        public SorteredQueryable(IOrderedQueryable<TSource> queryable)
        {
            this.queryable = queryable;
        }

        public Type ElementType => queryable.ElementType;

        public Expression Expression => queryable.Expression;

        public IQueryProvider Provider => queryable.Provider;

        public IEnumerator<TSource> GetEnumerator() => queryable.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => queryable.GetEnumerator();
    }
}
