using Chat.Domain.Contracts.Repositories;
using Chat.Domain.Contracts.Services;
using Chat.Domain.Entities;
using Chat.Repositories;

namespace Chat.Services
{
    public class ChatHistoryService:IChatHistoryService
    {
        private IRepositoryFactory _repository;
        public ChatHistoryService(IRepositoryFactory repository, Guid? apikey)
        {
            _repository = repository;
            _repository.Repository.ApiKey = apikey;
        }
        public async Task<ChatHistory> Create(ChatHistory item)
        {
            return await _repository.Repository.Create<ChatHistory>(item);
        }

        public async Task CreateRange(ICollection<ChatHistory> entity)
        {
            await _repository.Repository.CreateRange<ChatHistory>(entity);
        }


        public async Task<IEnumerable<ChatHistory>> ReadAll( ISpecification<ChatHistory> specification = null, int? skip = null, int? take = null)
        {
            return await _repository.Repository.ReadAll<ChatHistory>(specification, skip, take);

        }

        public async Task<ChatHistory> ReadById(long id)
        {
            return await _repository.Repository.ReadById<ChatHistory>(id);
        }

        public async Task Update(ChatHistory entity)
        {
            await _repository.Repository.Update(entity);
        }

        public async Task UpdateRange(ICollection<ChatHistory> entity)
        {
            await _repository.Repository.UpdateRange(entity.ToList());
        }


        public async Task Remove(ChatHistory item)
        {
            await _repository.Repository.Remove(item);
        }

        public async Task RemoveRange(IEnumerable<ChatHistory> items)
        {
            await _repository.Repository.RemoveRange(items);
        }

        public async Task<IEnumerable<long?>> GetUniqueIds()
        { 
	       return await _repository.Repository.GetUniqueIds();
		}

        public async Task<long> GetCount<T>(ISpecification<T> specification = null) where T : BaseEntity
        {
			return await _repository.Repository.GetCount(specification);
		}


    }
}
