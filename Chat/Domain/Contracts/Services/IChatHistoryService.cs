using Chat.Domain.Contracts.Repositories;
using Chat.Domain.Entities;

namespace Chat.Domain.Contracts.Services
{
    public interface IChatHistoryService : IService<ChatHistory,long>
    {
	    Task<IEnumerable<long?>> GetUniqueIds();

		Task<long> GetCount<T>(ISpecification<T> specification = null) where T : BaseEntity;

	}
}
