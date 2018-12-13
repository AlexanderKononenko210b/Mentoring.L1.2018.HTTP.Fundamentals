using System;
using HttpListener.BusinessLayer.Infrastructure.Enums;

namespace HttpListener.BusinessLayer.Infrastructure.Attributes
{
    /// <summary>
    /// Represents a <see cref="ExpressionBuilderAttribute"/> class.
    /// </summary>
    public class ExpressionBuilderAttribute : Attribute
    {
        /// <summary>
        /// Initialize a new <see cref="ExpressionBuilderAttribute"/> instance.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="operation"></param>
        public ExpressionBuilderAttribute(string propertyName, Operation operation)
        {
            PropertyName = propertyName;
            Operation = operation;
        }

        /// <summary>
        /// Gets or sets property name.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets operation.
        /// </summary>
        public Operation Operation { get; set; }
    }
}
