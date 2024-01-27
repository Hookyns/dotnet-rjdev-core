using System;
using System.Linq.Expressions;

namespace RJDev.Core.Expressions
{
    public class ReplaceParameterVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _oldParameter;
        private readonly ParameterExpression _newParameter;

        private ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            _oldParameter = oldParameter;
            _newParameter = newParameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _oldParameter ? _newParameter : base.VisitParameter(node);
        }

        public static Expression Replace(ParameterExpression oldParameter, ParameterExpression newParameter, Expression body)
        {
            ReplaceParameterVisitor visitor = new(
                oldParameter,
                newParameter
            );

            return visitor.Visit(body) ?? throw new NullReferenceException("Replacement of the parameter failed.");
        }
    }
}