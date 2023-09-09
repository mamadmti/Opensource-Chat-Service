using Chat.Domain.Contracts.Repositories;
using Chat.Domain.Entities;
using Chat.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chat.Specifications
{
    public class UserGroupsSpecifications : BaseSpecifcation<UserGroups>
    {
        public UserGroupsSpecifications(long GroupId)
        {
            Criteria = i => i.GroupsId == GroupId;

        }
    }

    public class UserGroupsforuseridSpecifications : BaseSpecifcation<UserGroups>
    {
        public UserGroupsforuseridSpecifications(long UsersId)
        {
            Criteria = i => i.UsersId == UsersId;

        }
    }
}
