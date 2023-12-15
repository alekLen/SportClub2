using Microsoft.AspNetCore.Mvc;
using SportClub.BLL.DTO;
using SportClub.BLL.Interfaces;
using SportClub.DAL.Entities;

namespace SportClub.Controllers
{
    public class TrainingIndController : Controller
    {
        private readonly IUser userService;
        private readonly ICoach coachService;
       // private readonly IPost postService;
        private readonly ISpeciality specialityService;
        private readonly ITrainingInd trainingIndService;
        public TrainingIndController(/*ITime time,*/ IUser us, ICoach c, ISpeciality sp, ITrainingInd t)
        {
           // timeService = time;
            userService = us;
            coachService = c;
            trainingIndService = t;
            specialityService = sp;
        }
        public async Task<IActionResult> GetAllTrainingIndsOfCoach( int id)
        {
            var p = await trainingIndService.GetAllOfCoachTrainingInds(id);
            return View(p);
        }
        public async Task<IActionResult> GetAllTrainingIndsOfClient(int id)
        {
            var p = await trainingIndService.GetAllOfClientTrainingInds(id);
            return View(p);
        }
        public async Task<IActionResult> GetAllTrainingInds()
        {
            var p = await trainingIndService.GetAllTrainingInds();
            return View(p);
        }

        public async Task<IActionResult> GetTrainingInd( int id)
        {
            TrainingIndDTO p = await trainingIndService.GetTrainingInd(id);
            return View(p);
        }
        public async Task<IActionResult> Details(int id)
        {
            TrainingIndDTO t = await trainingIndService.GetTrainingInd(id);
            return View(t);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            TrainingIndDTO t = await trainingIndService.GetTrainingInd(id);
            return View(t);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TrainingIndDTO c)
        {
            try
            {
                TrainingIndDTO t = await trainingIndService.GetTrainingInd(c.Id.Value);
                t.Id = c.Id;
                t.Name = c.Name;
                //t.TimeId = c.TimeId;
                t.RoomId = c.RoomId;
                t.CoachName = c.CoachName;
                t.CoachId = c.CoachId;
                t.UserName = c.UserName;
                t.UserId = c.UserId;
               // t.SpecialityName = c.SpecialityName;
               // t.SpecialityId = c.SpecialityId;
                 await trainingIndService.UpdateTrainingInd(t);
                return RedirectToAction("GetTrainingInds");
            }
            catch { return View(c); }
        }
    }
}
