using System;
using System.Xml.Serialization;

namespace HttpListener.BusinessLayer.Infrastructure.Models
{
    /// <summary>
    /// Represents a model <see cref="OrderView"/> class.
    /// </summary>
    [XmlRoot(ElementName = "catalog")]
    public class OrderView
    {
        [XmlAttribute(AttributeName = "id")]
        public int OrderID { get; set; }

        [XmlElement(ElementName = "customerId")]
        public string CustomerID { get; set; }

        [XmlElement(ElementName = "employeeID")]
        public int? EmployeeID { get; set; }

        [XmlElement(ElementName = "orderDate")]
        public DateTime? OrderDate { get; set; }

        [XmlElement(ElementName = "requiredDate")]
        public DateTime? RequiredDate { get; set; }

        [XmlElement(ElementName = "shippedDate")]
        public DateTime? ShippedDate { get; set; }

        [XmlElement(ElementName = "shipVia")]
        public int? ShipVia { get; set; }

        [XmlElement(ElementName = "freight")]
        public decimal? Freight { get; set; }

        [XmlElement(ElementName = "shipName")]
        public string ShipName { get; set; }

        [XmlElement(ElementName = "shipAddress")]
        public string ShipAddress { get; set; }

        [XmlElement(ElementName = "shipCity")]
        public string ShipCity { get; set; }

        [XmlElement(ElementName = "shipRegion")]
        public string ShipRegion { get; set; }

        [XmlElement(ElementName = "shipPostalCode")]
        public string ShipPostalCode { get; set; }

        [XmlElement(ElementName = "shipCountry")]
        public string ShipCountry { get; set; }
    }
}
