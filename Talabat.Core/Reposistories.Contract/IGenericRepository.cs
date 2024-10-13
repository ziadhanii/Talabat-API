using Talabat.Core.Specifications;

namespace Talabat.Core.Reposistories.Contract;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
    Task<T?> GetEntityWithSpecAsync(ISpecifications<T> spec);
    Task<int> GetCountAsync(ISpecifications<T> spec);
}