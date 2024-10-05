namespace Talabat.Core.Reposistories.Contract;

public interface IGenericReposistory<T> where T : BaseEntity
{
    Task<T?> GetAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
}