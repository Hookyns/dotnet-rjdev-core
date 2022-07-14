# RJDev.Core.Patterns.Specifications.EntityFramework

EntityFramework related extensions for the Specifications.

## IQueryable{TType}.ResolveSpecificationAsync()

```csharp
Task<IReadOnlyList<TTarget>> ResolveSpecificationAsync<TType, TTarget>(
	this IQueryable<TType> queryable,
	IMappedQuerySpecification<TType, TTarget>? specification,
	(SpecificationSortType sortType, Expression<Func<TType, object>> selector) defaultSort,
	CancellationToken cancellationToken = default
)
```
