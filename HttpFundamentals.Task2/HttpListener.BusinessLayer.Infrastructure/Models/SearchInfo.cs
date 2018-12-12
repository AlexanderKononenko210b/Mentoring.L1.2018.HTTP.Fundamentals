using System;

namespace HttpListener.BusinessLayer.Infrastructure.Models
{
    /// <summary>
    /// Represents a <see cref="SearchInfo"/> class.
    /// </summary>
    public class SearchInfo
    {
        /// <summary>
        /// Gets or sets a customer id.
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets a 'from' date search boarder.
        /// </summary>
        public DateTime? From { get; set; }

        /// <summary>
        /// Gets or sets a 'to' date search boarder.
        /// </summary>
        public DateTime? To { get; set; }

        /// <summary>
        /// Gets or sets count orders skipping in search result.
        /// </summary>
        public int? Skip { get; set; }

        /// <summary>
        /// Gets or sets count orders to take in search result.
        /// </summary>
        public int? Take { get; set; }  
    }
}
