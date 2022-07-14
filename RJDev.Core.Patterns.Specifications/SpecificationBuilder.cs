using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using RJDev.Core.Expressions;

namespace RJDev.Core.Patterns.Specifications
{
    public class SpecificationBuilder<TEntity> where TEntity : class
    {
        private List<Expression<Func<TEntity, bool>>> criterias = new();

        /// <summary>
        /// Merge specifications by logical AND operator.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public SpecificationBuilder<TEntity> And(Expression<Func<TEntity, bool>> criteria)
        {
            this.criterias.Add(criteria);
            return this;
        }

        /// <summary>
        /// Merge specifications by logical AND operator, if <see cref="predicate"/> is true.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SpecificationBuilder<TEntity> AndIf(Expression<Func<TEntity, bool>> criteria, bool predicate)
        {
            if (predicate)
            {
                this.criterias.Add(criteria);
            }

            return this;
        }

        /// <summary>
        /// Merge specifications by logical AND operator, if <see cref="value"/> is not null or empty.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SpecificationBuilder<TEntity> AndIfNotEmpty(Expression<Func<TEntity, bool>> criteria, string? value)
        {
            return this.AndIf(criteria, !string.IsNullOrWhiteSpace(value));
        }

        /// <summary>
        /// Merge specifications by logical AND operator, if <see cref="value"/> is not null or zero.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SpecificationBuilder<TEntity> AndIfNotEmpty(Expression<Func<TEntity, bool>> criteria, int? value)
        {
            return this.AndIf(criteria, value != null && value != 0);
        }

        /// <summary>
        /// Merge specifications by logical AND operator, if <see cref="value"/> is not null or Empty.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SpecificationBuilder<TEntity> AndIfNotEmpty(Expression<Func<TEntity, bool>> criteria, Guid? value)
        {
            return this.AndIf(criteria, value != null && value != Guid.Empty);
        }

        /// <summary>
        /// Merge specifications by logical AND operator, if <see cref="value"/> is not null or default date.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SpecificationBuilder<TEntity> AndIfNotEmpty(Expression<Func<TEntity, bool>> criteria, DateTime? value)
        {
            return this.AndIf(criteria, value.HasValue && value.Value != default);
        }

        /// <summary>
        /// Build configured criterias to <see cref="ISpecification{TEntity}"/>
        /// </summary>
        /// <returns></returns>
        public ISpecification<TEntity> Build()
        {
            LogicalPredicateBuilder<TEntity> builder = new(x => true);

            foreach (Expression<Func<TEntity, bool>> criteria in this.criterias)
            {
                builder = builder.And(criteria);
            }

            return new BaseSpecification<TEntity>()
            {
                Criteria = builder.Build()
            };
        }
    }
}