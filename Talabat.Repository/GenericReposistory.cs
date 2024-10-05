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
}