﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HttpListener.BusinessLayer.Infrastructure.Enums;
using HttpListener.BusinessLayer.Infrastructure.Interfaces;

namespace HttpListener.BusinessLayer
{
    /// <summary>
    /// Represents a <see cref="ExpressionBuilder{T}"/> class
    /// </summary>
    public class ExpressionBuilder<T> : IFilter<T> where T : class
    {
        private readonly Dictionary<Operation, Func<Expression, Expression, Expression>> _expessions =
            new Dictionary<Operation, Func<Expression, Expression, Expression>>();

        /// <summary>
        /// Initialize a <see cref="ExpressionBuilder{T}"/> instance.
        /// </summary>
        public ExpressionBuilder()
        {
            Initializer();
        }

        ///<inheritdoc/>
        public List<IFilterStatement> Statements { get; set; } = 
            new List<IFilterStatement>();

        ///<inheritdoc/>
        public Expression<Func<T, bool>> BuildExpression()
        {
            Expression finalExpression = null;
            var parameter = Expression.Parameter(typeof(T), "item");

            foreach (var statement in Statements)
            {
                var member = GetMemberExpression(parameter, statement.PropertyName);
                Expression constant;

                if (IsNullableType(member.Type))
                {
                    constant = Expression.Convert(Expression.Constant(statement.Value), member.Type);
                }
                else
                {
                    constant = Expression.Constant(statement.Value);
                }

                var expression = _expessions[statement.Operation].Invoke(member, constant);
                finalExpression = finalExpression == null ? expression : Expression.AndAlso(finalExpression, expression);
            }

            return finalExpression != null ? Expression.Lambda<Func<T, bool>>(finalExpression, parameter) : null;
        }

        /// <summary>
        /// Initialize expression dictionary.
        /// </summary>
        private void Initializer()
        {
            _expessions.Add(Operation.EqualTo, (member, constant) => Expression.Equal(member, constant));
            _expessions.Add(Operation.NotEqualTo, (member, constant) => Expression.NotEqual(member, constant));
            _expessions.Add(Operation.GreaterThan, (member, constant) => Expression.GreaterThan(member, constant));
            _expessions.Add(Operation.GreaterThanOrEqualTo, (member, constant) => Expression.GreaterThanOrEqual(member, constant));
            _expessions.Add(Operation.LessThan, (member, constant) => Expression.LessThan(member, constant));
            _expessions.Add(Operation.LessThanOrEqualTo, (member, constant) => Expression.LessThanOrEqual(member, constant));
        }

        /// <summary>
        /// Get member expression.
        /// </summary>
        /// <param name="param">The parameter</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The <see cref="MemberExpression"/></returns>
        private MemberExpression GetMemberExpression(Expression param, string propertyName)
        {
            if (propertyName.Contains("."))
            {
                var index = propertyName.IndexOf(".", StringComparison.InvariantCulture);
                var subParam = Expression.Property(param, propertyName.Substring(0, index));
                return GetMemberExpression(subParam, propertyName.Substring(index + 1));
            }

            return Expression.Property(param, propertyName);
        }

        /// <summary>
        /// Check nullable type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
