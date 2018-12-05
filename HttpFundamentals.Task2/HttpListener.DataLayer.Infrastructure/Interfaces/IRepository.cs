using System;
using System.Collections.Generic;

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
        IEnumerable<T> GetOrders(Func<T, bool> predicate);
    }
}
