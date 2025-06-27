using System.Linq;

namespace Perlink.Oi.Juridico.Infra.External
{
    /// <summary>
    /// Represents the result of a sorting operation.
    /// </summary>
    /// <typeparam name="T">The type of the content of the data source.</typeparam>
    public interface ISorteredQueryable<out T> : IOrderedQueryable<T>
    {
    }
}
