using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportClub.BLL.DTO;
using SportClub.BLL.Interfaces;
using SportClub.BLL.Services;
using SportClub.DAL.Entities;
using SportClub.Models;

namespace SportClub.Controllers
{
    public class TrainingGroupController : Controller
    {
        //private readonly IGroup groupService;
        private readonly ICoach coachService;

        //private readonly IUser userService;
        // private readonly IPost postService;

        private readonly ISpeciality specialityService;
        private readonly ITrainingGroup trainingGroupService;

        private readonly ITime timeService;
        private readonly IRoom roomService;

        public TrainingGroupController(ITime time, /*IGroup group,*/ ICoach c, IRoom r, ISpeciality sp, ITrainingGroup t)
        {
            timeService = time;
            //groupService = group;
            roomService = r;
            coachService = c;
            trainingGroupService = t;
            specialityService = sp;
        }
        
        public async Task<IActionResult> CreateTrainingGroup()
        {
            HttpContext.Session.SetString("path", Request.Path);
            await putCoaches();
            //await putGroups();
            await putRooms();
            await putTimes();//
            await putSpecialitys();//
            return View("CreateTrainingGroup");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainingGroup(TrainingGroupDTO group)
        {
            HttpContext.Session.SetString("path", Request.Path);

            if (ModelState.IsValid)
            {
                var coach = await coachService.GetCoach(group.CoachId);
                var room = await roomService.GetRoom(group.RoomId);
                //var gr = await groupService.GetGroup(group.GroupId);
                //var sp = await specialityService.GetSpeciality((int)group.SpecialityId);

                TrainingGroupDTO u = new();
                u.Name = group.Name;
                //u.Number = group.Number;

                u.CoachName = coach.Name;
                u.CoachId = group.CoachId;//

                u.RoomName = room.Name;
                u.RoomId = group.RoomId;//
                //u.TimeId = group.TimeId;

                //u.GroupName = gr.Name;
                //u.GroupId = group.GroupId;//

                //u.SpecialityName = sp.Name;
                //u.SpecialityId = group.SpecialityId;

                try
                {
                    await trainingGroupService.AddTrainingGroup(u);
                }
                catch { }
                return RedirectToAction("Index", "Home");
            }
            return View("CreateTrainingGroup", group);
        }

        public async Task putCoaches()
        {
            HttpContext.Session.SetString("path", Request.Path);
            IEnumerable<CoachDTO> p = await coachService.GetAllCoaches();
            ViewData["CoachListId"] = new SelectList(p, "Id", "Name");
        }
        //public async Task putGroups()
        //{
        //    HttpContext.Session.SetString("path", Request.Path);
        //    IEnumerable<GroupDTO> p = await groupService.GetAllGroups();
        //    ViewData["GroupTLId"] = new SelectList(p, "Id", "Name");
        //} 
        public async Task putTimes()
        {
            IEnumerable<TimeTDTO> p = await timeService.GetAllTimeTs();

            IEnumerable<TimeTDTO> p2 = p.OrderBy(x => double.Parse(x.StartTime.Split(':')[0] + "," + x.StartTime.Split(':')[1]));
            List<TimeShow> p1 = new();
            foreach (var t in p2)
            {
                TimeShow ts = new()
                {
                    Id = t.Id,
                    Time = t.StartTime + "/" + t.EndTime
                };
                p1.Add(ts);
            }
            p1.OrderBy(x => x.Time).ToList();
            ViewData["TimeId"] = new SelectList(p1, "Id", "Time");
        }

        public async Task putRooms()
        {
            HttpContext.Session.SetString("path", Request.Path);
            IEnumerable<RoomDTO> p = await roomService.GetAllRooms();
            ViewData["RoomId"] = new SelectList(p, "Id", "Name");
        }

        public async Task putSpecialitys()
        {
            HttpContext.Session.SetString("path", Request.Path);
            IEnumerable<SpecialityDTO> p = await specialityService.GetAllSpecialitys();
            ViewData["SpecialityId"] = new SelectList(p, "Id", "Name");
        }//

        public async Task<IActionResult> GetTrainingGroups()
        {
            var p = await trainingGroupService.GetAllTrainingGroups();
            return View(p);
        }


        public async Task<IActionResult> EditTrainingGroup(int id)
        {
            HttpContext.Session.SetString("path", Request.Path);
            TrainingGroupDTO trainingGroupdto = await trainingGroupService.GetTrainingGroup(id);
            if (trainingGroupdto != null)
            {
                await putCoaches();
                //await putGroups();
                await putRooms();
                await putTimes();//
                await putSpecialitys();//
                //await putUsers();
                //await putCoaches(); 
                return View("EditTrainingGroup", trainingGroupdto);
            }

            return View("GetTrainingGroups", "TrainingGroup");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTrainingGroup(int id, TrainingGroupDTO group)
        {
            HttpContext.Session.SetString("path", Request.Path);
            try
            {
                TrainingGroupDTO trainingGroupdto = await trainingGroupService.GetTrainingGroup(id);
                if (trainingGroupdto == null)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var coac = await coachService.GetCoach((int)group.CoachId);
                    var room = await roomService.GetRoom((int)group.RoomId);
                    //var sp = await specialityService.GetSpeciality((int)group.SpecialityId);

                    trainingGroupdto.Name = group.Name;
                    //trainingGroupdto.Number = group.Number;

                    trainingGroupdto.CoachName = coac.Name;
                    trainingGroupdto.CoachId = group.CoachId;

                    trainingGroupdto.RoomName = room.Name;
                    trainingGroupdto.RoomId = group.RoomId;
                    //trainingGroupdto.TimeId = group.TimeId;

                    //trainingGroupdto.GroupName = room.Name;
                    //trainingGroupdto.GroupId = group.GroupId;

                    //trainingGroupdto.SpecialityName = sp.Name;
                    //trainingGroupdto.SpecialityId = group.SpecialityId;
                    


                    try
                    {
                        await trainingGroupService.UpdateTrainingGroup(trainingGroupdto);
                    }
                    catch { return View("EditTrainingGroup", group); }
                    return RedirectToAction("GetTrainingGroups", "TrainingGroup");
                    // } 
                }
                return RedirectToAction("GetTrainingGroups");
            }
            catch
            {
                return View("GetTrainingGroups", "TrainingGroup");
            }
        }

        public async Task<IActionResult> DeleteTrainingGroup(int id)
        {
            HttpContext.Session.SetString("path", Request.Path);
            TrainingGroupDTO trainingGroup = await trainingGroupService.GetTrainingGroup(id);
            if (trainingGroup == null)
            {
                return NotFound();
            }
            return View(trainingGroup);
        }
        [HttpPost, ActionName("DeleteTrainingGroup")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            TrainingGroupDTO trainingGroup = await trainingGroupService.GetTrainingGroup(id);
            if (trainingGroup == null)
            {
                return NotFound();
            }

            await trainingGroupService.DeleteTrainingGroup(id);
            return RedirectToAction("GetTrainingGroups", "TrainingGroup");
        }
        public async Task<IActionResult> DetailsTrainingGroup(int id)
        {
            HttpContext.Session.SetString("path", Request.Path);
            TrainingGroupDTO trainingGroup = await trainingGroupService.GetTrainingGroup(id);
            if (trainingGroup == null)
            {
                return NotFound();
            }
            return View(trainingGroup);
        }
    }
}
