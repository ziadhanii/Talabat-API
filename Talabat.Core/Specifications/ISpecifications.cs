namespace Talabat.Core.Specifications;

public interface ISpecifications<T> where T : BaseEntity
{
    Expression<Func<T, bool>>? Criteria { get; }

    public List<Expression<Func<T, object>>> Includes { get; }
    Expression<Func<T, object>> OrderBy { get; }
    Expression<Func<T, object>> OrderByDescending { get; }

    public int Skip { set; get; }
    public int Take { set; get; }
    public bool IsPagingEnabled { set; get; }
}