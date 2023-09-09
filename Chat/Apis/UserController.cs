using Chat.Domain.Entities;
using Chat.Domain.ViewModels;
using Chat.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Chat.Domain.Contexts;
using Chat.Helper;
using Microsoft.AspNetCore.Authorization;
using Chat.Specifications;
using System;
using System.Net;
using System.Text.Json;
using Chat.Domain.Entities.Enums;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SharedLibrary.Extensions;
using SharedLibrary.MessagingLibraries;
using System.Text.RegularExpressions;
using SharedLibrary.Helpers;
using Microsoft.Extensions.Options;
using SharedLibrary.Enums;

namespace Chat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        readonly IServiceFactory _services;
        public IConfiguration _configuration;
        private readonly ChatDbContext _context;
        public UserController(IServiceFactory service, IConfiguration config, ChatDbContext context, IOptions<appSettingClass> options)
        {
            _services= service;
            _configuration = config;
            _context = context;
            _formatSettings = options.Value;
		}
        private readonly appSettingClass _formatSettings;

        /// <summary>
        /// SignUpWithEmailPhase1 (Description in document)
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>

        [HttpPost("SignUpWithEmailPhase1")]
        public async Task<ResponseHandling> SignUpWithEmailPhase1(SignUpUserPhaseOneVm u)
        {


            Users user = new Users();
            user.PhoneNumber = u.PhoneNumber;
            user.Email = u.UserEmail.ToLower();

            if (!Extension.IsDigitsOnly(user.PhoneNumber))
            {
                return new ResponseHandling(HttpStatusCode.BadRequest, "Phone Number Can Only Contain Numbers");
            }

            user.UserId = Guid.NewGuid(); ;
            user.CreateAt = DateTime.Now;
            user.ApiKey = _formatSettings.apiKey;
            user.RecordStatus = RecordStatus.InActive;

			string vrcode = Extension.GetRandomNumber(0, 1000000).ToString();

			var doesEmailexist = await _services.UsersService.ReadAll(new UsersEmailSpecifications(u.UserEmail));
            if (doesEmailexist.Any())
            {
	            //return new ResponseHandling(HttpStatusCode.BadRequest, "You already signed up , if you don't remember your password use the forget password");
	            user = doesEmailexist.FirstOrDefault();
	            user.TemporaryValidationText = vrcode;
	            user.VerificationCreatedAt = DateTime.Now;
                await _services.UsersService.Update(user);
                await _services.SaveAsync();

			}
			else
            {
	            user.TemporaryValidationText = vrcode;
	            user.VerificationCreatedAt = DateTime.Now;
                var x = await _services.UsersService.Create(user);
				await _services.SaveAsync();
			}



		    // Send  Email
		    MailRequest mail = new MailRequest();
			mail.Subject = "Chatapp Email Verification";

			mail.Body = "Your verification code :" + vrcode;

            mail.ToEmail = user.Email;

			HttpHelper xx = new HttpHelper();

			xx.apiBasicUri = _formatSettings.emailServiceUrl;

			ResponseHandling ex = await xx.Post<ResponseHandling>("SendingEmail/Mail", mail, _formatSettings.apiKey.ToString());
            
			return ex ;
        }





        /// <summary>
        /// SignUpWithEmailPhase2 (Description in document)
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
		[HttpPost("SignUpWithEmailPhase2")]
        public async Task<ResponseHandling> SignUpWithEmailPhase2 (SignUpUserPhaseTwoVm u)
        {
	        if (u.UserName.Length >= 6 && u.UserName.Substring(0, 6) == "group-")
	        {
		        return new ResponseHandling( HttpStatusCode.BadRequest,"Username can not start with this name");
	        }
	        Users user = new Users();
	       

			var doesEmailexist = await _services.UsersService.ReadAll(new UsersEmailSpecifications(u.UserEmail));
	        if (doesEmailexist.Any())
	        {
		        user = doesEmailexist.FirstOrDefault();
		        user.LastTryAt = DateTime.Now;
		        user.OverAllTryCount = user.OverAllTryCount + 1;
                await _services.UsersService.Update(user);
                await _services.SaveAsync();

                var doesUserNameexist = await _services.UsersService.ReadAll(new UsersSpecifications(u.UserName));
				if (doesUserNameexist.Any())
				{
					return new ResponseHandling(HttpStatusCode.BadRequest, "UserName is taken");
				}


				user = doesEmailexist.FirstOrDefault();
				if (u.VerificationCode != user.TemporaryValidationText)
				{
					return new ResponseHandling(HttpStatusCode.BadRequest, "Wrong verification code");
				}

				//set the default user-emailaddress if user dont fill the username 
				// if there was no username taken then previous one will stay still
				if (u.UserName != String.Empty)
				{
                    user.UserName = u.UserName;

                    bool usernamematch = Regex.IsMatch(u.UserName, @"^[a-zA-Z0-9_]+$");
                    if (!usernamematch)
                    {
	                    return new ResponseHandling(HttpStatusCode.BadRequest, "username can only contain letters and underscore and numbers");
                    }
				}
				

				if (user.UserName == String.Empty || user.UserName == null)
				{
					user.UserName = "user-" + u.UserEmail;

					var charsToRemove = new string[] { "@", ",", ".", ";", "'" };
					foreach (var c in charsToRemove)
					{
						user.UserName = user.UserName.Replace(c, string.Empty);
					}

					
				}


				user.Email = u.UserEmail.ToLower();
				user.Password = u.Password;


				if (user.VerificationCreatedAt < DateTime.Now.AddMinutes(-10))
				{
					return new ResponseHandling(HttpStatusCode.BadRequest, "Verification code expired");
				}

                if (user.OverAllTryCount > 5 && user.LastTryAt < DateTime.Now.AddMinutes(-10))
                {
                    return new ResponseHandling(HttpStatusCode.BadRequest, "Too much requests , try again later");
                }

                user.OverAllTryCount = 0;

                await _services.UsersService.Update(user);
                
				await _services.SaveAsync();

			}
	        else
	        {
					return new ResponseHandling(HttpStatusCode.BadRequest, "User doesnt exist");
	        }

			

	     
	        

	        // Go Sign IN

	        UserLoginVm loginVm = new UserLoginVm();
	        loginVm.Username = user.UserName;
	        loginVm.Password = u.Password;
	        var x = await Login(loginVm);
            return new ResponseHandling(HttpStatusCode.OK,x.Response);
        }






		//[HttpPost("SignUp")]
  //      public async Task<IActionResult> SignUp(NewUserVm u)
  //      {
	 //       if (u.UserName.Length >= 6 &&  u.UserName.Substring(0, 6) =="group-")
	 //       {
		//        return BadRequest("Username can not start with this name");
	 //       }

	      


		//	Users user = new Users();
  //          user.Email = u.UserEmail.ToLower();
  //          user.Password = u.Password;
  //          user.PhoneNumber = u.PhoneNumber.ToLower();
  //          if (!Extension.IsDigitsOnly(user.PhoneNumber))
  //          {
  //              return BadRequest("Phone Number Can Only Contain Numbers");
  //          }
  //          user.UserName = u.UserName.ToLower();
  //          user.CreateAt = DateTime.Now;
  //          user.ApiKey = Guid.Parse(_formatSettings.apiKey);

  //          var doesUserNameexist = await _services.UsersService.ReadAll(new UsersSpecifications(u.UserName));
  //          if (doesUserNameexist.Any())
  //          {
  //              return BadRequest("UserName Is Taken");
  //          }   
            
  //          var doesEmailexist = await _services.UsersService.ReadAll(new UsersEmailSpecifications(u.UserEmail));
  //          if (doesEmailexist.Any())
  //          {
  //              return BadRequest("You Already Signed Up , If You Don't Remember Your Password Use The Forget Password");
  //          }
  //          var x = await _services.UsersService.Create(user);
  //          await _services.SaveAsync();

  //          // Go Sign IN

  //          UserLoginVm loginVm = new UserLoginVm();
  //          loginVm.Username = user.UserName;
  //          loginVm.Password = u.Password;
  //          return await Login(loginVm);
  //      }



        //Create New User


        
        // Login

        [HttpPost("Login")]
        public async Task<ResponseHandling> Login(UserLoginVm _userData)
        {
            if (_userData != null && _userData.Username != null && _userData.Password != null)
            {
                var user = await GetUser(_userData);

                if (user != null)
                {
                    try
                    {
                        //create claims details based on the user information
                        var claims = new[] {
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                            new Claim("UserId", user.Id.ToString()),
                            new Claim("UserEmail", user.Email),
                            new Claim("UserName", user.UserName),
                            new Claim("Email", user.Email),
                            new Claim("UserGuid", user.UserId.ToString())
                            
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(10),
                            signingCredentials: signIn);
                        return new ResponseHandling(HttpStatusCode.OK,new JwtSecurityTokenHandler().WriteToken(token));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    

                    
                }
                else
                {
                    return new ResponseHandling(HttpStatusCode.BadRequest,"Invalid credentials");
                }
            }
            else
            {
                return new ResponseHandling(HttpStatusCode.BadRequest);
            }
        }



        private async Task<Users?> GetUser(UserLoginVm UserLoginVm)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == UserLoginVm.Username && u.Password == UserLoginVm.Password);
        }


        [Auth]
        [HttpGet("OnlineUsersUserId")]
        public async Task<ActionResult<IEnumerable<UsersProfile>>> OnlineUsersUserId()
        {
            //_services.Apikey = Guid.Parse(HttpContext.Items["api-key"].ToString()??string.Empty);
            _services.Apikey = _formatSettings.apiKey;
            var allusers = await _services.UsersConnectionsService.ReadAll();
            
            allusers = allusers.DistinctBy(a => a.UsersId);
            List<long> ids = new List<long>();
            foreach (var id in allusers)
            {
                ids.Add(id.UsersId);
            }
            
            
            //get usernames and show usernames
            var result = await _services.UsersService.ReadAll(new UsersSpecifications(ids));
            List<UsersProfile> up = new List<UsersProfile>();
            foreach (var VARIABLE in result)
            {
                UsersProfile UsersProfile = new UsersProfile();
                UsersProfile.UserName = VARIABLE.UserName;
                UsersProfile.UserId = VARIABLE.Id;
                up.Add(UsersProfile);
            }

            return Ok(up);
        }


       public class st
        {
	        public int skip { get; set; }
	        public int take { get; set; }

        }


		/// <summary>
		/// Get all users and groups that user has been joined
		/// </summary>
		/// <returns></returns>
		[Auth]
		[HttpPost("AllUsers")]
        public async Task<ActionResult<IEnumerable<UsersProfile>>> AllUsers(st st)
        {
	        //_services.Apikey = Guid.Parse(HttpContext.Items["api-key"].ToString()??string.Empty);
	        _services.Apikey = _formatSettings.apiKey;
	        long Currentuserid = long.Parse(HttpContext.Request.Headers["UserId"]);

			//get usernames and show usernames
			var result = await _services.UsersService.ReadAll(skip: st.skip, take: st.take);
	        List<UsersProfile> up = new List<UsersProfile>();
	        foreach (var VARIABLE in result)
	        {
		        UsersProfile UsersProfile = new UsersProfile();
		        UsersProfile.UserName = VARIABLE.UserName;
		        UsersProfile.UserId = VARIABLE.Id;
		        up.Add(UsersProfile);
	        }

			//get all groups of current user
			//var result3 = await _services.GroupsService.ReadAll();

			//var result2 = await _services.UserGroupsService.ReadAll(new UserGroupsforuseridSpecifications(Currentuserid));
			
			//foreach (var VARIABLE in result2)
			//{
			//	UsersProfile UsersProfile = new UsersProfile();
			//	UsersProfile.UserName = VARIABLE.Groups.GroupName;
			//	UsersProfile.GroupId = VARIABLE.GroupsId.ToString();

			//	up.Add(UsersProfile);
			//}

			//
			//

			return Ok(up);
        }

		/// <summary>
		/// Gets all groups and chats that are available and this user opened them before or joined them before
		/// </summary>
		/// <returns></returns>
		[Auth]
		[HttpGet("CurrentUsersChatList")]
        public async Task<ActionResult<IEnumerable<UsersProfile>>> CurrentUserChatList()
        {

            //get chat histories of current user and just show the users which current user had conversation with

            //_services.Apikey = Guid.Parse(HttpContext.Items["api-key"].ToString()??string.Empty);
            _services.Apikey = _formatSettings.apiKey;
            Guid UserGuid = Guid.Parse(HttpContext.Request.Headers["UserGuid"]);
            long Currentuserid = long.Parse(HttpContext.Request.Headers["UserId"]);

            //get users that i had conversation with
            IEnumerable<long?> History = await _services.ChatHistoryService.GetUniqueIds();
			//IEnumerable<ChatHistory> History = await _services.ChatHistoryService.ReadAll(new ChatHistoriesToUserIdSpecifications(UserGuid, Currentuserid),null,null,true);
			
			List<long> showingids = new List<long>();
            foreach (var VARIABLE in History)
            {
                if (VARIABLE != null)
                {
                    showingids.Add(VARIABLE ?? new long());
                }
            }


            //get usernames and show usernames
            var result = await _services.UsersService.ReadAll(new UsersSpecifications(showingids));


            List<UsersProfile> up = new List<UsersProfile>();
            foreach (var VARIABLE in result)
            {
	            long UnReadcount = await _services.ChatHistoryService.GetCount(new ChatHistoriesToUserIdChatUnreadCountSpecifications(VARIABLE.Id, UserGuid, Currentuserid, VARIABLE.UserId));

				UsersProfile UsersProfile = new UsersProfile();
                UsersProfile.UserName = VARIABLE.UserName;
                UsersProfile.UserId = VARIABLE.Id;
                UsersProfile.UnReadcount = UnReadcount;
                up.Add(UsersProfile);
            }



            //get all groups of current user
            var result3 = await _services.GroupsService.ReadAll();

            var result2 = await _services.UserGroupsService.ReadAll(new UserGroupsforuseridSpecifications(Currentuserid));

            foreach (var VARIABLE in result2)
            {
                UsersProfile UsersProfile = new UsersProfile();
                UsersProfile.UserName = VARIABLE.Groups.GroupName;
                UsersProfile.GroupId = VARIABLE.GroupsId.ToString();

                up.Add(UsersProfile);
            }
            //
            //

            return Ok(up);
        }








	}
}