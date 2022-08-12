using System.Collections.Generic;
using RJDev.Core.Essentials.Results;
using RJDev.Core.Patterns.Specifications;

namespace RJDev.Core.Domain.Repositories
{
    public interface IReadOnlyRepository<TEntity, in TKey>
        where TEntity : class
    {
        /// <summary>
        /// Get entity from repository by id.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        IResult<TEntity> GetById(TKey entityId);

        /// <summary>
        /// Get readonly collection of all the entities
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <returns></returns>
        IReadOnlyList<TItem> GetAll<TItem>() 
            where TItem : class;

        /// <summary>
        /// Get readonly collection of entities selected by the specification.
        /// </summary>
        /// <param name="specification"></param>
        /// <typeparam name="TItem"></typeparam>
        /// <returns></returns>
        IReadOnlyList<TItem> GetAll<TItem>(ISpecification<TEntity> specification)
            where TItem : class;

        /// <summary>
        /// Get readonly collection of entities selected by the query specification.
        /// </summary>
        /// <param name="specification"></param>
        /// <typeparam name="TItem"></typeparam>
        /// <returns></returns>
        IReadOnlyList<TItem> GetAll<TItem>(IQuerySpecification<TEntity> specification) 
            where TItem : class;
        
        /// <summary>
        /// Get readonly collection of entities selected by the mapped query specification.
        /// </summary>
        /// <param name="specification"></param>
        /// <typeparam name="TItem"></typeparam>
        /// <returns></returns>
        IReadOnlyList<TItem> GetAll<TItem>(IMappedQuerySpecification<TEntity, TItem> specification) 
            where TItem : class;
    }
}