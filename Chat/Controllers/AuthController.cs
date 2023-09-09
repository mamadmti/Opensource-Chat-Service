using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
	[Route("[action]")]
	public class AuthController : Controller
    {
		[HttpGet]
		[Route("/")]
		[Route("/auth")]

		public IActionResult Auth()
        {
            return View();
        }

		[HttpGet]

		public IActionResult SignUp()
		{
			return View();
		}

	}
}
