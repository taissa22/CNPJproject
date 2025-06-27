using System;
using System.Linq;

namespace Perlink.Oi.Juridico.Infra.External
{   
    //TODO: Troquei o nome porque estava dando conflito com o outro arquivo QueryableExtensions
    public static class QueryableExtensionsEntity
    {
        // TODO: sumary
        // TODO: What happens when 2 Types are used???
        public static IQueryable<TEntity> ReplacePropertyType<TEntity, TypeFrom, TypeTo>(this IQueryable<TEntity> source, Func<ReplaceTypeBuilder<TEntity, TypeFrom, TypeTo>, ReplaceType> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            ReplaceType replaceType = builder.Invoke(new ReplaceTypeBuilder<TEntity, TypeFrom, TypeTo>());
            return new ReplaceableQueryProvider(source.Provider, replaceType).CreateQuery<TEntity>(source.Expression);
        }
    }
}