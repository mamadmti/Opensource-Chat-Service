using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using SharedLibrary.Helpers;

namespace SharedLibrary.Extensions
{

	public class ValidationFilterAttribute : IActionFilter
	{
		public ValidationFilterAttribute(IOptions<appSettingClass> options)
		{
			_formatSettings = options.Value;
		}
		private readonly appSettingClass _formatSettings;
		public void OnActionExecuting(ActionExecutingContext context)
		{
			var apiKey = context.HttpContext.Request.Headers["api-key"];
			if (apiKey != _formatSettings.apiKey.ToString())
			{
				throw new Exception("Unknown error");
			}
		}
		public void OnActionExecuted(ActionExecutedContext context)
		{

		}
	}
}
