using Chat.Domain.Contracts.Repositories;
using Chat.Domain.Contracts.Services;
using Chat.Domain.Entities;
using Chat.Repositories;

namespace Chat.Services
{
    public class GroupsService: IGroups
    {
        private IRepositoryFactory _repository;
        public GroupsService(IRepositoryFactory repository,Guid? apikey)
        {
            _repository = repository;
            _repository.Repository.ApiKey = apikey;
        }
        public async Task<Groups> Create(Groups item)
        {
            return await _repository.Repository.Create<Groups>(item);
        }

        public async Task CreateRange(ICollection<Groups> entity)
        {
            await _repository.Repository.CreateRange<Groups>(entity);
        }


        public async Task<IEnumerable<Groups>> ReadAll( ISpecification<Groups> specification = null, int? skip = null, int? take = null)
        {
            return await _repository.Repository.ReadAll<Groups>(specification, skip, take);

        }

 

        public async Task<Groups> ReadById(long id)
        {
            return await _repository.Repository.ReadById<Groups>(id);
        }

        public async Task Update(Groups entity)
        {
            await _repository.Repository.Update(entity);
        }

        public async Task UpdateRange(ICollection<Groups> entity)
        {
            await _repository.Repository.UpdateRange(entity.ToList());
        }


        public async Task Remove(Groups item)
        {
            await _repository.Repository.Remove(item);
        }

        public async Task RemoveRange(IEnumerable<Groups> items)
        {
            await _repository.Repository.RemoveRange(items);
        }
    }
}
