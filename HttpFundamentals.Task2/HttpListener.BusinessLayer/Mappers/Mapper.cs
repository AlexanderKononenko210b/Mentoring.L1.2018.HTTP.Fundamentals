using HttpListener.BusinessLayer.Infrastructure.Interfaces;
using HttpListener.BusinessLayer.Infrastructure.Models;
using HttpListener.DataLayer.Infrastructure.Models;

namespace HttpListener.BusinessLayer.Mappers
{
    /// <summary>
    /// Represents a <see cref="Mapper"/> class.
    /// </summary>
    public class Mapper : IMapper<Order, OrderView>
    {
        /// <inheritdoc/>
        public OrderView Map(Order order)
        {
            if (order == null) return null;

            var orderView = new OrderView();

            orderView.OrderID = order.OrderID;
            orderView.CustomerID = order.CustomerID;
            orderView.EmployeeID = order.EmployeeID;
            orderView.Freight = order.Freight;
            orderView.OrderDate = order.OrderDate;
            orderView.RequiredDate = order.RequiredDate;
            orderView.ShippedDate = order.ShippedDate;
            orderView.ShipVia = order.ShipVia;
            orderView.ShipAddress = order.ShipAddress;
            orderView.ShipName = order.ShipName;
            orderView.ShipCity = order.ShipCity;
            orderView.ShipRegion = order.ShipRegion;
            orderView.ShipPostalCode = order.ShipPostalCode;
            orderView.ShipCountry = order.ShipCountry;

            return orderView;
        }
    }
}
