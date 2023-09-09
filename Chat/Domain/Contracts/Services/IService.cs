using Chat.Domain.Contracts.Repositories;

namespace Chat.Domain.Contracts.Services
{
    public interface IService<T,Id>
    {
        
        Task<T> Create(T item);
        Task CreateRange(ICollection<T> entity);
        Task<IEnumerable<T>> ReadAll( ISpecification<T> specification = null, int? skip = null, int? take = null);
        Task<T> ReadById(long id);
        Task Update(T entity);
        Task UpdateRange(ICollection<T> entity);
        Task Remove(T item);
        Task RemoveRange(IEnumerable<T> items);


    }
}
