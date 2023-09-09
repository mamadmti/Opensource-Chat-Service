using Chat.Domain.Contexts;
using Chat.Domain.Contracts.Services;
using Chat.Domain.Entities;
using Chat.Helper;
using Chat.Repositories;
using Chat.Services;
using Chat.Specifications;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Chat.Domain.Entities.Enums;
using System;
using Microsoft.Extensions.Options;
using SharedLibrary.Enums;
using SharedLibrary.Helpers;

namespace Chat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatHistoryController : ControllerBase
    {
        readonly IServiceFactory _services;
        public IConfiguration _configuration;
        private readonly ChatDbContext _context;
        public ChatHistoryController(IServiceFactory service, IConfiguration config, ChatDbContext context, IOptions<appSettingClass> options)
        {
            _services = service;
            _configuration = config;
            _context = context;
            _formatSettings = options.Value;
		}
        private readonly appSettingClass _formatSettings;


        /// <summary>
        /// Get current user's chat history 
        /// </summary>
        /// <param name="gc"></param>
        /// <returns></returns>

        [Auth]
        [HttpPost("GetChatHistory")]
        public async Task<ActionResult> GetChatHistory(getchathistory gc)
        {
            //Check To Not Send Them Both
            if (gc.TogroupId != null && gc.ToUserId != null)
            {
                BadRequest("Cant Use Both UserId & GroupId At The Same Time");
            }

            if (gc.ToUserId != null && gc.TogroupId == null)
            {
                _services.Apikey = _formatSettings.apiKey;
                Guid Currentuserguid = Guid.Parse(HttpContext.Request.Headers["UserGuid"]);
                long Currentuserid = long.Parse(HttpContext.Request.Headers["UserId"]);

                var res = await _services.UsersService.ReadAll(new UsersSpecifications(gc.ToUserId??new long()));
                if (res == null)
                {
                    return BadRequest("destination user doesnt exist");
                }
                var Destuserinfo = res.FirstOrDefault();
                //Get Both History Of PTP Chat
                IEnumerable<ChatHistory> History = await _services.ChatHistoryService.ReadAll(new ChatHistoriesToUserIdSpecifications(Destuserinfo.Id, Currentuserguid, Currentuserid,Destuserinfo.UserId),gc.skip,gc.take);
                long count = await _services.ChatHistoryService.GetCount(new ChatHistoriesToUserIdChatUnreadCountSpecifications(Destuserinfo.Id, Currentuserguid, Currentuserid, Destuserinfo.UserId));
				
                List<messagehistory> mhl = new List<messagehistory>();
                foreach (var chatHistory in History)
                {
                    messagehistory mh = new messagehistory();
                    mh.Id = chatHistory.Id;
                    mh.Message = chatHistory.Message;
                    mh.UserId = chatHistory.UserId;
                    mh.CreateAt = chatHistory.CreateAt;
                    //mh.UnreadCount = count;
                    mh.TotalCount = count;

					mh.ShortCreateAt = chatHistory.CreateAt.ToShortTimeString();
                    //mh.ShortCreateAt = chatHistory.CreateAt.ToShortDateString() + " " + mh.ShortCreateAt;


					mh.ToGroupId = chatHistory.ToGroupId;
                    mh.ToUserId = chatHistory.ToUserId;
                    mh.RecordStatus = chatHistory.RecordStatus; 
                    mhl.Add(mh);
                }
                return Ok(mhl);

            }
            else if(gc.TogroupId != null)
            {
                //Get All History Of That Group
                var History = await _services.ChatHistoryService.ReadAll(new ChatHistoriesToGroupIdSpecifications(gc.TogroupId ?? new long()), gc.skip, gc.take);
				List<messagehistory> mhl = new List<messagehistory>();
				foreach (var chatHistory in History)
				{
					messagehistory mh = new messagehistory();
					mh.Id = chatHistory.Id;
					mh.Message = chatHistory.Message;
					mh.UserId = chatHistory.UserId;
					mh.CreateAt = chatHistory.CreateAt;

					mh.ShortCreateAt = chatHistory.CreateAt.ToShortTimeString();
					//mh.ShortCreateAt = chatHistory.CreateAt.ToShortDateString() + " " + mh.ShortCreateAt;


					mh.ToGroupId = chatHistory.ToGroupId;
					mh.ToUserId = chatHistory.ToUserId;
					mh.RecordStatus = chatHistory.RecordStatus;
					mhl.Add(mh);
				}
				return Ok(mhl);
			}

            return BadRequest("please Fill Any Of Required Fields");
        }



        /// <summary>
        /// users can send messages that they've seen with this api (to check the seen flag in db)
        /// </summary>
        /// <param name="messageIds"></param>
        /// <returns></returns>
        [Auth]
        [HttpPost("MessageSeen")]
        public async Task<ActionResult> MessageSeen(List<long> messageIds)
        {
            Guid Currentuserguid = Guid.Parse(HttpContext.Request.Headers["UserGuid"]);
            long Currentuserid = long.Parse(HttpContext.Request.Headers["UserId"]);

            var History = await _services.ChatHistoryService.ReadAll(new ChatHistoriesSeenSpecifications(messageIds, Currentuserid));

            foreach (var message in History)
            {
                message.RecordStatus = RecordStatus.InActive;
            }
            await _services.ChatHistoryService.UpdateRange(History.ToList());
            await _context.SaveChangesAsync();

            return Ok("messages read flag checked");
        }








    }
}