using Chat.Domain.Contracts.Repositories;
using Chat.Domain.Contracts.Services;
using Chat.Domain.Entities;
using Chat.Repositories;

namespace Chat.Services
{
    public class UsersConnectionsService : IUsersConnectionsService
    {
        private IRepositoryFactory _repository;
        public UsersConnectionsService(IRepositoryFactory repository, Guid? apikey)
        {
            _repository = repository;
            _repository.Repository.ApiKey = apikey;
        }

        public async Task<UsersConnections> Create(UsersConnections item)
        {
            return await _repository.Repository.Create<UsersConnections>(item);
        }

        public  async Task CreateRange(ICollection<UsersConnections> entity)
        {
            await _repository.Repository.CreateRange<UsersConnections>(entity);
        }

        public   async Task<IEnumerable<UsersConnections>> ReadAll( ISpecification<UsersConnections> specification = null, int? skip = null, int? take = null)
        {
            return await _repository.Repository.ReadAll<UsersConnections>(specification, skip, take);

        }

       

        public   async Task<UsersConnections> ReadById(long id)
        {
            return await _repository.Repository.ReadById<UsersConnections>(id);
        }

        public   async Task Update(UsersConnections entity)
        {
            await _repository.Repository.Update(entity);
        }

        public   async Task UpdateRange(ICollection<UsersConnections> entity)
        {
            await _repository.Repository.UpdateRange(entity.ToList());
        }

        public   async Task Remove(UsersConnections item)
        {
            await _repository.Repository.Remove(item);
        }

        public   async Task RemoveRange(IEnumerable<UsersConnections> items)
        {
            await _repository.Repository.RemoveRange(items);
        }
    }
}
