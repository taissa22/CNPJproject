using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Infra.External
{
    internal class ReplaceableQueryProvider : IAsyncQueryProvider
    {
        private readonly IQueryProvider underlyingQueryProvider;

        private readonly ReplacerExpressionVisitor expressionVisitor;

        internal ReplaceableQueryProvider(IQueryProvider underlyingQueryProvider, ReplaceType replaceType)
        {
            this.underlyingQueryProvider = underlyingQueryProvider;
            this.expressionVisitor = new ReplacerExpressionVisitor(replaceType);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new ReplaceableQuery<TElement>(this, expression);
        }

        public IQueryable CreateQuery(Expression expression)
        {
            Type elementType = expression.Type.GetElementType();
            try
            {
                return (IQueryable)Activator.CreateInstance(typeof(ReplaceableQuery<>).MakeGenericType(elementType), new object[] { this, expression });
            }
            catch (System.Reflection.TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        internal IEnumerable<T> ExecuteQuery<T>(Expression expression)
        {
            return underlyingQueryProvider.CreateQuery<T>(Visit(expression)).AsEnumerable();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return underlyingQueryProvider.Execute<TResult>(Visit(expression));
        }

        public object Execute(Expression expression)
        {
            return underlyingQueryProvider.Execute(Visit(expression));
        }

        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        {
            return ((IAsyncQueryProvider)underlyingQueryProvider).ExecuteAsync<TResult>(Visit(expression));
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return ((IAsyncQueryProvider)underlyingQueryProvider).ExecuteAsync<TResult>(Visit(expression), cancellationToken);
        }

        private Expression Visit(Expression expression)
        {
            return expressionVisitor.Visit(expression);
        }
    }
}