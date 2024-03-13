using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Index(string scrollTo, int coachpage = 1, int roompage = 1,
            bool ispresscoach=false, bool ispressroom = false)
        {
            if (ispresscoach)
                scrollTo = "allcoaches";
            if(ispressroom)
                scrollTo = "allrooms";
            int pageSize = 2;
            var p = await coachService.GetAllCoaches();
            var p1= p.OrderBy(t => t.Id)
                            .Skip((coachpage - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
            var r= await roomService.GetAllRooms();
            var r1 = r.OrderBy(t => t.Id)
                            .Skip((roompage - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
            StartViewModel  start = new StartViewModel();
            //start.coaches=p.ToList();
            //start.rooms=r.ToList();
            start.coaches = p1;
            start.CurrentPageCoaches = coachpage;
            int sizeC = 0;
            if (p.Count() % pageSize == 0)
            {
                sizeC = p.Count() / pageSize;
            }
            else
            {
                sizeC = p.Count() / pageSize + 1;
            }
            start.TotalPagesCoaches = sizeC;
            start.rooms = r1;
            start.CurrentPageRooms = roompage;
            int sizeR = 0;
            if (r.Count() % pageSize == 0)
            {
                sizeR = r.Count() / pageSize;
            }
            else
            {
                sizeR = r.Count() / pageSize + 1;
            }
            start.TotalPagesRooms = sizeR;
            ViewBag.ScrollTo = scrollTo;
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