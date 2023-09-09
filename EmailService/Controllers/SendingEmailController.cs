using System.Net;
using MailKit;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MailKit.Security;
using SharedLibrary.MessagingLibraries;
using EmailService.Class;
using IMailService = EmailService.Class.IMailService;
using SharedLibrary.Extensions;

namespace EmailService.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class SendingEmailController : ControllerBase
	{
		private readonly ILogger<SendingEmailController> _logger;
		private readonly IMailService mailService;
		public SendingEmailController(ILogger<SendingEmailController> logger, IMailService mailService)
		{
			_logger = logger;
			this.mailService = mailService;
		}

		//[HttpPost]
		//public async Task<IActionResult> Post([FromForm] MailRequest request)
		//{

		//	try
		//	{
		//		await mailService.SendEmailAsync(request);
		//		return Ok();
		//	}
		//	catch (Exception ex)
		//	{

		//		throw ex;
		//	}

		//}

		[TypeFilter(typeof(ValidationFilterAttribute))]
		[HttpPost("Mail")]
		public async Task<ResponseHandling> PostMail( MailRequest request)
		{

			try
			{
				await mailService.SendEmailAsync(request);
				return new ResponseHandling(HttpStatusCode.OK ,"Email Sent");
			}
			catch (Exception ex)
			{

				return new ResponseHandling(HttpStatusCode.BadRequest,"Check your email address");
			}

		}

	}
}