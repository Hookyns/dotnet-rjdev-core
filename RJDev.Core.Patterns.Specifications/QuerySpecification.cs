namespace RJDev.Core.Patterns.Specifications
{
    public class QuerySpecification<TEntity> where TEntity : class
    {
        public static readonly IQuerySpecification<TEntity> Empty = new EmptyQuerySpecification<TEntity>();
    }
}