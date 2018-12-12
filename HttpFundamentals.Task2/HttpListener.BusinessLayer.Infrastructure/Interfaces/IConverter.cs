using System.Collections.Generic;
using System.IO;
using HttpListener.BusinessLayer.Infrastructure.Models;

namespace HttpListener.BusinessLayer.Infrastructure.Interfaces
{
    /// <summary>
    /// Represents an <see cref="IConverter"/> interface.
    /// </summary>
    public interface IConverter
    {
        /// <summary>
        /// Convert data to excel stream.
        /// </summary>
        /// <param name="orders"><see cref="IEnumerable{Order}"/></param>
        /// <param name="stream">The <see cref="MemoryStream"/></param>
        void ToExcelFormat(IEnumerable<OrderView> orders, MemoryStream stream);

        /// <summary>
        /// Convert data to xml stream.
        /// </summary>
        /// <param name="orders">The <see cref="IEnumerable{Order}"/></param>
        /// <param name="stream">The <see cref="MemoryStream"/></param>
        void ToXmlFormat(IEnumerable<OrderView> orders, MemoryStream stream);

        /// <summary>
        /// Convert stream to <see cref="IEnumerable{OrderView}"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/></param>
        /// <returns>The <see cref="IEnumerable{OrderView}"/></returns>
        IEnumerable<OrderView> FromXmlFormat(Stream stream);
    }
}
