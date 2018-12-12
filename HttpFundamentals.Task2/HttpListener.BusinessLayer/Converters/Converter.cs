using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
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
            var serializer = new XmlSerializer(typeof(List<OrderView>));
            serializer.Serialize(stream, orders);
        }

        ///<inheritdoc/>
        public IEnumerable<OrderView> FromXmlFormat(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(List<OrderView>));
            return serializer.Deserialize(stream) as List<OrderView>;
        }
    }
}
