using System.Collections.Specialized;
using System.IO;
using HttpListener.BusinessLayer.Infrastructure.Models;

namespace HttpListener.BusinessLayer.Infrastructure.Interfaces
{
    /// <summary>
    /// Represents an <see cref="IParser"/> interface.
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Get data from query string.
        /// </summary>
        /// <param name="queryStrings">The query string.</param>
        /// <returns></returns>
        SearchInfo ParseQuery(NameValueCollection queryStrings);

        /// <summary>
        /// Get data from body.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <returns></returns>
        SearchInfo ParseBody(Stream inputStream);
    }
}
