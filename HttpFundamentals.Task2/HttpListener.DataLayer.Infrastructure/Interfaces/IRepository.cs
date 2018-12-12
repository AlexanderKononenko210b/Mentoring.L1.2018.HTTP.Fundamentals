using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HttpListener.DataLayer.Infrastructure.Models;

namespace HttpListener.DataLayer.Infrastructure.Interfaces
{
    /// <summary>
    /// Represents an <see cref="IRepository{T}"/> interface.
    /// </summary>
    public interface IRepository<T> 
        where T : class
    {
        /// <summary>
        /// Get orders by predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The <see cref="IEnumerable{T}"/></returns>
        IEnumerable<T> GetOrders(Expression<Func<Order, bool>> predicate);
    }
}
