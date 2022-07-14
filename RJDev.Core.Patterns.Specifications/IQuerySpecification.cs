using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RJDev.Core.Patterns.Specifications
{
    public interface IQuerySpecification<TEntity> : ISpecification<TEntity> where TEntity : class
    {
        /// <summary>
        /// Collection with order by expressions.
        /// </summary>
        IList<(SpecificationSortType sortType, Expression<Func<TEntity, object>> selector)> OrderBy { get; }

        /// <summary>
        /// The number of items to skip.
        /// </summary>
        int? Skip { get; }

        /// <summary>
        /// The number of items to select.
        /// </summary>
        int? Take { get; }

        /// <summary>
        /// Merge specifications by logical AND operator.
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        new IQuerySpecification<TEntity> And(ISpecification<TEntity> specification);

        /// <summary>
        /// Merge specifications by logical AND operator.
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        new IQuerySpecification<TEntity> And(IQuerySpecification<TEntity> specification);

        /// <summary>
        /// Convert <see cref="IQuerySpecification{TEntity}"/> to <see cref="IMappedQuerySpecification{TEntity,TTarget}"/>.
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="postAction"></param>
        /// <returns></returns>
        IMappedQuerySpecification<TEntity, TTarget> ToMappedQuerySpecification<TTarget>(Expression<Func<TEntity, TTarget>> selector, Action<TTarget>? postAction = null)
            where TTarget : class;
    }
}