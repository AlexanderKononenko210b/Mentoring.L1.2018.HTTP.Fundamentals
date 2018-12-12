using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HttpListener.DataLayer.Infrastructure.Interfaces;
using HttpListener.DataLayer.Infrastructure.Models;

namespace HttpListener.DataLayer
{
    /// <summary>
    /// Represents a <see cref="OrderRepository"/> class.
    /// </summary>
    public class OrderRepository : IRepository<Order>
    {
        private readonly NorthwindContext _context;

        /// <summary>
        /// Initialize a <see cref="OrderRepository"/> instance.
        /// </summary>
        /// <param name="context"></param>
        public OrderRepository(NorthwindContext context)
        {
            _context = context;
        }

        ///<inheritdoc/>
        public IEnumerable<Order> GetOrders(Expression<Func<Order, bool>> predicate)
        {
            return _context.Set<Order>().Where(predicate);
        }
    }
}
