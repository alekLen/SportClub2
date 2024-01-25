using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportClub.BLL.DTO;
using SportClub.BLL.Interfaces;
using SportClub.BLL.Services;
using SportClub.DAL.Entities;
using SportClub.DAL.Interfaces;
using SportClub.Models;
using System.IO;

namespace SportClub.Controllers
{
    public class CoachController : Controller
    {
        IWebHostEnvironment _appEnvironment;
        private readonly IAdmin adminService;
        private readonly ICoach coachService;
        private readonly IPost postService;
        private readonly ISpeciality specialityService;
        private readonly IRoom roomService;
        private readonly ITrainingInd trainingIndService;
        private readonly ITrainingGroup trainingGroupService;
        private readonly IGroup groupService;
        private readonly ITime timeService;
        private readonly IShedule sheduleService;

        private static List<TrainingIndDTO> TrI = new();
        private static List<TrainingGroupDTO> TrG = new();
        public CoachController(IShedule sh,ITime tt,IGroup gr, IAdmin adm, IRoom room, ICoach c, ISpeciality sp, IPost p, IWebHostEnvironment appEnvironment, ITrainingInd tr, ITrainingGroup tg)
        {
            adminService = adm;
            coachService = c;
            postService = p;
            specialityService = sp;
            _appEnvironment = appEnvironment;
            roomService = room;
            trainingIndService = tr;
            trainingGroupService = tg;
            groupService = gr;
            timeService = tt;
            sheduleService = sh;
        }

        // GET: Users
        public async Task<IActionResult> GetCoaches()
        {
            var p = await coachService.GetAllCoaches();
            await putPosts();
            await putSpecialities();
            return View(p);
        }


        //public async Task<IActionResult> CoachProfile()
        //{
        //    string s = HttpContext.Session.GetString("Id");
        //    int id = Int32.Parse(s);
        //    CoachDTO p = await coachService.GetCoach(id);
        //    return View(p);
        //}
        public async Task<IActionResult> Details(int id)
        {
            CoachDTO p = await coachService.GetCoach(id);
            return View(p);
        }
        //[HttpGet]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    CoachDTO p = await coachService.GetCoach(id);
        //    await putPosts();
        //    await putSpecialities();
        //    return View(p);
        //}
        //[HttpPost]
        //public async Task<IActionResult> Edit(CoachDTO c)
        //{
        //    try
        //    {
        //        CoachDTO p = await coachService.GetCoach(c.Id);
        //        p.PostId = c.PostId;
        //        p.SpecialityId = c.SpecialityId;
        //        p.Description = c.Description;
        //        p.Phone= c.Phone;
        //        p.Email= c.Email;
        //        p.Photo = c.Photo;
        //        await coachService.UpdateCoach(p);
        //        return RedirectToAction("GetCoaches");
        //    }
        //    catch { return View(c); }
        //}
        public async Task<IActionResult> CoachProfile()
        {
            try
            {
                string s = HttpContext.Session.GetString("Id");
                int id = Int32.Parse(s);
                CoachDTO p = await coachService.GetCoach(id);
                return View(p);
            }
            catch { return View("Index", "Home"); }
        }
        public async Task<IActionResult> EditCoachProfile(CoachDTO user, IFormFile? p)
        {
            CoachDTO u = await coachService.GetCoach(user.Id);
            if (u != null)
            {
                int age = 0;
            try
            {
                DateTime birthDate;
                if (DateTime.TryParse(user.DateOfBirth, out birthDate))
                {
                    DateTime currentDate = DateTime.Now;
                    age = currentDate.Year - birthDate.Year;
                    if (currentDate.Month < birthDate.Month || (currentDate.Month == birthDate.Month && currentDate.Day < birthDate.Day))
                    {
                        age--;
                    }
                }
                else
                {
                    ModelState.AddModelError("DateOfBirth", "Некорректный формат даты рождения");
                }
        
             }
            catch { ModelState.AddModelError("DateOfBirth", "Некорректный формат даты рождения"); }
                if (p != null)
                {
                    string str = p.FileName.Replace(" ", "_");
                    string str1 = str.Replace("-", "_");
                    // Путь к папке Files
                    string path = "/Coaches/" + str1; // имя файла

                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await p.CopyToAsync(fileStream); // копируем файл в поток
                    }
                    u.Photo = path;
                }
                u.Login = user.Login;
                u.Gender = user.Gender;
                u.Email = user.Email;
                u.Age = age;
                u.Phone = user.Phone;
                u.Name = user.Name;
                u.DateOfBirth = user.DateOfBirth;              
                await coachService.UpdateCoach(u);
                return View("YouChangedProfile");
            }
            return RedirectToAction("CoachProfile");
        }
        public async Task<IActionResult> Edit(int id) 
        {
            HttpContext.Session.SetString("path", Request.Path);
            CoachDTO coachdto = await coachService.GetCoach(id);
            if (coachdto != null)
            {
                await putSpecialities();
                await putPosts();
                return View("Edit", coachdto); 
            }

            return View("GetCoaches", "Coach");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CoachDTO coach, IFormFile? p)
        {
            HttpContext.Session.SetString("path", Request.Path);
            try
            {
                CoachDTO coachdto = await coachService.GetCoach(id);
                if (coachdto == null)
                {
                    return NotFound();
                }
                 
                if (ModelState.IsValid)
                {
                    if (p != null)
                    {
                        string str = p.FileName.Replace(" ", "_");
                        string str1 = str.Replace("-", "_");
                        // Путь к папке Files
                        string path = "/Coaches/" + str1; // имя файла

                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await p.CopyToAsync(fileStream); // копируем файл в поток
                        }
                        coachdto.Photo = path;
                    }
                        coachdto.Login = coach.Login;
                        coachdto.Gender = coach.Gender;
                        coachdto.Email = coach.Email;
                        coachdto.Age = coach.Age;
                        coachdto.Phone = coach.Phone;
                     //   coachdto.Photo = path;
                        coachdto.Name = coach.Name;
                        coachdto.DateOfBirth = coach.DateOfBirth; 
                        coachdto.Password = coach.Password;
                        coachdto.Description = coach.Description;
                        coachdto.PostId = coach.PostId;
                        coachdto.SpecialityId = coach.SpecialityId;
                        try
                        {
                            await coachService.UpdateCoach(coachdto);
                        }
                        catch { return View("Edit", coach); } 
                        return RedirectToAction("GetCoaches", "Coach");
                   // } 
                }
                return RedirectToAction("GetCoaches");
            }
            catch
            {
                return View("GetCoaches", "Coach");
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            HttpContext.Session.SetString("path", Request.Path);
            CoachDTO coach = await coachService.GetCoach(id);
            if (coach == null)
            {
                return NotFound();
            }
            return View(coach);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            CoachDTO coach = await coachService.GetCoach(id);
            if (coach == null)
            {
                return NotFound();
            }

            await coachService.DeleteCoach(id);
            return RedirectToAction("GetCoaches", "Coach");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCoachProfile(CoachDTO coach)
        {
            HttpContext.Session.SetString("path", Request.Path);
            return View(coach);
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmDeleteCoachProfile(int id)
        {
            CoachDTO coach = await coachService.GetCoach(id);
            if (coach == null)
            {
                return NotFound();
            }
            await coachService.DeleteCoach(id);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult BackToCoachProfile()
        {
            return RedirectToAction("CoachProfile");
        }
        public async Task putPosts()
        {
            HttpContext.Session.SetString("path", Request.Path);
            IEnumerable<PostDTO> p = await postService.GetAllPosts();
            ViewData["PostId"] = new SelectList(p, "Id", "Name");
        }
        public async Task putSpecialities()
        {
            HttpContext.Session.SetString("path", Request.Path);
            IEnumerable<SpecialityDTO> p = await specialityService.GetAllSpecialitys();
            ViewData["SpecialityId"] = new SelectList(p, "Id", "Name");
        }
        [HttpPost]
        public IActionResult ChangeCoachPassword(CoachDTO user)
        {
            return View("PutPassword", user);
        }
        [HttpPost]
        public async Task<IActionResult> PutPassword(int id, string pass)
        {
            CoachDTO u = await coachService.GetCoach(id);
            try
            {
                if (await coachService.CheckPasswordC(u, pass))
                {
                    CangePasswordModel m = new();
                    m.Id = u.Id;
                    return View("ChangePassword", m);
                }
            }
            catch { }
            return View("PutPassword", u);
        }
        public async Task<IActionResult> SaveNewPassword(CangePasswordModel m)
        {
            CoachDTO u = await coachService.GetCoach(m.Id);
            string pass = m.Password;
            if (!string.IsNullOrEmpty(pass) && u != null)
            {
                await coachService.ChangeCoachPassword(u, pass);
                return View("YouChangedPassword");
            }
            return View("ErrorChangedPassword");
        }
        public async Task<IActionResult> MyShedule()
        {
            try
            {
                string s = HttpContext.Session.GetString("Id");
                int id = Int32.Parse(s);
                CoachDTO p = await coachService.GetCoach(id);
                if (p != null)
                {
                    List<TrainingIndToSee> trainings = new();
                    IEnumerable<TrainingIndDTO> trInd = await trainingIndService.GetAllTrainingInds();
                    var sortedTrInd = trInd.OrderBy(dto => dto.Day).ThenBy(dto => dto.Time);
                    foreach (TrainingIndDTO tr in sortedTrInd)
                    {
                        if(tr.CoachId == id)
                        {
                            // TrI.Add(tr);
                            TrainingIndToSee training = new();
                            RoomDTO room = new RoomDTO();
                            room = await roomService.GetRoom(tr.RoomId);
                            training.Room = room;
                            training.Id = tr.Id;
                           training.DayName = Setday(tr.Day);
                            training.Day = tr.Day;
                            training.Time = tr.Time;
                            training.User = tr.UserName;
                            trainings.Add(training);
                        }
                    }

                    List<TrainingGrToSee> trainings2 = new();
                    IEnumerable<TrainingGroupDTO> trg = await trainingGroupService.GetAllTrainingGroups();
                    var sortedTrG = trg.OrderBy(dto => dto.Day).ThenBy(dto => dto.Time);
                    foreach (TrainingGroupDTO group in sortedTrG)
                    {
                        if (group.CoachId == id)
                        {
                            //TrG.Add(group);
                            TrainingGrToSee training = new();
                            RoomDTO room = new RoomDTO();
                            room = await roomService.GetRoom(group.RoomId);
                            training.Id= group.Id;
                            training.Room = room;
                            training.DayName = Setday(group.Day);
                            training.Day = group.Day;
                            training.Time = group.Time;
                            training.Group = await groupService.GetGroup(group.GroupId);
                           IEnumerable <UserDTO> users=await groupService.GetGroupUsers(group.GroupId);
                            training.Users = users.ToList();
                            trainings2.Add(training);
                        }
                    }
                    //MakeSheduleView m = new();
                    //m.trainingInd = TrI;
                    //m.traininggroup = TrG;
                    //return View(m);

                   TrainingsAllToSee all= new TrainingsAllToSee();
                    all.trInds = trainings;
                    all.trGrs = trainings2;
                    return View(all);
                }
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        String Setday(int day)
        {
            if (day == 0)
                return "Понедельник";
            if (day == 1)
                return "Вторник";
            if (day == 2)
                return "Среда";
            if (day == 3)
                return "Четверг";
            if (day == 4)
                return "Пятница";
            if (day == 5)
                return "Суббота";
            if (day == 6)
                return "Воскрксенье";
            return null;
        }

        public async Task<IActionResult> AllCoachesToSee()
        {
            var p = await coachService.GetAllCoaches();
            return View(p);
        }
        [HttpGet]
        public async Task<IActionResult> Room_Shedule()
        {
            IEnumerable<RoomDTO> r = await roomService.GetAllRooms();
            return View(r);
        }
        public IActionResult BackToRooms()
        {
            return RedirectToAction("Room_Shedule");
        }
        public async Task<IActionResult> Shedule(int RoomId)
        {
            RoomDTO room = await roomService.GetRoom(RoomId);
            if (room != null)
            {
                SheduleDTO shDto = null;
                try
                {
                    shDto = await sheduleService.GetShedule(room.sheduleId.Value);
                }
                catch { }
                MakeSheduleView m = new();
                m.room = room;
                if (shDto != null)
                {
                    m.times = new();
                    foreach (var t in shDto.timetables)
                    {
                        TimetableShow t1 = new();
                        t1.Id = t.Id;
                        if (t.TimesId.Count == 0)
                        {
                            string s = "Выходной";
                            t1.Times.Add(s);
                        }
                        else
                        {
                            foreach (int i in t.TimesId)
                            {
                                TimeTDTO td = await timeService.GetTimeT(i);
                                string st = td.StartTime + "/" + td.EndTime;
                                t1.Times.Add(st);
                            }
                        }
                        m.times.Add(t1);
                        IEnumerable<TrainingIndDTO> trInd = await trainingIndService.GetAllTrainingInds();
                        m.trainingInd = trInd.ToList();

                        IEnumerable<TrainingGroupDTO> trg = await trainingGroupService.GetAllTrainingGroups();
                        List<TrainingGrToSee> trg1 = new();
                        foreach (var tr in trg)
                        {
                            TrainingGrToSee train = new();
                            train.Id = tr.Id;
                            train.Group = await groupService.GetGroup(tr.GroupId);
                            train.Room = room;
                            train.Coach = await coachService.GetCoach(tr.CoachId);
                            IEnumerable<UserDTO> us = await groupService.GetGroupUsers(tr.GroupId);
                            train.Users = us.ToList();
                            train.Time = tr.Time;
                            train.Day = tr.Day;
                            trg1.Add(train);
                        }
                        m.traininggroup = trg1.ToList();
                    }
                }
                else
                {
                    m.message = "для зала не составлен график";
                }
                ViewBag.MyId = HttpContext.Session.GetString("Id");
                return View(m);
            }
            else { return RedirectToAction("Room_Shedule"); }
        }
    }
}
