using Talabat.Core.Specifications;

namespace Talabat.Core.Reposistories.Contract;

public interface IGenericReposistory<T> where T : BaseEntity
{
    Task<T?> GetAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
    Task<T?> GetEntityWithSpecAsync(ISpecifications<T> spec);
}