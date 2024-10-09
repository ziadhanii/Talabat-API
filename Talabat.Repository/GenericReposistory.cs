using Talabat.Core.Specifications;

namespace Talabat.Repository;

public class GenericReposistory<T> : IGenericReposistory<T> where T : BaseEntity
{
    private readonly StoreContext _context;

    public GenericReposistory(StoreContext context)
    {
        _context = context;
    }

    public async Task<T?> GetAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<T?> GetEntityWithSpecAsync(ISpecifications<T> spec)
    {
        return await ApplySpecification(spec).SingleOrDefaultAsync();
    }


    private IQueryable<T> ApplySpecification(ISpecifications<T> spec)

    {
        return SpecifiactionsEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
    }
}