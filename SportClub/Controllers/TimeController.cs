using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportClub.BLL.DTO;
using SportClub.BLL.Interfaces;
using SportClub.Models;
using System.Drawing;

namespace SportClub.Controllers
{
    public class TimeController : Controller
    {
        private readonly IAdmin adminService;
        private readonly IUser userService;
        private readonly ICoach coachService;
        private readonly ITime timeService;
        private readonly ISpeciality specialityService;
        public TimeController(IAdmin adm, IUser us, ICoach c, ISpeciality sp, ITime t)
        {
            adminService = adm;
            userService = us;
            coachService = c;
            timeService = t;
            specialityService = sp;
        }
        [HttpGet]
        public async Task<IActionResult> AddTimeT()
        {
            IEnumerable<TimeTDTO> p= await timeService.GetAllTimeTs();
            List<TimeShow> p1 = new();
            foreach (var t in p)
            {
                TimeShow ts = new()
                {
                    Id = t.Id,
                    Time = t.StartTime + "/" + t.EndTime
                };
                p1.Add(ts);
            }

            ViewData["TimeId"] = new SelectList(p1, "Id", "Time");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddTimeT(string Start,string End)
        {
           await timeService.AddTimeT(Start,End);
            IEnumerable<TimeTDTO> p = await timeService.GetAllTimeTs();
            List<TimeShow> p1 = new();
            foreach (var t in p)
            {
                TimeShow ts = new()
                {
                    Id = t.Id,
                    Time = t.StartTime + "/" + t.EndTime
                };
                p1.Add(ts);
            }
            ViewData["TimeId"] = new SelectList(p1, "Id", "Time");
            return View();
        }
    }
}
