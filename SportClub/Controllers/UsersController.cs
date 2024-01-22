using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportClub.Filters;
using SportClub.Models;
using SportClub.BLL.Interfaces;
using SportClub.BLL.DTO;
using SportClub.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using SportClub.BLL.Services;


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
        private readonly IGroup groupService;
        public UsersController(IGroup gr, IRoom room, IAdmin adm, IUser us, ICoach c, ISpeciality sp, IPost p, ITrainingInd tr, ITrainingGroup tg)
        {
            adminService = adm;
            userService = us;
            coachService = c;
            postService = p;
            specialityService = sp;
            roomService = room;
            trainingIndService = tr;
            trainingGroupService = tg;
            groupService = gr;
        }

        // GET: Users
        public async Task<IActionResult> GetCoaches()
        {
            /* return _context.Users != null ?
                         View(await _context.Users.ToListAsync()) :
                         Problem("Entity set 'SportClubContext.Users'  is null.");*/
            var p= await coachService.GetAllCoaches();
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
            int id =Int32.Parse(s);
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
        public async Task<IActionResult> EditClientProfile( UserDTO user)
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
            return View("PutPassword",user);
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
            if (!string.IsNullOrEmpty(pass) && u!=null)
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
                            training.Group = await groupService.GetGroup(group.GroupId);
                            IEnumerable<UserDTO> users = await groupService.GetGroupUsers(group.GroupId);
                            training.Users = users.ToList();
                        foreach (var item in users)
                        {
                            if(item.Id==id)
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
    }
}
