using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportClub.Filters;
using SportClub.Models;
using SportClub.BLL.Interfaces;
using SportClub.BLL.DTO;
using SportClub.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using SportClub.BLL.Services;
using Azure.Core;


namespace SportClub.Controllers
{
    [Culture]
    public class UsersController : Controller
    {
        private readonly IAdmin adminService;
        private readonly IUser userService;
        private readonly ICoach coachService;
        private readonly IPost postService;
        private readonly ISpeciality specialityService;
        private readonly IRoom roomService;
        private readonly ITrainingInd trainingIndService;
        private readonly ITrainingGroup trainingGroupService;
        //private readonly IGroup groupService;
        private readonly IShedule sheduleService;
        private readonly ITime timeService;


        public UsersController(ITime t, IShedule sh, /*IGroup gr,*/ IRoom room, IAdmin adm, IUser us, ICoach c, ISpeciality sp, IPost p, ITrainingInd tr, ITrainingGroup tg)
        {
            adminService = adm;
            userService = us;
            coachService = c;
            postService = p;
            specialityService = sp;
            roomService = room;
            trainingIndService = tr;
            trainingGroupService = tg;
            //groupService = gr;
            sheduleService = sh;
            timeService = t;
        }

        // GET: Users
        public async Task<IActionResult> GetCoaches()
        {
            /* return _context.Users != null ?
                         View(await _context.Users.ToListAsync()) :
                         Problem("Entity set 'SportClubContext.Users'  is null.");*/
            var p = await coachService.GetAllCoaches();
            await putPosts();
            await putSpecialities();
            return View(p);
        }
        public async Task<IActionResult> GetClients()
        {
            /* return _context.Users != null ?
                         View(await _context.Users.ToListAsync()) :
                         Problem("Entity set 'SportClubContext.Users'  is null.");*/
            var p = await userService.GetAllUsers();
            await putPosts();
            await putSpecialities();
            return View(p);
        }

        public async Task<IActionResult> ClientProfile()
        {
            string s = HttpContext.Session.GetString("Id");
            int id = Int32.Parse(s);
            UserDTO p = await userService.GetUser(id);
            return View(p);
        }
        //public async Task<IActionResult> AdminProfile()
        //{
        //    try
        //    {
        //        string s = HttpContext.Session.GetString("Id");
        //        int id = Int32.Parse(s);
        //        AdminDTO p = await adminService.GetAdmin(id);
        //        return View(p);
        //    }
        //    catch { return View("Index", "Home"); }
        //}
        [HttpPost]
        public async Task<IActionResult> CoachProfile()
        {
            string s = HttpContext.Session.GetString("Id");
            int id = Int32.Parse(s);
            CoachDTO p = await coachService.GetCoach(id);
            return View(p);
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
        public async Task<IActionResult> EditClientProfile(UserDTO user)
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
            UserDTO u = await userService.GetUser(user.Id);
            if (u != null)
            {
                u.Login = user.Login;
                u.Gender = user.Gender;
                u.Email = user.Email;
                u.Age = age;
                u.Phone = user.Phone;
                u.Name = user.Name;
                //  u.Surname = user.Surname;
                // u.Dopname = user.Dopname;
                u.DateOfBirth = user.DateOfBirth;
                await userService.UpdateUser(u);
                return View("YouChangedProfile");
            }
            return RedirectToAction("ClientProfile");
        }
        [HttpPost]
        public IActionResult ChangeClientPassword(UserDTO user)
        {
            return View("PutPassword", user);
        }
        [HttpPost]
        public async Task<IActionResult> PutPassword(int id, string pass)
        {
            UserDTO u = await userService.GetUser(id);
            if (await userService.CheckPasswordU(u, pass))
            {
                CangePasswordModel m = new();
                m.Id = u.Id;
                return View("ChangePassword", m);
            }
            return View("PutPassword");
        }
        public async Task<IActionResult> SaveNewPassword(CangePasswordModel m)
        {
            UserDTO u = await userService.GetUser(m.Id);
            string pass = m.Password;
            if (!string.IsNullOrEmpty(pass) && u != null)
            {
                await userService.ChangeUserPassword(u, pass);
                return RedirectToAction("ClientProfile");
            }
            return RedirectToAction("ClientProfile");
        }

        public async Task<IActionResult> Edit(int id)
        {
            HttpContext.Session.SetString("path", Request.Path);
            UserDTO us = await userService.GetUser(id);
            if (us != null)
            {
                return View("Edit", us);
            }

            return View("GetClients"/*, "User"*/);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserDTO user)
        {
            HttpContext.Session.SetString("path", Request.Path);
            try
            {
                UserDTO usdt = await userService.GetUser(id);
                if (usdt == null)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    usdt = user;
                    //usdt.Age = user.Age;
                    //usdt.Gender = user.Gender;
                    //usdt.Login = user.Login;
                    //usdt.Phone = user.Phone;
                    //usdt.Name = user.Name;
                    //usdt.Email = user.Email;
                    //usdt.DateOfBirth = user.DateOfBirth;
                    //usdt.Password = user.Password;

                    try
                    {
                        await userService.UpdateUser(usdt);
                    }
                    catch { return View("Edit", user); }
                }
                return RedirectToAction("GetClients"/*, "Users"*/);
            }
            catch
            {
                return View("GetClients");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteClientProfile(UserDTO user)
        {
            HttpContext.Session.SetString("path", Request.Path);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmDeleteClientProfile(int id)
        {
            UserDTO user = await userService.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            await userService.DeleteUser(id);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult BackToClientProfile()
        {
            return RedirectToAction("ClientProfile");
        }
        public async Task<IActionResult> Delete(int id)
        {
            HttpContext.Session.SetString("path", Request.Path);
            UserDTO user = await userService.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            UserDTO user = await userService.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }

            await userService.DeleteUser(id);
            return RedirectToAction("GetClients");
        }
        public async Task<IActionResult> Details(int id)
        {
            HttpContext.Session.SetString("path", Request.Path);
            UserDTO user = await userService.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        public async Task<IActionResult> MyShedule()
        {
            try
            {
                string s = HttpContext.Session.GetString("Id");
                int id = Int32.Parse(s);
                UserDTO p = await userService.GetUser(id);
                if (p != null)
                {
                    List<TrainingIndToSee> trainings = new();
                    IEnumerable<TrainingIndDTO> trInd = await trainingIndService.GetAllTrainingInds();
                    var sortedTrInd = trInd.OrderBy(dto => dto.Day).ThenBy(dto => dto.Time);
                    foreach (TrainingIndDTO tr in sortedTrInd)
                    {
                        if (tr.UserId == id)
                        {
                            // TrI.Add(tr);
                            TrainingIndToSee training = new();
                            RoomDTO room = new RoomDTO();
                            room = await roomService.GetRoom(tr.RoomId);
                            training.Room = room;
                            training.Coach = await coachService.GetCoach(tr.CoachId.Value);
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
                        //TrG.Add(group);
                        TrainingGrToSee training = new();
                        RoomDTO room = new RoomDTO();
                        room = await roomService.GetRoom(group.RoomId);
                        training.Id = group.Id;
                        training.Room = room;
                        training.Coach = await coachService.GetCoach(group.CoachId);
                        training.DayName = Setday(group.Day);
                        training.Day = group.Day;
                        training.Time = group.Time;
                        //training.Group = await groupService.GetGroup(group.GroupId);
                        //IEnumerable<UserDTO> users = await groupService.GetGroupUsers(group.GroupId);
                        training.Number = group.Number;
                        IEnumerable<UserDTO> users = await trainingGroupService.GetTrainingGroupUsers(group.Id);

                        training.Users = users.ToList();
                        foreach (var item in users)
                        {
                            if (item.Id == id)
                                trainings2.Add(training);
                        }


                    }

                    TrainingsAllToSee all = new TrainingsAllToSee();
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
                            train.Room = room;
                            train.Coach = await coachService.GetCoach(tr.CoachId);

                            //train.Group = await groupService.GetGroup(tr.GroupId);
                            //IEnumerable<UserDTO> us = await groupService.GetGroupUsers(tr.GroupId);
                            train.Number = tr.Number;
                            IEnumerable<UserDTO> us = await trainingGroupService.GetTrainingGroupUsers(tr.Id);

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
                ViewBag.MyId= HttpContext.Session.GetString("Id");
                return View(m);
            }
            else { return RedirectToAction("Room_Shedule"); }
        }


        //new
        //------------------------------------------------------------------

        public async Task putTrGroupUsers(int id, bool flag)
        {
            HttpContext.Session.SetString("path", Request.Path);
            IEnumerable<UserDTO> p = await trainingGroupService.GetTrainingGroupUsers(id);
            if (flag)//true
            {
                foreach (var item in p)
                {
                    item.Name += "~";
                }
            }
            ViewData["UserstableId"] = new SelectList(p, "Id", "Name");
        }
        public async Task putUsers()
        {
            HttpContext.Session.SetString("path", Request.Path);
            IEnumerable<UserDTO> p = await userService.GetAllUsers();
            ViewData["UsersList"] = new SelectList(p, "Id", "Name");
        }

        public async Task<IActionResult> AddUsersToTrainingGroup(int trgroupId, int roomId)
        {
            HttpContext.Session.SetString("path", Request.Path);
            TrainingGroupDTO trgroupdto = await trainingGroupService.GetTrainingGroup(trgroupId);
            HttpContext.Session.SetInt32("roomId", trgroupdto.RoomId);
            if (trgroupdto != null)
            {
                await putTrGroupUsers(trgroupdto.Id, true);
                await putUsers();
                return View("AddUsersToTrainingGroup", trgroupdto);
            }
            return View("Index", "Home");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUsersToTrainingGroup(int trgroupId, int[] UsersList)
        {
            HttpContext.Session.SetString("path", Request.Path);
            try
            {
                TrainingGroupDTO trgroupdto = await trainingGroupService.GetTrainingGroup(trgroupId);//trgroupId
                if (trgroupdto == null)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {

                    foreach (var i in UsersList)
                    {
                        trgroupdto.UsersId.Add(await userService.GetUser(i));
                    }

                    try
                    {
                        await trainingGroupService.UpdateTrainingGroup(trgroupdto);
                    }
                    catch { return View("AddUsersToTrainingGroup"/*, group*/); }
                    return RedirectToAction("RoomWithShedule", "Time", new { Id = HttpContext.Session.GetInt32("roomId") });
                }
                return RedirectToAction("RoomWithShedule", "Time", new { Id = HttpContext.Session.GetInt32("roomId") });
            }
            catch
            {
                return View("Index", "Home");
            }
        }

        public async Task<IActionResult> AddUserToTrainingGroup(int groupId, int roomId)
        {
            HttpContext.Session.SetString("path", Request.Path);
            TrainingGroupDTO trgroupdto = await trainingGroupService.GetTrainingGroup(groupId);
            HttpContext.Session.SetInt32("roomId", trgroupdto.RoomId);

            int userId = Convert.ToInt32(HttpContext.Session.GetString("Id"));

            if (trgroupdto != null)
            {
                trgroupdto.UsersId.Add(await userService.GetUser(userId));
                await trainingGroupService.UpdateTrainingGroup(trgroupdto);

                return RedirectToAction("Room_Shedule", "Users");
            }
            return View("Index", "Home");
        }


        public async Task<IActionResult> DeleteUsersInTrainingGroup(int Id)
        {
            HttpContext.Session.SetString("path", Request.Path);
            TrainingGroupDTO trgroupdto = await trainingGroupService.GetTrainingGroup(Id);
            HttpContext.Session.SetInt32("roomId", trgroupdto.RoomId);
            if (trgroupdto != null)
            {
                await putTrGroupUsers(trgroupdto.Id, false);
                return View("DeleteUsersInTrainingGroup", trgroupdto);
            }
            return View("Index", "Home");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUsersInTrainingGroup(int trgroupId, int UsersId)
        {
            HttpContext.Session.SetString("path", Request.Path);
            try
            {
                TrainingGroupDTO trgroupdto = await trainingGroupService.GetTrainingGroup(trgroupId);
                if (trgroupdto == null || UsersId == null)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    for (int i = 0; i < trgroupdto.UsersId.Count; i++)
                    {
                        if (trgroupdto.UsersId[i].Id == UsersId)
                        {
                            trgroupdto.UsersId.RemoveAt(i);
                        }
                    }

                    try
                    {
                        await trainingGroupService.UpdateTrainingGroup(trgroupdto);
                    }
                    catch { return View("DeleteUserInTrainingGroup"/*, group*/); }
                    return RedirectToAction("RoomWithShedule", "Time", new { Id = HttpContext.Session.GetInt32("roomId") });
                }
                return RedirectToAction("RoomWithShedule", "Time", new { Id = HttpContext.Session.GetInt32("roomId") });
            }
            catch
            {
                return View("Index", "Home");
            }
        }


        [HttpGet]
        public IActionResult DeleteUserAction()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmDeleteUserAction(int id)
        {
            HttpContext.Session.SetString("path", Request.Path);
            try
            {
                TrainingGroupDTO trgroupdto = await trainingGroupService.GetTrainingGroup(id);

                int UserId = Convert.ToInt32(HttpContext.Session.GetString("Id"));
                if (trgroupdto == null || UserId == null)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    for (int i = 0; i < trgroupdto.UsersId.Count; i++)
                    {
                        if (trgroupdto.UsersId[i].Id == UserId)
                        {
                            trgroupdto.UsersId.RemoveAt(i);
                        }
                    }

                    try
                    {
                        await trainingGroupService.UpdateTrainingGroup(trgroupdto);
                    }
                    catch
                    {
                        return Json(false);
                    }
                    return Json(true);
                }
                return Json(false);
            }
            catch
            {
                return Json(false);
            }
        }

        [HttpGet]
        public IActionResult DeleteUserInTrGroupAction()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmDeleteUserInTrGroupAction(int id, int UserId)
        {
            HttpContext.Session.SetString("path", Request.Path);
            try
            {
                TrainingGroupDTO trgroupdto = await trainingGroupService.GetTrainingGroup(id);
                if (trgroupdto == null)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    for (int i = 0; i < trgroupdto.UsersId.Count; i++)
                    {
                        if (trgroupdto.UsersId[i].Id == UserId)
                        {
                            trgroupdto.UsersId.RemoveAt(i);
                        }
                    }

                    try
                    {
                        await trainingGroupService.UpdateTrainingGroup(trgroupdto);
                    }
                    catch
                    {
                        return Json(false);
                    }
                    return Json(true);
                }
                return Json(false);
            }
            catch
            {
                return Json(false);
            }
        }
    }


}