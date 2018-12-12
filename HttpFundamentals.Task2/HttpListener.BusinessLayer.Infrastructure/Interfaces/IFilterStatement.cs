using HttpListener.BusinessLayer.Infrastructure.Enums;

namespace HttpListener.BusinessLayer.Infrastructure.Interfaces
{
    /// <summary>
    /// Represents an <see cref="IFilterStatement"/> interface.
    /// </summary>
    public interface IFilterStatement
    {
        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the operation.
        /// </summary>
        Operation Operation { get; set; }

        /// <summary>
        /// Constant value that will interact with the property defined in this filter statement.
        /// </summary>
        object Value { get; set; }
    }
}
