using System.Diagnostics;
using Chat.Domain.Contexts;
using Chat.Helper;
using Chat.Services;
using Microsoft.AspNetCore.SignalR;
using Chat.Specifications;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNet.SignalR;
using Hub = Microsoft.AspNetCore.SignalR.Hub;
using Chat.Domain.Entities;
using Chat.Domain.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.AspNet.SignalR.Messaging;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using SharedLibrary.Enums;
using SharedLibrary.Helpers;

namespace Chat.Hubs
{

    
    public class ChatHub : Hub
    {
        readonly IServiceFactory _services;
        public IConfiguration _configuration;
        private readonly ChatDbContext _context;
        public ChatHub(IServiceFactory service, IConfiguration config, ChatDbContext context, IOptions<appSettingClass> options)
        {
            _services = service;
            _configuration = config;
            _context = context;
            _formatSettings = options.Value;
		}
        private readonly appSettingClass _formatSettings;
		//public ChatHub()
		//{
		//}

		[Microsoft.AspNetCore.Authorization.Authorize]
        [Auth]


        /// <summary>
        /// Everytime any device connects
        /// </summary>
        /// <returns></returns>

        public override async Task OnConnectedAsync()
        {
            if (!Context.User.Claims.Any())
            {
                return;
            }
            var Oid = Context.User.Identities.FirstOrDefault().Claims.ToList();
            long UserId = long.Parse( Oid.Find(a => a.Type == "UserId").Value);
            string connectionid = Context.ConnectionId;

            //var res = await _services.UsersConnectionsService.ReadAll(new UsersConnectionsSpecifications(UserId));

            UsersConnections uc = new UsersConnections();

            uc.UsersId = UserId;
            uc.UserConnectionId = connectionid;
            uc.ApiKey = _formatSettings.apiKey;
            uc.CreateAt = DateTime.Now;

            // now go save it on db
            await _services.UsersConnectionsService.Create(uc);


            await _services.SaveAsync();

        }

        /// <summary>
        /// Everytime any device disconnect
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (!Context.User.Claims.Any())
            {
                return;
            }
            var Oid = Context.User.Identities.FirstOrDefault().Claims.ToList();
            long UserId = long.Parse(Oid.Find(a => a.Type == "UserId").Value);
            string connectionid = Context.ConnectionId;
            //remove the connection from db
            var res = await _services.UsersConnectionsService.ReadAll(new UsersConnectionsSpecifications(connectionid));

            await _services.UsersConnectionsService.Remove(res.FirstOrDefault()!);
            await _services.SaveAsync();

        }

        /// <summary>
        /// Send UserName For Private Message / Send GroupId For Group Message
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="receiverUserName"></param>
        /// <returns></returns>
        public async Task SendMessage (string Message, string? receiverUserName,string? GroupId)
        {
            if (!Context.User.Claims.Any())
            {
                //In Case Of No Auth
                return;
            }

            if (receiverUserName != null && GroupId != null)
            {
                //Both PTP Messaging And Group Messaging Can Not Have Value At The Same Time
                return;
            }

            if (receiverUserName != null)
            {

                //go get the userid
                var Users = await _services.UsersService.ReadAll(new UsersSpecifications(receiverUserName));
                long UserId = Users.FirstOrDefault().Id;

                //Get CurrentUser Userid & username
                var Oid = Context.User.Identities.FirstOrDefault().Claims.ToList();
                long SenderUserId = long.Parse(Oid.Find(a => a.Type == "UserId").Value);
                string SenderUsername = Oid.Find(a => a.Type == "UserName").Value;

                //Send the Message To All Those Connections Of That User
                var UsersConnections = await _services.UsersConnectionsService.ReadAll(new UsersConnectionsSpecifications(UserId,SenderUserId));
                List<string> str = new List<string>();
                foreach (var VARIABLE in UsersConnections)
                {
                    str.Add(VARIABLE.UserConnectionId);
                }

                //Save The Message In Db
                var ch = await SaveMessages(UserId, Message,null);

				messagehistory mh = new messagehistory();

				mh.Id = ch.Id;
				mh.CreateAt = ch.CreateAt;
				mh.ShortCreateAt = ch.CreateAt.ToShortTimeString();
				//mh.ShortCreateAt =  ch.CreateAt.ToShortDateString() +" " + mh.ShortCreateAt;

				mh.ToUserId = ch.ToUserId;
				mh.Message = ch.Message;
				mh.RecordStatus = ch.RecordStatus;
				mh.UserId = ch.UserId;

				//send to all of those Sessions //                 //Sender Username
				await Clients.Clients(str).SendAsync("ShowMessage", SenderUsername , Message,null,mh);

            }
            else if (GroupId != null) 
            {
                //GEt All Users In That Group
                var AllUsersInGroup = await _services.UserGroupsService.ReadAll( new UserGroupsSpecifications( long.Parse(GroupId) ));
                
                List<long> userIdsInGroup = new List<long>();
                foreach (var user in AllUsersInGroup)
                {
                    userIdsInGroup.Add(user.UsersId);
                }
                //Get All Online Sessions In That Group

                //Get CurrentUser Userid & username
                var Oid = Context.User.Identities.FirstOrDefault().Claims.ToList();
                long SenderUserId = long.Parse(Oid.Find(a => a.Type == "UserId").Value);
                string SenderUsername = Oid.Find(a => a.Type == "UserName").Value;


                var AllUsersConnectionInGroup = await _services.UsersConnectionsService.ReadAll(new UsersConnectionsSpecifications(userIdsInGroup));
                List<string> connectionIdsToSend = new List<string>();
                foreach (var v in AllUsersConnectionInGroup)
                {
                    connectionIdsToSend.Add(v.UserConnectionId);
                }
				//Send That Message For All
				var ch = await SaveMessages(null, Message, long.Parse(GroupId));


				messagehistory mh = new messagehistory();

				mh.Id = ch.Id;
				mh.CreateAt = ch.CreateAt;
				mh.ShortCreateAt = ch.CreateAt.ToShortTimeString();
				//mh.ShortCreateAt =  ch.CreateAt.ToShortDateString() +" " + mh.ShortCreateAt;

				mh.ToGroupId = ch.ToGroupId;
				mh.Message = ch.Message;
				mh.RecordStatus = ch.RecordStatus;
				mh.UserId = ch.UserId;

				await Clients.Clients(connectionIdsToSend).SendAsync("ShowMessage", SenderUsername, Message, GroupId,mh);


            }

        }


        private async Task<ChatHistory> SaveMessages(long? ToUserid, string Message, long? ToGroupId)
        {
            var Oid = Context.User.Identities.FirstOrDefault().Claims.ToList();
            Guid Userguid = Guid.Parse(Oid.Find(a => a.Type == "UserGuid").Value);
            Guid apikey = _formatSettings.apiKey;


            ChatHistory ch = new ChatHistory();
            ch.Message = Message;
			if (ToGroupId != null)
            {
                // in case of groupid
	            ch.ToGroupId = ToGroupId;
            }
			if(ToUserid != null)
			{
				// in case of ptp
				ch.ToUserId = ToUserid;
			}
			
			if(ToUserid == null && ToGroupId == null)
			{
				throw new Exception("User sent no group or user id");
			}
			ch.CreateAt =DateTime.Now;
            ch.UserId = Userguid;
            ch.ApiKey = apikey;
            var Users = await _services.ChatHistoryService.Create(ch);
            await _services.SaveAsync();
            return ch;
        }





        public async Task CreateGroup(string? GroupName,string? tmp)
        {
            //await _services.UsersService.ReadAll(new UsersSpecifications(UserId));

            //await _services.UsersConnectionsService.Create();
            //await Clients.All.SendAsync("ShowMessage");
        }

        public async Task SendGroupMessage(string RoleName, string ApiKey,string UserName,string UserId,string Message)
        {
            //await Clients.Groups(ApiKey).SendAsync("ShowMessage", UserName,RoleName, Message,UserId);
        }
        public async Task IsTyping(string RoleName, string ApiKey, string UserName, string UserId)
        {
            await Clients.Groups(ApiKey).SendAsync("ShowIsTyping", UserName, RoleName, UserId);
        }


        //all users can make any message that was sent to them
        public async Task MessageSeen(long[] messageIds )
        {
	        var Oid = Context.User.Identities.FirstOrDefault().Claims.ToList();
	        Guid Userguid = Guid.Parse(Oid.Find(a => a.Type == "UserGuid").Value);
	        Guid apikey = _formatSettings.apiKey;
	        long CurrentUserUserId = long.Parse(Oid.Find(a => a.Type == "UserId").Value);



            /////////////////////////// for ptp chats only
            
			//get all those messages that opposite user have seen
			IEnumerable<ChatHistory> messages = await _services.ChatHistoryService.ReadAll(
	            new ChatHistoriesSeenSpecifications(messageIds.ToList(), CurrentUserUserId ));
			
			List<long> userIds = new List<long>() ;
			List<Guid> userGuids = new List<Guid>();

            foreach (var message in messages)
            {
	            message.RecordStatus =  RecordStatus.InActive;
	            if (message.ToUserId != null)
	            {
		            //userIds.Add(message.ToUserId ?? new long());
	            }
	            userGuids.Add(message.UserId);
            }
            await _services.ChatHistoryService.UpdateRange(messages.ToList());
            await _services.SaveAsync();

			//now that you have saved that message is seen
			//lets inform that user that message was seen
			//it can be just an update signal to refresh the chatlist
			// or you can send the exact messages to the other user to update it on its ui

			//notify sender and receiver of that messages by sending the message to them

			//allUsersWhichNeedsThisUpdate
			var au = await _services.UsersService.ReadAll(new UsersSpecifications(userGuids));
			
			foreach (var u in au)
			{
				userIds.Add(u.Id);
			}

			// now go get connection of sender users

			var c = await _services.UsersConnectionsService.ReadAll(new UsersConnectionsSpecifications(userIds));
            List<string> conncetionList = new List<string>();
            foreach (var connection in c)
            {
	            conncetionList.Add(connection.UserConnectionId);
            }

            
            List<messagehistory> mhlist = new List<messagehistory>();

            foreach (var ch in messages)
            {
	            messagehistory mh = new messagehistory();
				mh.Id = ch.Id;
	            mh.CreateAt = ch.CreateAt;
	            mh.ShortCreateAt = ch.CreateAt.ToShortTimeString();
	            //mh.ShortCreateAt =  ch.CreateAt.ToShortDateString() +" " + mh.ShortCreateAt;

	            mh.ToUserId = ch.ToUserId;
	            mh.Message = ch.Message;
	            mh.RecordStatus = ch.RecordStatus;
	            mh.UserId = ch.UserId;
	            mhlist.Add(mh);
			}
           

			await Clients.Clients(conncetionList).SendAsync("UpdateMessage", mhlist);


		}

	}
}
