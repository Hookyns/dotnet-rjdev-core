using System;
using System.Linq.Expressions;

namespace RJDev.Core.Patterns.Specifications
{
    /// <summary>
    /// Empty specification with <code>() => true</code> criteria.
    /// Empty specification is excluded from query after adding other specification.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EmptyMappedQuerySpecification<TEntity> : EmptyQuerySpecification<TEntity>, IMappedQuerySpecification<TEntity, TEntity>
        where TEntity : class
    {
        /// <inheritdoc />
        public Expression<Func<TEntity, TEntity>> Selector => x => x;

        /// <inheritdoc />
        public Action<TEntity>? PostAction => null;

        public new IMappedQuerySpecification<TEntity, TEntity> And(ISpecification<TEntity> specification)
        {
            return new BaseMappedQuerySpecification<TEntity, TEntity>()
            {
                Criteria = ResolveCriteria(specification),
                OrderBy = OrderBy,
                Skip = Skip,
                Take = Take,
                Selector = Selector,
                PostAction = PostAction
            };
        }

        public new IMappedQuerySpecification<TEntity, TEntity> And(IQuerySpecification<TEntity> specification)
        {
            return new BaseMappedQuerySpecification<TEntity, TEntity>()
            {
                Criteria = ResolveCriteria(specification),
                OrderBy = BaseQuerySpecification<TEntity>.MergeOrders(OrderBy, specification.OrderBy),
                Skip = Skip ?? specification.Skip,
                Take = Take ?? specification.Take,
                Selector = Selector,
                PostAction = PostAction
            };
        }

        public IMappedQuerySpecification<TEntity, TEntity> And(IMappedQuerySpecification<TEntity, TEntity> specification)
        {
            throw new NotImplementedException();
        }
    }
}