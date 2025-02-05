namespace Talabat.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly StoreContext _context;

    public GenericRepository(StoreContext context)
    {
        _context = context;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<T?> GetEntityByIdWithSpecAsync(ISpecifications<T> spec)
    {
        return await ApplySpecification(spec).SingleOrDefaultAsync();
    }

    public async Task<int> GetCountAsync(ISpecifications<T> spec)
    {
        return await ApplySpecification(spec).CountAsync();
    }

    public async Task AddAsync(T entity) => await _context.AddAsync(entity);

    public void Update(T entity) => _context.Update(entity);

    public void Delete(T entity) => _context.Remove(entity);

    private IQueryable<T> ApplySpecification(ISpecifications<T> spec)

    {
        return SpecifiactionsEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
    }
}