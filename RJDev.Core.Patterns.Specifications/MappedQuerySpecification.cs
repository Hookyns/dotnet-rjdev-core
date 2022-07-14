namespace RJDev.Core.Patterns.Specifications
{
    public class MappedQuerySpecification<TEntity> where TEntity : class
    {
        public static readonly IMappedQuerySpecification<TEntity, TEntity> Empty = new EmptyMappedQuerySpecification<TEntity>();
    }
}