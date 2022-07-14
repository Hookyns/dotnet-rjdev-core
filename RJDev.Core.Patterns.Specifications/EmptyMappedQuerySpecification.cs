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
                Criteria = this.ResolveCriteria(specification),
                OrderBy = this.OrderBy,
                Skip = this.Skip,
                Take = this.Take,
                Selector = this.Selector,
                PostAction = this.PostAction
            };
        }

        public new IMappedQuerySpecification<TEntity, TEntity> And(IQuerySpecification<TEntity> specification)
        {
            return new BaseMappedQuerySpecification<TEntity, TEntity>()
            {
                Criteria = this.ResolveCriteria(specification),
                OrderBy = BaseQuerySpecification<TEntity>.MergeOrders(this.OrderBy, specification.OrderBy),
                Skip = this.Skip ?? specification.Skip,
                Take = this.Take ?? specification.Take,
                Selector = this.Selector,
                PostAction = this.PostAction
            };
        }

        public IMappedQuerySpecification<TEntity, TEntity> And(IMappedQuerySpecification<TEntity, TEntity> specification)
        {
            throw new NotImplementedException();
        }
    }
}