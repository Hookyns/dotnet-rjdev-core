using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RJDev.Core.Patterns.Specifications
{
    /// <summary>
    /// Empty specification with <code>() => true</code> criteria.
    /// Empty specification is excluded from query after adding other specification.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EmptyQuerySpecification<TEntity> : EmptySpecification<TEntity>, IQuerySpecification<TEntity>
        where TEntity : class
    {
        /// <inheritdoc />
        public IList<(SpecificationSortType sortType, Expression<Func<TEntity, object>> selector)> OrderBy { get; }
            = new List<(SpecificationSortType sortType, Expression<Func<TEntity, object>> selector)>();

        /// <inheritdoc />
        public int? Skip => null;

        /// <inheritdoc />
        public int? Take => null;

        public EmptyQuerySpecification()
        {
            this.Criteria = x => true;
        }

        /// <inheritdoc />
        public new IQuerySpecification<TEntity> And(ISpecification<TEntity> specification)
        {
            return new BaseQuerySpecification<TEntity>()
            {
                Criteria = specification.Criteria
            };
        }

        /// <inheritdoc />
        public IMappedQuerySpecification<TEntity, TTarget> ToMappedQuerySpecification<TTarget>(Expression<Func<TEntity, TTarget>> selector, Action<TTarget>? postAction = null)
            where TTarget : class
        {
            return new BaseMappedQuerySpecification<TEntity, TTarget>
            {
                Criteria = this.Criteria,
                OrderBy = this.OrderBy,
                Skip = this.Skip,
                Take = this.Take,
                Selector = selector,
                PostAction = postAction
            };
        }
    }
}