namespace RJDev.Core.Patterns.Specifications
{
    public static class Specification
    {
        /// <summary>
        /// Create new base specification for chaining.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static ISpecification<TEntity> Chain<TEntity>() where TEntity : class
        {
            return new EmptySpecification<TEntity>();
        }

        /// <summary>
        /// Create specification builder for building specifications.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static SpecificationBuilder<TEntity> Build<TEntity>() where TEntity : class
        {
            return new SpecificationBuilder<TEntity>();
        }

        /// <summary>
        /// Create NONE specification selecting NO items.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static ISpecification<TEntity> None<TEntity>() where TEntity : class
        {
            return new NoneSpecification<TEntity>();
        }
    }

    public static class Specification<TType> where TType : class
    {
        public static readonly ISpecification<TType> Empty = new EmptySpecification<TType>();
    }
}