using Chat.Domain.Entities;

namespace Chat.Domain.Contracts.Repositories
{
    public interface IRepository
    {
        public Guid? ApiKey { get; set; }
        Task<T> Create<T> (T entity) where T : BaseEntity ;
        Task CreateRange<T>(ICollection<T> entity) where T : BaseEntity;
        Task<ICollection<T>> ReadAll<T>(ISpecification<T> specification = null, int? skip = null, int? take = null) where T : BaseEntity;
        Task<T> ReadById<T>(long id) where T : BaseEntity;
        Task Update<T>(T entity) where T : BaseEntity;
        Task UpdateRange<T>(ICollection<T> entity) where T : BaseEntity;
        Task Remove<T>(T entity) where T : BaseEntity;
        Task RemoveRange<T>(IEnumerable<T> entity) where T : BaseEntity;
        Task SaveChange();

        Task<IEnumerable<long?>> GetUniqueIds();
        Task<long> GetCount <T>(ISpecification<T> specification = null) where T : BaseEntity;
		//GetChatCount<T>(ISpecification<T> specification = null)
	}
}
