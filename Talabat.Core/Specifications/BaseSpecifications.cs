namespace Talabat.Core.Specifications;

public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
{
    public Expression<Func<T, bool>>? Criteria { get; } = null;
    public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

    public BaseSpecifications()
    {
        // Criteria = null;
    }


    public BaseSpecifications(Expression<Func<T, bool>>? criteriaExpression)
    {
        Criteria = criteriaExpression; // P => P.Id == 10 ; 
    }
}