using Chat.Domain.Contracts.Repositories;
using Chat.Domain.Contracts.Services;
using Chat.Domain.Entities;
using Chat.Repositories;

namespace Chat.Services
{
    public class UsersService:IUsers
    {
        private IRepositoryFactory _repository;
        public UsersService(IRepositoryFactory repository, Guid? apikey)
        {
            _repository = repository;
            _repository.Repository.ApiKey = apikey;
        }
        public async Task<Users> Create(Users item)
        {
            return await _repository.Repository.Create<Users>(item);
        }

        public async Task CreateRange(ICollection<Users> entity)
        {
            await _repository.Repository.CreateRange<Users>(entity);
        }


        public async Task<IEnumerable<Users>> ReadAll(ISpecification < Users> specification = null, int? skip = null, int? take = null)
        {
            return await _repository.Repository.ReadAll<Users>(specification, skip, take);
        }

    

        public async Task<Users> ReadById(long id)
        {
            return await _repository.Repository.ReadById<Users>(id);
        }

        public async Task Update(Users entity)
        {
            await _repository.Repository.Update(entity);
        }

        public async Task UpdateRange(ICollection<Users> entity)
        {
            await _repository.Repository.UpdateRange(entity.ToList());
        }


        public async Task Remove(Users item)
        {
            await _repository.Repository.Remove(item);
        }

        public async Task RemoveRange(IEnumerable<Users> items)
        {
            await _repository.Repository.RemoveRange(items);
        }
    }
}
