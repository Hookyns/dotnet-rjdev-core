using System;
using System.Linq;
using System.Linq.Expressions;
using RJDev.Core.Expressions;

namespace RJDev.Core.Patterns.Specifications
{
    public class BaseSpecification<TEntity> : SpecificationBase<TEntity> where TEntity : class
    {
        /// <inheritdoc />
        public override ISpecification<TEntity> And(ISpecification<TEntity> specification)
        {
            Expression<Func<TEntity, bool>>? criteria = ResolveCriteria(specification);

            return new BaseSpecification<TEntity>()
            {
                Criteria = criteria
            };
        }

        /// <inheritdoc />
        public override IQuerySpecification<TEntity> And(IQuerySpecification<TEntity> specification)
        {
            Expression<Func<TEntity, bool>>? criteria = ResolveCriteria(specification);

            return new BaseQuerySpecification<TEntity>()
            {
                Criteria = criteria,
                OrderBy = specification.OrderBy.ToList(),
                Skip = specification.Skip,
                Take = specification.Take
            };
        }

        /// <inheritdoc />
        public override IMappedQuerySpecification<TEntity, TTarget> And<TTarget>(IMappedQuerySpecification<TEntity, TTarget> specification) where TTarget : class
        {
            Expression<Func<TEntity, bool>>? criteria = ResolveCriteria(specification);

            return new BaseMappedQuerySpecification<TEntity, TTarget>()
            {
                Criteria = criteria,
                OrderBy = specification.OrderBy.ToList(),
                Skip = specification.Skip,
                Take = specification.Take,
                Selector = specification.Selector,
                PostAction = specification.PostAction
            };
        }

        protected Expression<Func<TEntity, bool>>? ResolveCriteria(ISpecification<TEntity> specification)
        {
            Expression<Func<TEntity, bool>>? criteria = Criteria ?? specification.Criteria;

            if (Criteria != null && specification.Criteria != null)
            {
                criteria = new LogicalPredicateBuilder<TEntity>(Criteria).And(specification.Criteria).Build();
            }

            return criteria;
        }

        protected Expression<Func<TEntity, bool>>? ResolveCriteria(IQuerySpecification<TEntity> specification)
        {
            Expression<Func<TEntity, bool>>? criteria = Criteria ?? specification.Criteria;

            if (Criteria != null && specification.Criteria != null)
            {
                criteria = new LogicalPredicateBuilder<TEntity>(Criteria).And(specification.Criteria).Build();
            }

            return criteria;
        }
    }
}