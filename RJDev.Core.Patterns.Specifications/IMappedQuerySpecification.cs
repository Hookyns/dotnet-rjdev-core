using System;
using System.Linq.Expressions;

namespace RJDev.Core.Patterns.Specifications
{
    public interface IMappedQuerySpecification<TEntity, TTarget> : IQuerySpecification<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Selector for custom specific dataset.
        /// </summary>
        Expression<Func<TEntity, TTarget>> Selector { get; }

        /// <summary>
        /// Optional action executed on each item after selection.
        /// </summary>
        Action<TTarget>? PostAction { get; }

        /// <summary>
        /// Merge specifications by logical AND operator.
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        new IMappedQuerySpecification<TEntity, TTarget> And(ISpecification<TEntity> specification);

        /// <summary>
        /// Merge specifications by logical AND operator.
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        new IMappedQuerySpecification<TEntity, TTarget> And(IQuerySpecification<TEntity> specification);

        /// <summary>
        /// Merge specifications by logical AND operator.
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        IMappedQuerySpecification<TEntity, TTarget> And(IMappedQuerySpecification<TEntity, TTarget> specification);
    }
}