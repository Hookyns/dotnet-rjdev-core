using System;
using System.Linq.Expressions;

namespace RJDev.Core.Patterns.Specifications
{
    public interface ISpecification<TEntity> where TEntity : class
    {
        /// <summary>
        /// Select criterium/condition.
        /// </summary>
        Expression<Func<TEntity, bool>>? Criteria { get; }

        /// <summary>
        /// Merge specifications by logical AND operator.
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        ISpecification<TEntity> And(ISpecification<TEntity> specification);

        /// <summary>
        /// Merge specifications by logical AND operator.
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        IQuerySpecification<TEntity> And(IQuerySpecification<TEntity> specification);

        /// <summary>
        /// Merge specifications by logical AND operator.
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        IMappedQuerySpecification<TEntity, TTarget> And<TTarget>(IMappedQuerySpecification<TEntity, TTarget> specification) where TTarget : class;

        /// <summary>
        /// Merge specifications by logical AND operator, if <see cref="predicate"/> is true.
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        ISpecification<TEntity> AndIf(ISpecification<TEntity> specification, bool predicate);

        /// <summary>
        /// Merge specifications by logical AND operator, if <see cref="value"/> is not null or empty.
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        ISpecification<TEntity> AndIfNotEmpty(ISpecification<TEntity> specification, string? value);

        /// <summary>
        /// Convert <see cref="ISpecification{TEntity}"/> to <see cref="IQuerySpecification{TEntity}"/>.
        /// </summary>
        /// <param name="configurator"></param>
        /// <returns></returns>
        IQuerySpecification<TEntity> ToQuerySpecification(Action<BaseQuerySpecificationBuilder<TEntity>> configurator);

        /// <summary>
        /// Convert <see cref="ISpecification{TEntity}"/> to <see cref="IMappedQuerySpecification{TEntity,TTarget}"/>.
        /// </summary>
        /// <returns></returns>
        IMappedQuerySpecification<TEntity, TEntity> ToMappedQuerySpecification();
    }
}