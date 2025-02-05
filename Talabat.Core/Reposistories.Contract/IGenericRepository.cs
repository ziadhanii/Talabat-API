using Talabat.Core.Specifications;

namespace Talabat.Core.Reposistories.Contract;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
    Task<T?> GetEntityByIdWithSpecAsync(ISpecifications<T> spec);
    Task<int> GetCountAsync(ISpecifications<T> spec);

    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}