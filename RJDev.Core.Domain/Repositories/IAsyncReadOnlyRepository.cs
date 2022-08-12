using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RJDev.Core.Essentials.Results;
using RJDev.Core.Patterns.Specifications;

namespace RJDev.Core.Domain.Repositories
{
    public interface IAsyncReadOnlyRepository<TEntity, TKey>
        where TEntity : class
    {
        /// <summary>
        /// Get entity from repository by id.
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IResult<TEntity>> GetByIdAsync(TKey entityId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get readonly collection of all the entities
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TItem"></typeparam>
        /// <returns></returns>
        Task<IReadOnlyList<TItem>> GetAll<TItem>(CancellationToken cancellationToken = default)
            where TItem : class;

        /// <summary>
        /// Get readonly collection of entities selected by the specification.
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TItem"></typeparam>
        /// <returns></returns>
        Task<IReadOnlyList<TItem>> GetAll<TItem>(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
            where TItem : class;

        /// <summary>
        /// Get readonly collection of entities selected by the query specification.
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TItem"></typeparam>
        /// <returns></returns>
        Task<IReadOnlyList<TItem>> GetAll<TItem>(IQuerySpecification<TEntity> specification, CancellationToken cancellationToken = default)
            where TItem : class;

        /// <summary>
        /// Get readonly collection of entities selected by the mapped query specification.
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TItem"></typeparam>
        /// <returns></returns>
        Task<IReadOnlyList<TItem>> GetAll<TItem>(IMappedQuerySpecification<TEntity, TItem> specification, CancellationToken cancellationToken = default)
            where TItem : class;
    }
}