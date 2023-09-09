using Chat.Domain.Contracts.Repositories;
using Chat.Domain.Entities;
using Chat.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Chat.Domain.Entities.Enums;
using SharedLibrary.Enums;

namespace Chat.Specifications
{
    public class ChatHistoriesToUserIdSpecifications : BaseSpecifcation<ChatHistory>
    {
        //To Get Both Direction Messages
        public ChatHistoriesToUserIdSpecifications(long ToUserId,Guid currentUserGuid , long currentUserId , Guid ToUserGuid)
        {
                                     // sent messages                                          //received messages
            Criteria = i => i.ToUserId == ToUserId && i.UserId == currentUserGuid || i.ToUserId == currentUserId && i.UserId == ToUserGuid ;
        }

	}

    public class ChatHistoriesToUserIdChatUnreadCountSpecifications : BaseSpecifcation<ChatHistory>
    {
	    //To Get Both Direction Messages
	    public ChatHistoriesToUserIdChatUnreadCountSpecifications(long ToUserId, Guid currentUserGuid, long currentUserId, Guid ToUserGuid)
	    {
            // sent messages                                          //received messages
            Criteria = i => i.ToUserId == ToUserId && i.UserId == currentUserGuid || i.ToUserId == currentUserId && i.UserId == ToUserGuid;
	    }

    }
	public class ChatHistoriesToGroupIdSpecifications : BaseSpecifcation<ChatHistory>
    {
        public ChatHistoriesToGroupIdSpecifications(long ToGroupId)
        {
            Criteria = i => i.ToGroupId == ToGroupId;
        }
    }

    public class ChatHistoriesSeenSpecifications : BaseSpecifcation<ChatHistory>
    {
	    public ChatHistoriesSeenSpecifications(long MessageId)
	    {
		    Criteria = i => i.Id == MessageId;
	    }
	    public ChatHistoriesSeenSpecifications(List<long> MessageIds)
	    {                                                           // just being able to see messages sent to current user
		    Criteria = i => MessageIds.Contains(i.Id);
	    }
		public ChatHistoriesSeenSpecifications(List<long> MessageIds ,long toUserId)
	    {                                                           // just being able to see messages sent to current user
		    Criteria = i => MessageIds.Contains(i.Id) && i.ToUserId == toUserId;
	    }
	   
	}



}
