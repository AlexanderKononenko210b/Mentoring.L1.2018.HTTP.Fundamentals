using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HttpListener.BusinessLayer.Infrastructure.Interfaces
{
    /// <summary>
    /// Represents an <see cref="IFilter{T}"/> interface.
    /// </summary>
    public interface IFilter<T>
        where T : class
    {
        /// <summary>
        /// Group of statements that compose this filter.
        /// </summary>
        List<IFilterStatement> Statements { get; set; }

        /// <summary>
        /// Build a Linq expression.
        /// </summary>
        Expression<Func<T, bool>> BuildExpression();
    }
}
