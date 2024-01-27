using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RJDev.Core.Patterns.Specifications
{
    public class BaseQuerySpecificationBuilder<TEntity>
        where TEntity : class
    {
        private readonly List<(SpecificationSortType sortType, Expression<Func<TEntity, object>> selector)> _orderBy = new();
        protected int? _skip;
        protected int? _take;

        /// <summary>
        /// Add <see cref="SpecificationSortType.Ascending"/> order by expression.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public BaseQuerySpecificationBuilder<TEntity> OrderBy(Expression<Func<TEntity, object>> selector)
        {
            _orderBy.Add((SpecificationSortType.Ascending, selector));
            return this;
        }

        /// <summary>
        /// Add <see cref="SpecificationSortType.Descending"/> order by expression.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public BaseQuerySpecificationBuilder<TEntity> OrderByDescending(Expression<Func<TEntity, object>> selector)
        {
            _orderBy.Add((SpecificationSortType.Descending, selector));
            return this;
        }

        /// <summary>
        /// Set the number of items to select.
        /// </summary>
        /// <param name="takeCount"></param>
        /// <returns></returns>
        public BaseQuerySpecificationBuilder<TEntity> Take(int takeCount)
        {
            _take = takeCount;
            return this;
        }

        /// <summary>
        /// Set the number of items to skip.
        /// </summary>
        /// <param name="skipCount"></param>
        /// <returns></returns>
        public BaseQuerySpecificationBuilder<TEntity> Skip(int skipCount)
        {
            _skip = skipCount;
            return this;
        }

        /// <summary>
        /// Build <see cref="IQuerySpecification{TEntity}"/>.
        /// </summary>
        /// <returns></returns>
        public IQuerySpecification<TEntity> Build()
        {
            return new BaseQuerySpecification<TEntity>()
            {
                Skip = _skip,
                Take = _take,
                OrderBy = _orderBy.ToList()
            };
        }
    }
}