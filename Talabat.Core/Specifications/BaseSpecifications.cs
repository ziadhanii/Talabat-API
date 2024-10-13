namespace Talabat.Core.Specifications;

public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
{
    public Expression<Func<T, bool>>? Criteria { get; }
    public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
    public Expression<Func<T, object>> OrderBy { get; private set; } = null;
    public Expression<Func<T, object>> OrderByDescending { get; private set; }
    public int Skip { get; set; }
    public int Take { get; set; }
    public bool IsPagingEnabled { get; set; }


    public BaseSpecifications()
    {
        // Criteria = null;
    }


    protected BaseSpecifications(Expression<Func<T, bool>>? criteriaExpression)
    {
        Criteria = criteriaExpression; // P => P.Id == 10 ; 
    }

    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }

    public void ApplyPagination(int skip, int take)
    {
        IsPagingEnabled = true;
        Skip = skip;
        Take = take;
    }
}