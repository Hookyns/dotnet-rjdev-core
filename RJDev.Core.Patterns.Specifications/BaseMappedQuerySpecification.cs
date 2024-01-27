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
            get => selector ?? throw new InvalidOperationException($"Mapped query specification '{GetType().FullName}' does not initialize '{nameof(Selector)}'.");
            protected internal init => selector = value;
        }

        /// <inheritdoc />
        public Action<TTarget>? PostAction { get; protected internal init; }


        /// <inheritdoc />
        IMappedQuerySpecification<TEntity, TTarget> IMappedQuerySpecification<TEntity, TTarget>.And(ISpecification<TEntity> specification)
        {
            return new BaseMappedQuerySpecification<TEntity, TTarget>()
            {
                Criteria = ResolveCriteria(specification),
                OrderBy = OrderBy,
                Skip = Skip,
                Take = Take,
                Selector = Selector,
                PostAction = PostAction
            };
        }

        /// <inheritdoc />
        IMappedQuerySpecification<TEntity, TTarget> IMappedQuerySpecification<TEntity, TTarget>.And(IQuerySpecification<TEntity> specification)
        {
            return new BaseMappedQuerySpecification<TEntity, TTarget>()
            {
                Criteria = ResolveCriteria(specification),
                OrderBy = MergeOrders(OrderBy, specification.OrderBy),
                Skip = Skip ?? specification.Skip,
                Take = Take ?? specification.Take,
                Selector = Selector,
                PostAction = PostAction
            };
        }

        /// <inheritdoc />
        public override IMappedQuerySpecification<TEntity, TAnotherTarget> And<TAnotherTarget>(IMappedQuerySpecification<TEntity, TAnotherTarget> specification)
            where TAnotherTarget : class
        {
            AssertLimits(specification.Skip, specification.Take, specification.GetType());
            AssertTypes<TAnotherTarget>();

            return new BaseMappedQuerySpecification<TEntity, TAnotherTarget>
            {
                Criteria = ResolveCriteria(specification),
                OrderBy = MergeOrders(OrderBy, specification.OrderBy),
                Skip = specification.Skip ?? Skip,
                Take = specification.Take ?? Take,
                Selector = specification.Selector,
                PostAction = MergePostActions(specification.PostAction)
            };
        }

        /// <inheritdoc />
        public IMappedQuerySpecification<TEntity, TTarget> And(IMappedQuerySpecification<TEntity, TTarget> specification)
        {
            AssertLimits(specification.Skip, specification.Take, specification.GetType());

            return new BaseMappedQuerySpecification<TEntity, TTarget>()
            {
                Criteria = ResolveCriteria(specification),
                OrderBy = MergeOrders(OrderBy, specification.OrderBy),
                Skip = specification.Skip ?? Skip,
                Take = specification.Take ?? Take,
                Selector = specification.Selector,
                PostAction = specification.PostAction ?? PostAction
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
                return PostAction as Action<TAnotherTarget>;
            }

            if (PostAction == null)
            {
                return specificationPostAction;
            }

            return target =>
            {
                (PostAction as Action<TAnotherTarget>)?.Invoke(target);
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