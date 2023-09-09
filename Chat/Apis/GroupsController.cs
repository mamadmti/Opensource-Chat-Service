using System.Linq;
using System.Text.RegularExpressions;
using Chat.Domain.Contexts;
using Chat.Domain.Contracts.Services;
using Chat.Domain.Entities;
using Chat.Domain.ViewModels;
using Chat.Helper;
using Chat.Services;
using Chat.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SharedLibrary.Helpers;
using static Azure.Core.HttpHeader;

namespace Chat.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {

        readonly IServiceFactory _services;
        public IConfiguration _configuration;
        private readonly ChatDbContext _context;
        public GroupsController(IServiceFactory service, IConfiguration config, ChatDbContext context, IOptions<appSettingClass> options)
        {
            _services = service;
            _configuration = config;
            _context = context;
            _formatSettings = options.Value;
		}
        private readonly appSettingClass _formatSettings;
		/// <summary>
		/// Create new Group 
		/// </summary>
		/// <param name="R"></param>
		/// <returns></returns>
		[Auth]
        [HttpPost("NewGroup")]
        public async Task<IActionResult> NewGroup(NewGroupVm R)
        {
			if (R.GroupName.Length >= 5 && R.GroupName.Substring(0, 5) == "user-")
			{
				return BadRequest("Groupname can not start with this name");
			}

			Groups Group = new Groups();
            Group.GroupName = R.GroupName.ToLower();
            Group.CreateAt = DateTime.Now;
            Group.UserId = Guid.Parse(HttpContext.Request.Headers["UserGuid"]);
            Group.ApiKey = _formatSettings.apiKey;
            var x = await _services.GroupsService.Create(Group);
            await _services.SaveAsync();

            //Then Admin Should be Added To Its own Group
            newGroupUsers gu = new newGroupUsers();
            gu.UsersId = long.Parse(HttpContext.Request.Headers["UserId"]);
            gu.GroupsId = Group.Id;
            var res =await AddPeopleToGroup(gu);

            return Ok("Group " + R.GroupName + "Created");
        }

        /// <summary>
        /// Create new group with people inside of it (All in one api)
        /// </summary>
        /// <param name="R"></param>
        /// <returns></returns>
        [Auth]
		[HttpPost("NewGroupWithPeople")]
        public async Task<IActionResult> NewGroupWithPeople(NewGroupWithPeopleVm R)
        {
	        if ( R.GroupName.Length >= 5 && R.GroupName.Substring(0, 5) == "user-")
	        {
		        return BadRequest("Groupname can not start with this name");
	        }

			Groups Group = new Groups();
	        Group.GroupName = R.GroupName.ToLower();
	        Group.CreateAt = DateTime.Now;
	        Group.UserId = Guid.Parse(HttpContext.Request.Headers["UserGuid"]);
	        Group.ApiKey = _formatSettings.apiKey;
	        var x = await _services.GroupsService.Create(Group);
	        await _services.SaveAsync();

	        //Then Admin Should be Added To Its own Group
	        newGroupUsers gu = new newGroupUsers();
	        gu.UsersId = long.Parse(HttpContext.Request.Headers["UserId"]);
	        gu.GroupsId = Group.Id;
	        var res = await AddPeopleToGroup(gu);


			//now add people to that group
			List<string> gpofpeople = new List<string>();
			gpofpeople.AddRange(R.People.Split(' ').ToList());
			
			var peopleprofiles =await _services.UsersService.ReadAll(new UsersSpecifications(gpofpeople));

			peopleprofiles = peopleprofiles.Where(a => a.Id != gu.UsersId); // except the creator because it was added before
			foreach (var person in peopleprofiles)
			{
				newGroupUsers guu = new newGroupUsers();
				guu.UsersId = person.Id;
				guu.GroupsId = Group.Id;
				var res2 = await AddPeopleToGroup(guu);
			}
			

			return Ok("Group " + R.GroupName + "Created");
        }

		/// <summary>
		/// Add People To Your Group
		/// </summary>
		/// <param name="R"></param>
		/// <returns></returns>
		[Auth]
        [HttpPost("AddPeopleToGroup")]
        public async Task<IActionResult> AddPeopleToGroup(newGroupUsers R)
        {

            UserGroups ug = new UserGroups();
            ug.UsersId = R.UsersId;
            ug.CreateAt = DateTime.Now;
            ug.UserId = Guid.Parse(HttpContext.Request.Headers["UserGuid"]);
            ug.GroupsId = R.GroupsId;
            ug.ApiKey = _formatSettings.apiKey;
            var x = await _services.UserGroupsService.Create(ug);
            await _services.SaveAsync();
            return Ok("User Added To The Requested Group");
        }


        /// <summary>
        /// Get Group Users
        /// </summary>
        /// <param name="R"></param>
        /// <returns></returns>
        [Auth]
        [HttpGet("GetGroupUsers/{groupId}")]
        public async Task<IActionResult> GetGroupUsers(long groupId)
        {
            //First See If The User Is In Requested Group

            var useringroup = await _services.UserGroupsService.ReadById(long.Parse(HttpContext.Request.Headers["UserId"]));
            if (useringroup==null)
            {
                return BadRequest("Bad Request ! You Are Not In This Group");
            }
            else
            {
                var usersingroup = await _services.UserGroupsService.ReadAll(new UserGroupsSpecifications(groupId));
                return Ok(usersingroup);
            }

        }

        /// <summary>
        /// List Of Current User Groups
        /// </summary>
        /// <returns></returns>
        [Auth]
        [HttpGet("GetListOfCurrentUserGroups")]
        public async Task<IActionResult> GetListOfCurrentUserGroups()
        {
            //First See If The User Is In Requested Group

            long userid = long.Parse(HttpContext.Request.Headers["UserId"]);

            var usersingroup = await _services.UserGroupsService.ReadAll(new UserGroupsSpecifications(userid));

            List<Groups> gp = new List<Groups>();
            foreach (var VARIABLE in usersingroup)
            {
                gp.Add(VARIABLE.Groups);
            }

            return Ok(gp);
            

        }

    }
}
