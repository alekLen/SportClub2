﻿using Microsoft.AspNetCore.Mvc;
using SportClub.BLL.Interfaces;
using SportClub.Filters;
using SportClub.Models;
using System.Diagnostics;

namespace SportClub.Controllers
{
    [Culture]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICoach coachService;
        private readonly IRoom roomService;

        public HomeController(IRoom r, ICoach c, ILogger<HomeController> logger)
        {
            _logger = logger;
            coachService = c;
            roomService = r;
        }

        public async Task<IActionResult> Index()
        {
            var p = await coachService.GetAllCoaches();
            var r= await roomService.GetAllRooms();
            StartViewModel  start = new StartViewModel();
            start.coaches=p.ToList();
            start.rooms=r.ToList();
            return View(start);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public ActionResult ChangeCulture(string lang)
        {
            string? returnUrl = HttpContext.Session.GetString("path") ?? "/Home/Index";
           // string? returnUrl =  "/Home/Index";

            // Список культур
            List<string> cultures = new List<string>() { "ru", "en", "uk", "de", "fr" };
            if (!cultures.Contains(lang))
            {
                lang = "ru";
            }

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(10); // срок хранения куки - 10 дней
            Response.Cookies.Append("lang", lang, option); // создание куки
            return Redirect(returnUrl);
            //return Redirect("/Home/Index");
        }
    }
}