namespace Talabat.Repository;

internal static class SpecifiactionsEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputquery, ISpecifications<T> spec)
    {
        var query = inputquery;

        if (spec.Criteria != null)
            query = query.Where(spec.Criteria);

        if (spec.OrderBy is not null)
            query = query.OrderBy(spec.OrderBy);

        else if (spec.OrderByDescending is not null)
            query = query.OrderByDescending(spec.OrderByDescending);
        
          
        if (spec.IsPagingEnabled)
            query = query.Skip(spec.Skip).Take(spec.Take);

        query = spec.Includes.Aggregate(query,
            (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

        return query;
    }
}