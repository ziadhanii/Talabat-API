using System.Collections.Concurrent;
using Talabat.Core;

namespace Talabat.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ConcurrentDictionary<string, object> _repositories;
    private readonly StoreContext _context;

    public UnitOfWork(StoreContext context)
    {
        _context = context;
        _repositories = new ConcurrentDictionary<string, object>();
    }

    public IGenericRepository<T> Repository<T>() where T : BaseEntity
    {
        var key = typeof(T).Name;

        return (IGenericRepository<T>)_repositories.GetOrAdd(key, n => new GenericRepository<T>(_context));
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }
}