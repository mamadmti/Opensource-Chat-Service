using Chat.Domain.Contracts.Repositories;
using Chat.Domain.Entities;
using Chat.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chat.Specifications
{
    public class UsersConnectionsSpecifications : BaseSpecifcation<UsersConnections>
    {
        public UsersConnectionsSpecifications(long UserId,long SenderUserId)
        {
            Criteria = i => i.UsersId == UserId || i.UsersId == SenderUserId;
        }
        public UsersConnectionsSpecifications(List<long> UserId)
        {
            Criteria = i => UserId.Contains(i.UsersId) ;
        }
        public UsersConnectionsSpecifications(string connectionId)
        {
            Criteria = i => i.UserConnectionId == connectionId;
        }
 
    }
}
