using Microsoft.AspNetCore.Mvc;
using SportClub.BLL.DTO;
using SportClub.BLL.Interfaces;

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
        public IActionResult AddTimeT()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddTimeT(TimeTDTO t)
        {
            timeService.AddTimeT(t);
            return View();
        }
    }
}
