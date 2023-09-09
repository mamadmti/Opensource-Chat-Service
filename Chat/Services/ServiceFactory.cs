using Chat.Domain.Contracts.Services;
using Chat.Domain.Entities;
using Chat.Repositories;

namespace Chat.Services
{
    public interface IServiceFactory
    {
        Guid Apikey { get; set; }
        public ChatHistoryService ChatHistoryService { get;  }
        public GroupsService GroupsService { get;  }
        public UserGroupsService UserGroupsService { get;  }
        public UsersService UsersService { get; }
        public UsersConnectionsService UsersConnectionsService { get; }
        Task<int> SaveAsync();

    }

    public class ServiceFactory : IDisposable, IServiceFactory
    {
        private bool disposed = false;

        public Guid Apikey { get; set; }
        private readonly IRepositoryFactory _factory;
        public ServiceFactory(IRepositoryFactory repositoryFactory)
        {
            _factory = repositoryFactory;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    ((IDisposable)_factory).Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private ChatHistoryService _ChatHistoryService;
        public ChatHistoryService ChatHistoryService
        {
            get
            {
                return this._ChatHistoryService ??= new ChatHistoryService(_factory,this.Apikey);
            }
        }

        private GroupsService _GroupsService;
        public GroupsService GroupsService {
            get
            {
                return this._GroupsService ??= new GroupsService(_factory,this.Apikey);
            }
        }

        private UserGroupsService _UserGroupsService;
        public UserGroupsService UserGroupsService {
            get
            {
                return this._UserGroupsService ??= new UserGroupsService(_factory, this.Apikey);
            }
        }

        private UsersService _UsersService;
        public UsersService UsersService
        {
            get
            {
                return this._UsersService ??= new UsersService(_factory, this.Apikey);
            }
        }

        private UsersConnectionsService _UsersConnectionsService;
        public UsersConnectionsService UsersConnectionsService
        {
            get
            {
                return this._UsersConnectionsService ??= new UsersConnectionsService(_factory, this.Apikey);
            }
        }




        public async Task<int> SaveAsync()
        {
            try
            {
                return await _factory.SaveAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}
