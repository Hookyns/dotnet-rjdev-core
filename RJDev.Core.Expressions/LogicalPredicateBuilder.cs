using System;
using System.Linq.Expressions;

namespace RJDev.Core.Expressions
{
    public class LogicalPredicateBuilder<TParam>
    {
        /// <summary>
        /// Current predicate
        /// </summary>
        private Expression<Func<TParam, bool>> _predicate;

        public LogicalPredicateBuilder(Expression<Func<TParam, bool>> predicate)
        {
            _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        }

        /// <summary>
        /// Add another predicate into the builder and join it's body by logical AND operator.
        /// </summary>
        /// <param name="anotherPredicate"></param>
        /// <returns></returns>
        public LogicalPredicateBuilder<TParam> And(Expression<Func<TParam, bool>> anotherPredicate)
        {
            _predicate = Expression.Lambda<Func<TParam, bool>>(
                Expression.AndAlso(_predicate.Body, GetFixedBody(anotherPredicate)),
                _predicate.Parameters
            );
            return this;
        }

        /// <summary>
        /// Add another predicate into the builder and join it's body by logical OR operator.
        /// </summary>
        /// <param name="anotherPredicate"></param>
        /// <returns></returns>
        public LogicalPredicateBuilder<TParam> Or(Expression<Func<TParam, bool>> anotherPredicate)
        {
            _predicate = Expression.Lambda<Func<TParam, bool>>(
                Expression.OrElse(_predicate.Body, GetFixedBody(anotherPredicate)),
                _predicate.Parameters
            );
            return this;
        }

        /// <summary>
        /// Returns final Expression.
        /// </summary>
        /// <returns></returns>
        public Expression<Func<TParam, bool>> Build()
        {
            return _predicate;
        }

        private Expression GetFixedBody(Expression<Func<TParam, bool>> anotherPredicate)
        {
            Expression right = ReplaceParameterVisitor.Replace(
                anotherPredicate.Parameters[0],
                _predicate.Parameters[0],
                anotherPredicate.Body
            );
            return right;
        }
    }
}