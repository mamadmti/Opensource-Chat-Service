﻿using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
