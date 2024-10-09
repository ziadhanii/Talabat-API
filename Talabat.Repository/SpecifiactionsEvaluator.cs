using Talabat.Core.Specifications;

namespace Talabat.Repository;

internal static class SpecifiactionsEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputquery, ISpecifications<T> spec)
    {
        var query = inputquery;

        if (spec.Criteria != null)
            query = query.Where(spec.Criteria);

        query = spec.Includes.Aggregate(query,
            (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

        return query;
    }
}