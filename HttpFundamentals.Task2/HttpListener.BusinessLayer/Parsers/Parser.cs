using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using HttpListener.BusinessLayer.Infrastructure.Interfaces;
using HttpListener.BusinessLayer.Infrastructure.Models;

namespace HttpListener.BusinessLayer.Parsers
{
    /// <summary>
    /// Represents a <see cref="Parser"/> class.
    /// </summary>
    public class Parser : IParser
    {
        ///<inheritdoc/>
        public SearchInfo ParseBody(Stream inputStream)
        {
            using (var reader = new StreamReader(inputStream))
            {
                var data = reader.ReadToEnd().Split('&');

                var searchInfo = new SearchInfo();
                searchInfo.CustomerId = GetIntParameter("customerId", data);
                searchInfo.From = GetDateTimeParameter("from", data);
                searchInfo.To = GetDateTimeParameter("to", data);
                searchInfo.Skip = GetIntParameter("skip", data);
                searchInfo.Take = GetIntParameter("take", data);

                return searchInfo;
            }
        }

        ///<inheritdoc/>
        public SearchInfo ParseQuery(NameValueCollection queryStrings)
        {
            if (queryStrings == null || queryStrings.Count == 0) return null;

            var searchInfo = new SearchInfo();
            searchInfo.CustomerId = queryStrings["customerId"] != null && int.TryParse(queryStrings["CustomerId"], out var customerId) ? (int?)customerId : null;
            searchInfo.From = queryStrings["from"] != null && DateTime.TryParse(queryStrings["from"], out var fromDateTime) ? (DateTime?)fromDateTime : null;
            searchInfo.To = queryStrings["to"] != null && DateTime.TryParse(queryStrings["to"], out var toDateTime) ? (DateTime?)toDateTime : null;
            searchInfo.Skip = queryStrings["skip"] != null && int.TryParse(queryStrings["skip"], out var skipResult) ? (int?)skipResult : null;
            searchInfo.Take = queryStrings["take"] != null && int.TryParse(queryStrings["take"], out var takeResult) ? (int?)takeResult : null;

            return searchInfo;
        }

        /// <summary>
        /// Get int parameter from body data.
        /// </summary>
        /// <param name="name">The name parameter.</param>
        /// <param name="data">The body data as string[].</param>
        /// <returns></returns>
        private int? GetIntParameter(string name, string[] data)
        {
            var parameterKeyValue = data.FirstOrDefault(param => param.StartsWith(name))?.Split('=');
            return parameterKeyValue != null &&
                   parameterKeyValue.Length == 2 &&
                                    int.TryParse(parameterKeyValue[1], out var parameterValue) ? (int?)parameterValue : null;
        }

        /// <summary>
        /// Get DateTime parameter from body data.
        /// </summary>
        /// <param name="name">The name parameter.</param>
        /// <param name="data">The body data as string[].</param>
        /// <returns></returns>
        private DateTime? GetDateTimeParameter(string name, string[] data)
        {
            var parameterKeyValue = data.FirstOrDefault(param => param.StartsWith(name))?.Split('=');
            return parameterKeyValue != null &&
                   parameterKeyValue.Length == 2 &&
                   DateTime.TryParse(parameterKeyValue[1], out var parameterValue) ? (DateTime?)parameterValue : null;
        }
    }
}
