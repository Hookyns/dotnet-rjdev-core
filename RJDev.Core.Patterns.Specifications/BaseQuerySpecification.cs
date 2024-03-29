using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RJDev.Core.Patterns.Specifications
{
    public class BaseQuerySpecification<TEntity> : BaseSpecification<TEntity>, IQuerySpecification<TEntity>
        where TEntity : class
    {
        /// <inheritdoc />
        public IList<(SpecificationSortType sortType, Expression<Func<TEntity, object>> selector)> OrderBy { get; protected internal init; } =
            new List<(SpecificationSortType sortType, Expression<Func<TEntity, object>> selector)>();

        /// <inheritdoc />
        public int? Skip { get; init; }

        /// <inheritdoc />
        public int? Take { get; init; }

        /// <inheritdoc />
        public IMappedQuerySpecification<TEntity, TTarget> ToMappedQuerySpecification<TTarget>(Expression<Func<TEntity, TTarget>> selector, Action<TTarget>? postAction = null)
            where TTarget : class
        {
            return new BaseMappedQuerySpecification<TEntity, TTarget>
            {
                Criteria = Criteria,
                OrderBy = OrderBy,
                Skip = Skip,
                Take = Take,
                Selector = selector,
                PostAction = postAction
            };
        }

        /// <inheritdoc />
        public override IMappedQuerySpecification<TEntity, TEntity> ToMappedQuerySpecification()
        {
            return new BaseMappedQuerySpecification<TEntity, TEntity>
            {
                Criteria = Criteria,
                OrderBy = OrderBy,
                Skip = Skip,
                Take = Take,
                Selector = entity => entity
            };
        }

        /// <inheritdoc cref="IQuerySpecification{TEntity}.And(RJDev.Core.Patterns.Specifications.IQuerySpecification{TEntity})" />
        public override IQuerySpecification<TEntity> And(IQuerySpecification<TEntity> specification)
        {
            AssertLimits(specification.Skip, specification.Take, specification.GetType());

            return new BaseQuerySpecification<TEntity>()
            {
                Criteria = ResolveCriteria(specification),
                OrderBy = MergeOrders(OrderBy, specification.OrderBy),
                Skip = specification.Skip ?? Skip,
                Take = specification.Take ?? Take
            };
        }

        /// <inheritdoc />
        public override IMappedQuerySpecification<TEntity, TTarget> And<TTarget>(IMappedQuerySpecification<TEntity, TTarget> specification)
            where TTarget : class
        {
            AssertLimits(specification.Skip, specification.Take, specification.GetType());

            return new BaseMappedQuerySpecification<TEntity, TTarget>()
            {
                Criteria = ResolveCriteria(specification),
                OrderBy = MergeOrders(OrderBy, specification.OrderBy),
                Skip = specification.Skip ?? Skip,
                Take = specification.Take ?? Take,
                Selector = specification.Selector,
                PostAction = specification.PostAction
            };
        }

        /// <inheritdoc />
        IQuerySpecification<TEntity> IQuerySpecification<TEntity>.And(ISpecification<TEntity> specification)
        {
            return new BaseQuerySpecification<TEntity>()
            {
                Criteria = ResolveCriteria(specification),
                OrderBy = OrderBy,
                Skip = Skip,
                Take = Take
            };
        }

        /// <summary>
        /// Merge <see cref="OrderBy"/> expressions.
        /// </summary>
        /// <param name="sourceOrderBy"></param>
        /// <param name="specificationOrderBy"></param>
        /// <returns></returns>
        public static List<(SpecificationSortType sortType, Expression<Func<TEntity, object>> selector)> MergeOrders(
            IEnumerable<(SpecificationSortType sortType, Expression<Func<TEntity, object>> selector)> sourceOrderBy,
            IEnumerable<(SpecificationSortType sortType, Expression<Func<TEntity, object>> selector)> specificationOrderBy
        )
        {
            var newOrders = sourceOrderBy.ToList();
            newOrders.AddRange(specificationOrderBy);
            return newOrders;
        }

        /// <summary>
        /// Assert <see cref="Skip"/> a <see cref="Take"/> values.
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="type"></param>
        /// <exception cref="InvalidOperationException"></exception>
        protected void AssertLimits(int? skip, int? take, Type type)
        {
            if (Skip.HasValue && skip.HasValue && Skip != skip)
            {
                throw new InvalidOperationException("Unable to merge two specifications with different 'Skip' values." +
                    $"{Environment.NewLine}Left specification '{GetType().FullName}' = {Skip}, right specification '{type.FullName}' = {skip}.");
            }

            if (Take.HasValue && take.HasValue && Take != take)
            {
                throw new InvalidOperationException("Unable to merge two specifications with different 'Take' values." +
                    $"{Environment.NewLine}Left specification '{GetType().FullName}' = {Take}, right specification '{type.FullName}' = {take}.");
            }
        }

        protected void OrderByAsc(Expression<Func<TEntity, object>> expression)
        {
            OrderBy.Add((SpecificationSortType.Ascending, expression));
        }

        protected void OrderByDesc(Expression<Func<TEntity, object>> expression)
        {
            OrderBy.Add((SpecificationSortType.Descending, expression));
        }
    }
}