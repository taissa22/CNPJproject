using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Perlink.Oi.Juridico.Data
{
    /**
     * Fonte:
     * https://stackoverflow.com/questions/37527783/get-sql-code-from-an-entity-framework-core-iqueryablet
     */

    public static class IQueryableExtensions
    {
        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");

        private static readonly FieldInfo QueryModelGeneratorField = QueryCompilerTypeInfo.DeclaredFields.First(x => x.Name == "_queryModelGenerator");

        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");

        private static readonly PropertyInfo DatabaseDependenciesField = typeof(Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");

        private static readonly MethodInfo OrderByMethod = typeof(Queryable)
                                .GetMethods()
                                .Single(
                                    method => method.Name == "OrderBy" && method.GetParameters().Length == 2);

        private static readonly MethodInfo OrderByDescendingMethod = typeof(Queryable)
                                .GetMethods()
                                .Single(
                                    method => method.Name == "OrderByDescending" && method.GetParameters().Length == 2);


        /// <summary>
        /// retorna a saída da query de um IQueryable
        /// </summary>
        /// <returns>string com sql de um IQueryable</returns>
        public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
            var queryCompiler = (QueryCompiler)QueryCompilerField.GetValue(query.Provider);
            var modelGenerator = (QueryModelGenerator)QueryModelGeneratorField.GetValue(queryCompiler);
            var queryModel = modelGenerator.ParseQuery(query.Expression);
            var database = (IDatabase)DataBaseField.GetValue(queryCompiler);
            var databaseDependencies = (DatabaseDependencies)DatabaseDependenciesField.GetValue(database);
            var queryCompilationContext = databaseDependencies.QueryCompilationContextFactory.Create(false);
            var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
            var sql = modelVisitor.Queries.First().ToString();

            return sql;
        }

        public static bool PropertyExists<TEntity>(this IQueryable<TEntity> source, string propertyName)
        {
            return propertyName != null && typeof(TEntity).GetProperty(propertyName, BindingFlags.IgnoreCase |
                BindingFlags.Public | BindingFlags.Instance) != null;
        }
        /// <summary>
        /// Método criado para realizar paginação em IQueryable
        /// </summary>
        /// <returns>IQueryable paginado</returns>
        public static IQueryable<TEntity> Paginar<TEntity>(this IQueryable<TEntity> source, int pagina, int quantidade)
        {
            if (pagina > 0 && quantidade > 0)
                return source.Skip((pagina - 1) * quantidade)
                         .Take(quantidade);
            else
                return source;
        }

        /// <summary>
        /// Ordena um IQueryable pelo nome de sua propriedade "propertyName".
        /// O parâmetro opcional "defaultPropertyName" deve ser informado quando houver uma propriedade default para ordenação
        /// </summary>
        /// <returns>IQueryable ordenado</returns>
        public static IQueryable<TEntity> OrdenarPorPropriedade<TEntity>(
           this IQueryable<TEntity> source, bool ascending, string propertyName, string defaultPropertyName = "")
        {
            string ordenarPor;
            if (!PropertyExists(source, propertyName) && !PropertyExists(source, defaultPropertyName))
                return source;
            else
                ordenarPor = PropertyExists(source, propertyName) ? propertyName : defaultPropertyName;
            //torna a ordenação crescente sempre que a 'propertyName' não existir ou não for informada
            if(!PropertyExists(source, propertyName))
                ascending = true;
            
            ParameterExpression paramterExpression = Expression.Parameter(typeof(TEntity));
            Expression orderByProperty = Expression.Property(paramterExpression, ordenarPor);
            LambdaExpression lambda = Expression.Lambda(orderByProperty, paramterExpression);
            MethodInfo genericMethod;
            if (ascending)
                genericMethod =
                  OrderByMethod.MakeGenericMethod(typeof(TEntity), orderByProperty.Type);
            else
                genericMethod =
              OrderByDescendingMethod.MakeGenericMethod(typeof(TEntity), orderByProperty.Type);
            object ret = genericMethod.Invoke(null, new object[] { source, lambda });
            return (IQueryable<TEntity>)ret;
        }

        public static IQueryable<TSource> WhereIf<TSource>(
            this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            if (condition)
                return source.Where(predicate);

            return source;
        }
    }
}

