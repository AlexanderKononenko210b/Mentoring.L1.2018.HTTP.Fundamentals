using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using HttpListener.BusinessLayer.Infrastructure.Interfaces;
using HttpListener.BusinessLayer.Infrastructure.Models;
using OfficeOpenXml;

namespace HttpListener.BusinessLayer.Converters
{
    /// <summary>
    /// Represents a <see cref="Converter"/> class.
    /// </summary>
    public class Converter : IConverter
    {
        ///<inheritdoc/>
        public void ToExcelFormat(IEnumerable<OrderView> orders, MemoryStream stream)
        {
            using (var excelApp = new ExcelPackage())
            {
                var writer = excelApp.Workbook.Worksheets.Add("Order List");
                writer.Cells.LoadFromCollection(orders, true);
                writer.Cells.AutoFitColumns();
                excelApp.SaveAs(stream);
            }
        }

        ///<inheritdoc/>
        public void ToXmlFormat(IEnumerable<OrderView> orders, MemoryStream stream)
        {
            XDocument document = new XDocument(
                new XDeclaration("1.0", "utf-8", string.Empty),
                new XElement("orderList",
                    from order in orders
                    select
                        new XElement("order",
                            new XAttribute("id", order.OrderID),
                            new XElement("customerId", order.CustomerID),
                            new XElement("employeeId", order.EmployeeID),
                            new XElement("orderDate", order.OrderDate.HasValue ? order.OrderDate.Value.ToShortDateString() : "empty"),
                            new XElement("requiredDate", order.RequiredDate.HasValue ? order.RequiredDate.Value.ToShortDateString() : "empty"),
                            new XElement("requiredDate", order.ShippedDate.HasValue ? order.ShippedDate.Value.ToShortDateString() : "empty"),
                            new XElement("shipVia", order.ShipVia.HasValue ? order.ShipVia.ToString() : "empty"),
                            new XElement("freight", order.Freight.HasValue ? order.Freight.ToString() : "empty"),
                            new XElement("shipName", order.ShipName ?? "empty"),
                            new XElement("shipAddress", order.ShipAddress ?? "empty"),
                            new XElement("shipCity", order.ShipCity ?? "empty"),
                            new XElement("shipRegion", order.ShipRegion ?? "empty"),
                            new XElement("shipPostalCode", order.ShipPostalCode ?? "empty"),
                            new XElement("shipCountry", order.ShipCountry ?? "empty")
                        )));

            document.Save(stream);
        }
    }
}
