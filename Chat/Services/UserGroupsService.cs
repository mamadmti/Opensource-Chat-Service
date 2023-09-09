using Chat.Domain.Contracts.Repositories;
using Chat.Domain.Contracts.Services;
using Chat.Domain.Entities;
using Chat.Repositories;

namespace Chat.Services
{
    public class UserGroupsService: IUserGroups
    {
        private IRepositoryFactory _repository;
        public UserGroupsService(IRepositoryFactory repository, Guid? apikey)
        {
            _repository = repository;
            _repository.Repository.ApiKey = apikey;
        }
        public async Task<UserGroups> Create(UserGroups item)
        {
            return await _repository.Repository.Create<UserGroups>(item);
        }

        public async Task CreateRange(ICollection<UserGroups> entity)
        {
            await _repository.Repository.CreateRange<UserGroups>(entity);
        }


        public async Task<IEnumerable<UserGroups>> ReadAll( ISpecification<UserGroups> specification = null, int? skip = null, int? take = null)
        {
            return await _repository.Repository.ReadAll<UserGroups>(specification,skip,take);
        }

   

        public async Task<UserGroups> ReadById(long id)
        {
            return await _repository.Repository.ReadById<UserGroups>(id);
        }

        public async Task Update(UserGroups entity)
        {
            await _repository.Repository.Update(entity);
        }

        public async Task UpdateRange(ICollection<UserGroups> entity)
        {
            await _repository.Repository.UpdateRange(entity.ToList());
        }


        public async Task Remove(UserGroups item)
        {
            await _repository.Repository.Remove(item);
        }

        public async Task RemoveRange(IEnumerable<UserGroups> items)
        {
            await _repository.Repository.RemoveRange(items);
        }

    }
}
