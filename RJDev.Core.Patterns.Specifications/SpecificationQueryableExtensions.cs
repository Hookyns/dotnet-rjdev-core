using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RJDev.Core.Patterns.Specifications
{
    public static class SpecificationQueryableExtensions
    {
        /// <summary>
        /// Apply <see cref="ISpecification{TEntity}.Criteria"/> on the <see cref="Queryable"/> collection via Where().
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="specification"></param>
        /// <typeparam name="TType"></typeparam>
        /// <returns></returns>
        public static IQueryable<TType> ApplyCriteria<TType>(this IQueryable<TType> queryable, ISpecification<TType> specification)
            where TType : class
        {
            if (specification.Criteria != null)
            {
                return queryable.Where(specification.Criteria);
            }

            return queryable;
        }

        /// <summary>
        /// Apply given specification on the <see cref="Queryable"/> collection.
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="specification"></param>
        /// <typeparam name="TType"></typeparam>
        /// <returns></returns>
        public static IQueryable<TType> ApplySpecification<TType>(this IQueryable<TType> queryable, ISpecification<TType> specification)
            where TType : class
        {
            return queryable.ApplyCriteria(specification);
        }

        /// <summary>
        /// Apply given specification on the <see cref="Queryable"/> collection.
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="specification"></param>
        /// <typeparam name="TType"></typeparam>
        /// <returns></returns>
        public static IQueryable<TType> ApplySpecification<TType>(this IQueryable<TType> queryable, IQuerySpecification<TType> specification)
            where TType : class
        {
            IQueryable<TType> query = specification.Criteria != null ? queryable.Where(specification.Criteria) : queryable;

            foreach ((SpecificationSortType sortType, Expression<Func<TType, object>> selector) in specification.OrderBy)
            {
                query = sortType == SpecificationSortType.Ascending
                    ? query.OrderBy(selector)
                    : query.OrderByDescending(selector);
            }

            if (specification.Skip.HasValue)
            {
                query = query.Skip(specification.Skip.Value);
            }

            if (specification.Take.HasValue)
            {
                query = query.Take(specification.Take.Value);
            }

            return query;
        }

        /// <summary>
        /// Apply given specification on the <see cref="Queryable"/> collection.
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="specification"></param>
        /// <typeparam name="TType"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <returns></returns>
        public static IQueryable<TType> ApplyQuerySpecification<TType, TTarget>(this IQueryable<TType> queryable, IMappedQuerySpecification<TType, TTarget> specification)
            where TType : class
            where TTarget : class
        {
            return queryable.ApplySpecification(specification);
        }

        /// <summary>
        /// Apply given specification on the <see cref="Queryable"/> collection and return results.
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="specification"></param>
        /// <param name="defaultSort">Výchozí řadící kritérium.</param>
        /// <typeparam name="TType"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <returns></returns>
        public static IReadOnlyList<TTarget> ResolveSpecification<TType, TTarget>(
            this IQueryable<TType> queryable,
            IMappedQuerySpecification<TType, TTarget>? specification,
            (SpecificationSortType sortType, Expression<Func<TType, object>> selector) defaultSort
        )
            where TType : class
            where TTarget : class
        {
            if (specification == null)
            {
                return queryable.ToList().Cast<TTarget>().ToList();
            }

            IQueryable<TType> query = specification.Criteria != null ? queryable.Where(specification.Criteria) : queryable;
            var orderBy = specification.OrderBy;

            if (orderBy.Count == 0)
            {
                orderBy = new[] { defaultSort };
            }

            foreach ((SpecificationSortType sortType, Expression<Func<TType, object>> selector) in orderBy)
            {
                query = sortType == SpecificationSortType.Ascending
                    ? query.OrderBy(selector)
                    : query.OrderByDescending(selector);
            }

            IQueryable<TTarget> selectedQuery = query.Select(specification.Selector);

            if (specification.Skip.HasValue)
            {
                selectedQuery = selectedQuery.Skip(specification.Skip.Value);
            }

            if (specification.Take.HasValue)
            {
                selectedQuery = selectedQuery.Take(specification.Take.Value);
            }

            List<TTarget> result = selectedQuery.ToList();

            if (specification.PostAction != null)
            {
                result.ForEach(specification.PostAction);
            }

            return result;
        }
    }
}