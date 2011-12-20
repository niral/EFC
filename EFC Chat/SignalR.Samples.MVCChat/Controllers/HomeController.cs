﻿using System.Web.Mvc;

namespace SignalR.Samples.MVCChat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to EFC - Exercises Forum Chat";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
