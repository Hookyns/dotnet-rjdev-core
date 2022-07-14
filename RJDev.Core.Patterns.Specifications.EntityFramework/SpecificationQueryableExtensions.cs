using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RJDev.Core.Patterns.Specifications.EntityFramework
{
	public static class SpecificationQueryableExtensions
	{
        /// <summary>
		/// Aplikuje <see cref="IMappedQuerySpecification{TEntity,TTarget}"/> a vrátí vyčíslený výsledek.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="specification"></param>
		/// <param name="defaultSort">Výchozí řadící kritérium.</param>
		/// <param name="cancellationToken"></param>
		/// <typeparam name="TType"></typeparam>
		/// <typeparam name="TTarget"></typeparam>
		/// <returns></returns>
		public static async Task<IReadOnlyList<TTarget>> ResolveSpecificationAsync<TType, TTarget>(
			this IQueryable<TType> queryable,
			IMappedQuerySpecification<TType, TTarget>? specification,
			(SpecificationSortType sortType, Expression<Func<TType, object>> selector) defaultSort,
			CancellationToken cancellationToken = default
		)
			where TType : class
			where TTarget : class
		{
			if (specification == null)
			{
				return (await queryable.ToListAsync(cancellationToken)).Cast<TTarget>().ToList();
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

			List<TTarget> result = await selectedQuery.ToListAsync(cancellationToken);

			if (specification.PostAction != null)
			{
				result.ForEach(specification.PostAction);
			}

			return result;
		}
	}
}