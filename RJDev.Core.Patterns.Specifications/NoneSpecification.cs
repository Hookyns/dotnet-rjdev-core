namespace RJDev.Core.Patterns.Specifications
{
    /// <summary>
    /// NONE specification containing <code>() => false</code> condition.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class NoneSpecification<TEntity> : BaseSpecification<TEntity> where TEntity : class
    {
        public NoneSpecification()
        {
            Criteria = x => false;
        }
    }
}