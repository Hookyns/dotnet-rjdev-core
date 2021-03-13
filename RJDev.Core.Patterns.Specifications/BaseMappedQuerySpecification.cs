using System;
using System.Linq.Expressions;

namespace RJDev.Core.Patterns.Specifications
{
    public class BaseMappedQuerySpecification<TEntity, TTarget> : BaseQuerySpecification<TEntity>, IMappedQuerySpecification<TEntity, TTarget>
        where TEntity : class
    {
        private readonly Expression<Func<TEntity, TTarget>>? selector;

        /// <inheritdoc />
        public Expression<Func<TEntity, TTarget>> Selector
        {
            get => this.selector ?? throw new InvalidOperationException($"Mapped query specification '{this.GetType().FullName}' does not initialize '{nameof(this.Selector)}'.");
            protected internal init => this.selector = value;
        }

        /// <inheritdoc />
        public Action<TTarget>? PostAction { get; protected internal init; }


        /// <inheritdoc />
        IMappedQuerySpecification<TEntity, TTarget> IMappedQuerySpecification<TEntity, TTarget>.And(ISpecification<TEntity> specification)
        {
            return new BaseMappedQuerySpecification<TEntity, TTarget>()
            {
                Criteria = this.ResolveCriteria(specification),
                OrderBy = this.OrderBy,
                Skip = this.Skip,
                Take = this.Take,
                Selector = this.Selector,
                PostAction = this.PostAction
            };
        }

        /// <inheritdoc />
        IMappedQuerySpecification<TEntity, TTarget> IMappedQuerySpecification<TEntity, TTarget>.And(IQuerySpecification<TEntity> specification)
        {
            return new BaseMappedQuerySpecification<TEntity, TTarget>()
            {
                Criteria = this.ResolveCriteria(specification),
                OrderBy = MergeOrders(this.OrderBy, specification.OrderBy),
                Skip = this.Skip ?? specification.Skip,
                Take = this.Take ?? specification.Take,
                Selector = this.Selector,
                PostAction = this.PostAction
            };
        }

        /// <inheritdoc />
        public override IMappedQuerySpecification<TEntity, TAnotherTarget> And<TAnotherTarget>(IMappedQuerySpecification<TEntity, TAnotherTarget> specification)
            where TAnotherTarget : class
        {
            this.AssertLimits(specification.Skip, specification.Take, specification.GetType());
            this.AssertTypes<TAnotherTarget>();

            return new BaseMappedQuerySpecification<TEntity, TAnotherTarget>
            {
                Criteria = this.ResolveCriteria(specification),
                OrderBy = MergeOrders(this.OrderBy, specification.OrderBy),
                Skip = specification.Skip ?? this.Skip,
                Take = specification.Take ?? this.Take,
                Selector = specification.Selector,
                PostAction = this.MergePostActions(specification.PostAction)
            };
        }

        /// <inheritdoc />
        public IMappedQuerySpecification<TEntity, TTarget> And(IMappedQuerySpecification<TEntity, TTarget> specification)
        {
            this.AssertLimits(specification.Skip, specification.Take, specification.GetType());

            return new BaseMappedQuerySpecification<TEntity, TTarget>()
            {
                Criteria = this.ResolveCriteria(specification),
                OrderBy = MergeOrders(this.OrderBy, specification.OrderBy),
                Skip = specification.Skip ?? this.Skip,
                Take = specification.Take ?? this.Take,
                Selector = specification.Selector,
                PostAction = specification.PostAction ?? this.PostAction
            };
        }

        /// <summary>
        /// Merge <see cref="PostAction"/> with another post action.
        /// </summary>
        /// <param name="specificationPostAction"></param>
        /// <typeparam name="TAnotherTarget"></typeparam>
        /// <returns></returns>
        private Action<TAnotherTarget>? MergePostActions<TAnotherTarget>(Action<TAnotherTarget>? specificationPostAction) where TAnotherTarget : class
        {
            if (specificationPostAction == null)
            {
                return this.PostAction as Action<TAnotherTarget>;
            }

            if (this.PostAction == null)
            {
                return specificationPostAction;
            }

            return target =>
            {
                (this.PostAction as Action<TAnotherTarget>)?.Invoke(target);
                specificationPostAction(target);
            };
        }

        /// <summary>
        /// Asserts both target types are the same types.
        /// </summary>
        /// <typeparam name="TAnotherTarget"></typeparam>
        /// <exception cref="InvalidOperationException"></exception>
        private void AssertTypes<TAnotherTarget>()
        {
            Type thisType = typeof(TTarget);
            Type targetType = typeof(TAnotherTarget);

            if (thisType != targetType)
            {
                throw new InvalidOperationException("Unable to merge two specifications with target types." +
                    $"{Environment.NewLine}Left specification '{thisType.FullName}', right specification '{targetType.FullName}'.");
            }
        }
    }
}