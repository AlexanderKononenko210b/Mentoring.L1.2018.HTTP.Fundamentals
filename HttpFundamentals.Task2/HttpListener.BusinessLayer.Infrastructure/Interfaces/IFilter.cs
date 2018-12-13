using System;
using System.Linq.Expressions;
using HttpListener.BusinessLayer.Infrastructure.Models;

namespace HttpListener.BusinessLayer.Infrastructure.Interfaces
{
    /// <summary>
    /// Represents an <see cref="IFilter{T}"/> interface.
    /// </summary>
    public interface IFilter<T>
        where T : class
    {
        /// <summary>
        /// Build a Linq expression.
        /// </summary>
        Expression<Func<T, bool>> BuildExpression(SearchInfo searchInfo);
    }
}
