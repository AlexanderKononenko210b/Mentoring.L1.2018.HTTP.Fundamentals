using HttpListener.BusinessLayer.Infrastructure.Enums;
using HttpListener.BusinessLayer.Infrastructure.Interfaces;

namespace HttpListener.BusinessLayer.Infrastructure.Models
{
    /// <summary>
    /// Represents a <see cref="FilterStatement"/> class.
    /// </summary>
    public class FilterStatement : IFilterStatement
    {
        /// <summary>
        /// Gets or sets property name.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets operation.
        /// </summary>
        public Operation Operation { get; set; }

        /// <summary>
        /// Gets or sets value.
        /// </summary>
        public object Value { get; set; }
    }
}
