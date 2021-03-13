namespace RJDev.Core.Patterns.Specifications
{
    /// <summary>
    /// Empty specification with <code>() => true</code> criteria.
    /// Empty specification is excluded from query after adding other specification.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EmptySpecification<TEntity> : BaseSpecification<TEntity> where TEntity : class
    {
        public EmptySpecification()
        {
            this.Criteria = x => true;
        }

        /// <inheritdoc />
        public override ISpecification<TEntity> And(ISpecification<TEntity> specification)
        {
            return specification;
        }

        /// <inheritdoc />
        public override IQuerySpecification<TEntity> And(IQuerySpecification<TEntity> specification)
        {
            return specification;
        }
    }
}