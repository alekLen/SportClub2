using Microsoft.AspNetCore.Mvc;
using SportClub.BLL.DTO;

namespace SportClub.Controllers
{
    public class TimeController : Controller
    {
        [HttpGet]
        public IActionResult AddTimeT()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddTimeT(TimeTDTO t)
        {

            return View();
        }
    }
}
