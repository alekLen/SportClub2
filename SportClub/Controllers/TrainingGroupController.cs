using Microsoft.AspNetCore.Mvc;
using SportClub.BLL.DTO;
using SportClub.BLL.Interfaces;

namespace SportClub.Controllers
{
    public class TrainingGroupController : Controller
    {
        private readonly IUser userService;
        private readonly ICoach coachService;
        // private readonly IPost postService;
        private readonly ISpeciality specialityService;
        private readonly ITrainingGroup trainingGroupService;
        public TrainingGroupController(/*ITime time,*/ IUser us, ICoach c, ISpeciality sp, ITrainingGroup t)
        {
            // timeService = time;
            userService = us;
            coachService = c;
            trainingGroupService = t;
            specialityService = sp;
        }
        public async Task<IActionResult> GetAllTrainingGroupsOfCoach(int id)
        {
            var p = await trainingGroupService.GetAllOfCoachTrainingGroups(id);
            return View(p);
        }
        public async Task<IActionResult> GetAllTrainingGroupsOfClient(int id)
        {
            var p = await trainingGroupService.GetAllOfClientTrainingGroups(id);
            return View(p);
        }
        public async Task<IActionResult> GetAllTrainingGroups()
        {
            var p = await trainingGroupService.GetAllTrainingGroups();
            return View(p);
        }

        public async Task<IActionResult> GetTrainingGroup(int id)
        {
            TrainingGroupDTO p = await trainingGroupService.GetTrainingGroup(id);
            return View(p);
        }
        public async Task<IActionResult> Details(int id)
        {
            TrainingGroupDTO t = await trainingGroupService.GetTrainingGroup(id);
            return View(t);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            TrainingGroupDTO t = await trainingGroupService.GetTrainingGroup(id);
            return View(t);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TrainingGroupDTO c)
        {
            try
            {
                TrainingGroupDTO t = await trainingGroupService.GetTrainingGroup(c.Id);
                t.Id = c.Id;
                t.Name = c.Name;
                t.Number=c.Number;  
                t.TimeId = c.TimeId;
                t.RoomId = c.RoomId;
                t.CoachName = c.CoachName;
                t.CoachId = c.CoachId;
                t.GroupName = c.GroupName;
                t.GroupId = c.GroupId;
                t.SpecialityName = c.SpecialityName;
                t.SpecialityId = c.SpecialityId;
                await trainingGroupService.UpdateTrainingGroup(t);
                return RedirectToAction("GetTrainingGroups");
            }
            catch { return View(c); }
        }
    }
}
