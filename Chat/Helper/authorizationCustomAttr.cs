using Chat.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace Chat.Helper
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class Auth : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var s = context.HttpContext.Request.Headers["Authorization"];
            if (AuthenticationHeaderValue.TryParse(s, out var headerValue))
            {
                // we have a valid AuthenticationHeaderValue that has the following details:

                var scheme = headerValue.Scheme;
                var parameter = headerValue.Parameter;

                // scheme will be "Bearer"
                // parmameter will be the token itself.
                // or
                var stream = parameter;
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(stream);
                var tokenS = handler.ReadToken(stream) as JwtSecurityToken;
                context.HttpContext.Request.Headers["UserName"] = tokenS.Claims.FirstOrDefault(a=>a.Type == "UserName")?.Value;
                context.HttpContext.Request.Headers["UserGuid"] = tokenS.Claims.FirstOrDefault(a=>a.Type == "UserGuid")?.Value;
                context.HttpContext.Request.Headers["UserId"] = tokenS.Claims.FirstOrDefault(a=>a.Type == "UserId")?.Value;


            }

            var user = context.HttpContext.Request.Headers["Authorization"];
            if (user == string.Empty || user.Count == 0)
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }


    }

}
