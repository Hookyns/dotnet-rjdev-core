# RJDev.Core.Patterns.Specifications
Implementation of the Specification pattern.

## ISpecification{TEntity}
```csharp
interface ISpecification<TEntity> where TEntity : class
{
    /// <summary>
    /// Select criterium/condition.
    /// </summary>
    Expression<Func<TEntity, bool>>? Criteria { get; }

    /// <summary>
    /// Merge specifications by logical AND operator.
    /// </summary>
    /// <param name="specification"></param>
    /// <returns></returns>
    ISpecification<TEntity> And(ISpecification<TEntity> specification);

    /// <summary>
    /// Merge specifications by logical AND operator.
    /// </summary>
    /// <param name="specification"></param>
    /// <returns></returns>
    IQuerySpecification<TEntity> And(IQuerySpecification<TEntity> specification);

    /// <summary>
    /// Merge specifications by logical AND operator.
    /// </summary>
    /// <param name="specification"></param>
    /// <returns></returns>
    IMappedQuerySpecification<TEntity, TTarget> And<TTarget>(IMappedQuerySpecification<TEntity, TTarget> specification) where TTarget : class;

    /// <summary>
    /// Merge specifications by logical AND operator, if <see cref="predicate"/> is true.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    ISpecification<TEntity> AndIf(ISpecification<TEntity> specification, bool predicate);

    /// <summary>
    /// Merge specifications by logical AND operator, if <see cref="value"/> is not null or empty.
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    ISpecification<TEntity> AndIfNotEmpty(ISpecification<TEntity> specification, string? value);

    /// <summary>
    /// Convert <see cref="ISpecification{TEntity}"/> to <see cref="IQuerySpecification{TEntity}"/>.
    /// </summary>
    /// <param name="configurator"></param>
    /// <returns></returns>
    IQuerySpecification<TEntity> ToQuerySpecification(Action<BaseQuerySpecificationBuilder<TEntity>> configurator);

    /// <summary>
    /// Convert <see cref="ISpecification{TEntity}"/> to <see cref="IMappedQuerySpecification{TEntity,TTarget}"/>.
    /// </summary>
    /// <returns></returns>
    IMappedQuerySpecification<TEntity, TEntity> ToMappedQuerySpecification();
}
```

## IQuerySpecification{TEntity}
```csharp
interface IQuerySpecification<TEntity> : ISpecification<TEntity> where TEntity : class
{
    /// <summary>
    /// Collection with order by expressions.
    /// </summary>
    IList<(SpecificationSortType sortType, Expression<Func<TEntity, object>> selector)> OrderBy { get; }

    /// <summary>
    /// The number of items to skip.
    /// </summary>
    int? Skip { get; }

    /// <summary>
    /// The number of items to select.
    /// </summary>
    int? Take { get; }

    /// <summary>
    /// Merge specifications by logical AND operator.
    /// </summary>
    /// <param name="specification"></param>
    /// <returns></returns>
    new IQuerySpecification<TEntity> And(ISpecification<TEntity> specification);

    /// <summary>
    /// Merge specifications by logical AND operator.
    /// </summary>
    /// <param name="specification"></param>
    /// <returns></returns>
    new IQuerySpecification<TEntity> And(IQuerySpecification<TEntity> specification);

    /// <summary>
    /// Convert <see cref="IQuerySpecification{TEntity}"/> to <see cref="IMappedQuerySpecification{TEntity,TTarget}"/>.
    /// </summary>
    /// <param name="selector"></param>
    /// <param name="postAction"></param>
    /// <returns></returns>
    IMappedQuerySpecification<TEntity, TTarget> ToMappedQuerySpecification<TTarget>(Expression<Func<TEntity, TTarget>> selector, Action<TTarget>? postAction = null)
        where TTarget : class;
}
```

## IMappedQuerySpecification{TEntity, TTarget}
```csharp
interface IMappedQuerySpecification<TEntity, TTarget> : IQuerySpecification<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Selector for custom specific dataset.
    /// </summary>
    Expression<Func<TEntity, TTarget>> Selector { get; }

    /// <summary>
    /// Optional action executed on each item after selection.
    /// </summary>
    Action<TTarget>? PostAction { get; }

    /// <summary>
    /// Merge specifications by logical AND operator.
    /// </summary>
    /// <param name="specification"></param>
    /// <returns></returns>
    new IMappedQuerySpecification<TEntity, TTarget> And(ISpecification<TEntity> specification);

    /// <summary>
    /// Merge specifications by logical AND operator.
    /// </summary>
    /// <param name="specification"></param>
    /// <returns></returns>
    new IMappedQuerySpecification<TEntity, TTarget> And(IQuerySpecification<TEntity> specification);

    /// <summary>
    /// Merge specifications by logical AND operator.
    /// </summary>
    /// <param name="specification"></param>
    /// <returns></returns>
    IMappedQuerySpecification<TEntity, TTarget> And(IMappedQuerySpecification<TEntity, TTarget> specification);
}
```