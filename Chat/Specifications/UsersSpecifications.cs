using Chat.Domain.Contracts.Repositories;
using Chat.Domain.Entities;
using Chat.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chat.Specifications
{
    public class UsersSpecifications : BaseSpecifcation<Users>
    {
  
        public UsersSpecifications(string UserName)
        {
            Criteria = i => i.UserName == UserName;
        }
        public UsersSpecifications(long UserId)
        {
            Criteria = i => i.Id == UserId;
        }
        public UsersSpecifications(List<long> Ids)
        {
            Criteria = i => Ids.Contains(i.Id);
        }
        public UsersSpecifications(List<string> UserNames)
        {
	        Criteria = i => UserNames.Contains(i.UserName);
        }
        public UsersSpecifications(List<Guid> guids )
        {
	        Criteria = i => guids.Contains(i.UserId);
        }
	}

    public class UsersEmailSpecifications : BaseSpecifcation<Users>
    {

        public UsersEmailSpecifications(string Email)
        {
            Criteria = x => x.Email == Email ;
        }
    }
}
