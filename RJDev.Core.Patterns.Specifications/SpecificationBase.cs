using System;
using System.Linq.Expressions;

namespace RJDev.Core.Patterns.Specifications
{
    public abstract class SpecificationBase<TEntity> : ISpecification<TEntity>
        where TEntity : class
    {
        private Expression<Func<TEntity, bool>>? _criteria;

        /// <inheritdoc />
        public Expression<Func<TEntity, bool>>? Criteria
        {
            get => _criteria;
            protected internal init => _criteria = value;
        }

        /// <summary>
        /// Set <see cref="Criteria"/>.
        /// </summary>
        /// <param name="criteria"></param>
        protected void SetCriteria(Expression<Func<TEntity, bool>> criteria)
        {
            _criteria = criteria;
        }

        /// <inheritdoc />
        public abstract ISpecification<TEntity> And(ISpecification<TEntity> specification);

        /// <inheritdoc />
        public abstract IQuerySpecification<TEntity> And(IQuerySpecification<TEntity> specification);

        /// <inheritdoc />
        public abstract IMappedQuerySpecification<TEntity, TTarget> And<TTarget>(IMappedQuerySpecification<TEntity, TTarget> specification) where TTarget : class;

        /// <inheritdoc />
        ISpecification<TEntity> ISpecification<TEntity>.AndIf(ISpecification<TEntity> specification, bool predicate)
        {
            return predicate ? And(specification) : this;
        }

        /// <inheritdoc />
        ISpecification<TEntity> ISpecification<TEntity>.AndIfNotEmpty(ISpecification<TEntity> specification, string? text)
        {
            return string.IsNullOrWhiteSpace(text) ? this : And(specification);
        }

        /// <inheritdoc />
        public IQuerySpecification<TEntity> ToQuerySpecification(Action<BaseQuerySpecificationBuilder<TEntity>> configurator)
        {
            BaseQuerySpecificationBuilder<TEntity> builder = new();
            configurator.Invoke(builder);
            return And(builder.Build());
        }

        /// <inheritdoc />
        public virtual IMappedQuerySpecification<TEntity, TEntity> ToMappedQuerySpecification()
        {
            return new BaseMappedQuerySpecification<TEntity, TEntity>
            {
                Criteria = Criteria,
                Selector = entity => entity
            };
        }
    }
}