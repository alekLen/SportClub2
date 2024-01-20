
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Packaging.Signing;
using SportClub.BLL.DTO;
using SportClub.BLL.Interfaces;
using SportClub.BLL.Services;
using SportClub.DAL.Entities;
using SportClub.Models;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Xml;

namespace SportClub.Controllers
{
    public class TimeController : Controller
    {
        private readonly ITrainingGroup trainingGroupService;
        private readonly IGroup groupService;
        private readonly IAdmin adminService;
        private readonly IRoom roomService;
        private readonly IUser userService;
        private readonly ICoach coachService;
        private readonly ITime timeService;
        private readonly ITimetable timetableService;
        private readonly IShedule sheduleService;
        private readonly ISpeciality specialityService;
        private readonly ITrainingInd trainingIndService;
        private static List<TimeTDTO> timesT=new();
        private static List<TimetableDTO> timetables = new();
        //private static List<TrainingIndDTO> trainingsInd = new();
        public TimeController(ITrainingGroup tg, IGroup g, IShedule sh,IRoom room,IAdmin adm, IUser us, ICoach c, ISpeciality sp, ITime t, ITimetable timetableService, ITrainingInd tr)
        {
            adminService = adm;
            userService = us;
            coachService = c;
            timeService = t;
            specialityService = sp;
            roomService=room;
            this.timetableService = timetableService;
            sheduleService = sh;
            trainingIndService = tr;
            groupService = g;
            trainingGroupService = tg;
        }
        [HttpGet]
        public async Task<IActionResult> AddTimeT()
        {
            await PutTimes();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddTimeT(string Start, string End)
        {          
            if (Start != null && End != null)
            {
                if (CheckTime(Start, End))
                {
                    try

                    {
                        await timeService.AddTimeT(Start, End);
                    }
                    catch { }
                }
            }
            else
            {
                ModelState.AddModelError("", "Заполните время начала и окончания тренировки!");
            }
            await PutTimes();
            return View();
        }    
        public async Task PutTimes()
        {
            IEnumerable<TimeTDTO> p = await timeService.GetAllTimeTs();
            
            IEnumerable<TimeTDTO> p2 = p.OrderBy(x => double.Parse(x.StartTime.Split(':')[0]+","+ x.StartTime.Split(':')[1]));
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
        public async Task<IActionResult> EditTime(int Id)
        {
           // HttpContext.Session.SetString("path", Request.Path);
            TimeTDTO p = await timeService.GetTimeT(Id);
            if (p != null)
            {
                if (TempData.ContainsKey("ErrorMessage"))
                {
                    // Retrieve the error message
                    ViewBag.ErrorMessage = TempData["ErrorMessage"] as string;
                }
                return View("EditTimeT",p);
            }
            await PutTimes();
            return View("AddTimeT");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTime( TimeTDTO t, string Start, string End)
        {
          //  HttpContext.Session.SetString("path", Request.Path);
            try
            {
                TimeTDTO p = await timeService.GetTimeT(t.Id);
                if (p == null)
                {
                    await PutTimes();
                    return View("AddTimeT");
                }
                if (CheckTime(Start, End))
                {
                    p.StartTime = Start;
                    p.EndTime = End;
                    await timeService.UpdateTimeT(p);
                    try
                    {
                        TempData.Remove("ErrorMessage");
                    }
                    catch { }
                }
                else
                {
                    TempData["ErrorMessage"] = "некорректное редактирование";
                    return RedirectToAction("EditTime", new { Id = t.Id });
                   
                }

                return RedirectToAction("AddTimeT");
            }
            catch
            {
                await PutTimes();
                return View("AddTimeT");
            }
        }
        public async Task<IActionResult> DeleteTime(int Id)
        {
            // HttpContext.Session.SetString("path", Request.Path);
            TimeTDTO p = await timeService.GetTimeT(Id);
            if (p != null)
            {
               
                return View("DeleteTimeT", p);
            }
            await PutTimes();
            return View("AddTimeT");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDeleteTime(TimeTDTO t)
        {
            //  HttpContext.Session.SetString("path", Request.Path);
            try
            {
                TimeTDTO p = await timeService.GetTimeT(t.Id);
                if (p == null)
                {
                    await PutTimes();
                    return View("AddTimeT");
                }               
                await timeService.DeleteTimeT(p.Id);
                await PutTimes();
                return View("AddTimeT");
            }
            catch
            {
                await PutTimes();
                return View("AddTimeT");
            }
        }
        [HttpGet]
      
         public async Task<IActionResult> AddTimetable()
         {           
             await PutTimes();
             return View();
         }
         public async Task<IActionResult> AddTimesToTable(int id)
         {
             TimeTDTO p = await timeService.GetTimeT(id);
             timesT.Add(p);
             await PutTimes();
              PutTimesToTable();
             return View("AddTimetable");
         }
         [HttpPost]
         public async Task<IActionResult> AddTimeTable()
         {
             if (timesT.Count > 0)
             {
                 TimetableDTO t = new();
                 foreach (var time in timesT)
                     t.TimesId.Add(time.Id);
                 await timetableService.AddTimetable(t);
                 timesT.Clear();
                return RedirectToAction("GetAllTimetable");
            }
             else
             {
                await PutTimes();
                PutTimesToTable();
                 return View();
             }
         }
        [HttpGet]
        public async Task<IActionResult> Cancel()
        {
            if (timesT.Count > 0)
            {              
                timesT.RemoveAt(timesT.Count-1);
                await PutTimes();
                PutTimesToTable();
                return View("AddTimetable");              
            }
            else
            {
                return Redirect("/Home/Index");
            }
        }
        [HttpGet]
        public  IActionResult Exit()
        {
            timesT.Clear();
            return Redirect("/Home/Index");            
        }
        [HttpPost]
        public IActionResult ExitTime()
        {
            timesT.Clear();
            return RedirectToAction("AddTimeT");
        }
        [HttpGet]
        public IActionResult Back()
        {         
            return RedirectToAction("AddTimetable");
        }
        [HttpGet]
        public IActionResult BackToAll()
        {
            return RedirectToAction("GetAllTimetable");
        }
        public void PutTimesToTable()
         {

             List<TimeShow> p1 = new();
             IEnumerable<TimeTDTO> p2 = timesT.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
             foreach (var t in p2)
             {
                 TimeShow ts = new()
                 {
                     Id = t.Id,
                     Time = t.StartTime + "/" + t.EndTime
                 };
                 p1.Add(ts);
             }
             ViewData["TimetableId"] = new SelectList(p1, "Id", "Time");
         }
        [HttpGet]
        public async Task <IActionResult> ChoseRomm()
        {
            IEnumerable<RoomDTO> r = await roomService.GetAllRooms();
            ViewData["RoomsId"] = new SelectList(r, "Id", "Name");
            return View("ChoseRomm");
        }
        [HttpPost]
        public async Task<IActionResult> GetAllTimetable(int Id)
        {
            List<TimetableShow> ts = new();
            IEnumerable<TimetableDTO> p = await timetableService.GetAllTimetables();
            List<TimeTDTO> pp = new();
            foreach (var t in p)
            {
                TimetableShow t1 = new();
                t1.Id = t.Id;
                foreach (int i in t.TimesId)
                {
                    TimeTDTO td = await timeService.GetTimeT(i);
                    pp.Add(td);
                }
                IEnumerable<TimeTDTO> p2 = pp.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                foreach (var i in p2)
                {
                    string st = i.StartTime + "/" + i.EndTime;
                    t1.Times.Add(st);
                }
                ts.Add(t1);
                pp.Clear();
            }
            RoomDTO r=await roomService.GetRoom(Id);
            MakeSheduleView m = new();
            m.times = ts;
            m.room = r;
            return View("GetTimetables", m);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTimetable()
        {
            List<TimetableShow> ts = new();
            IEnumerable<TimetableDTO> p = await timetableService.GetAllTimetables();
            List<TimeTDTO> pp=new ();
            foreach (var t in p)
            {
                TimetableShow t1 = new();
                t1.Id = t.Id;
                foreach (int i in t.TimesId)
                {
                    TimeTDTO td = await timeService.GetTimeT(i);
                   pp.Add(td);                
                }
                IEnumerable<TimeTDTO> p2 = pp.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                foreach (var i in p2)
                {                  
                    string st = i.StartTime + "/" + i.EndTime;
                    t1.Times.Add(st);
                }
                ts.Add(t1);
                pp.Clear();
            }
            MakeSheduleView m = new();
            m.times = ts;
            return View("GetTimetablesToSee",m);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteTimetable(int id)
        {
            if (id != 0)
            {
                TimetableDTO tt = await timetableService.GetTimetable(id);
                List<TimetableShow> ts = new();              
                List<TimeTDTO> pp = new();
                TimetableShow tss = new();
                tss.Id = tt.Id;
                    foreach (var i in tt.TimesId)
                    {
                        TimeTDTO td = await timeService.GetTimeT(i);
                        pp.Add(td);
                    }
                    IEnumerable<TimeTDTO> p2 = pp.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                    foreach (var i in p2)
                    {
                        string st = i.StartTime + "/" + i.EndTime;
                        tss.Times.Add(st);
                    }
                    ts.Add(tss);                             
                MakeSheduleView m = new();
                m.times = ts;
                return View(m);
            }
            return Redirect("GetAllTimetable");
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmDeleteTimetable(int id)
        {
            if (id != 0)
            {
                await timetableService.DeleteTimetable(id);

            }
            return Redirect("GetAllTimetable");
        }

        [HttpPost]
        public async Task<IActionResult> AddTimetableToShedule(int id, int roomId)
        {
            if (id != 0)
            {
                TimetableDTO tt = await timetableService.GetTimetable(id);
                if (timetables.Count < 7)
                    timetables.Add(tt);
            }
            else
            {
                TimetableDTO tt1=new();
                tt1.Id= id;
                timetables.Add(tt1);
            }
            RoomDTO r = await roomService.GetRoom(roomId);
            List<TimetableShow> ts = new();
            IEnumerable<TimetableDTO> p = await timetableService.GetAllTimetables();
            foreach (var t in p)
            {
                TimetableShow t1 = new();
                t1.Id = t.Id;

                foreach (int i in t.TimesId)
                {
                    TimeTDTO td = await timeService.GetTimeT(i);
                    string st = td.StartTime + "/" + td.EndTime;
                    t1.Times.Add(st);
                }
                ts.Add(t1);
            }
            List<TimetableShow> ts2 = new();        
            foreach (var t in timetables)
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
                ts2.Add(t1);
            }

            MakeSheduleView m = new();
            m.times = ts;
            m.room = r;
            m.timesAdded = ts2;
            return View("GetTimetables", m);
        }

        //--------------------------------------------------------------------
        //[HttpPost]
        //public async Task<IActionResult> AddIndtrainingToSchedule(int id, int roomId)
        //{
        //    if(id != 0)
        //    {
        //        TrainingIndDTO tr = await trainingIndService.GetTrainingInd(id);
        //        trainingsInd.Add(tr);
        //    }
        //    else
        //    {
        //        TrainingIndDTO tr = new();
        //        tr.Id = id;
        //        trainingsInd.Add(tr);
        //    }
        //    RoomDTO room = await roomService.GetRoom(roomId);
        //    List<TrainingIndDTO> lTrInd = new();
        //    IEnumerable<TrainingIndDTO> trInd = await trainingIndService.GetAllTrainingInds();
        //    foreach (var t in trInd)
        //    {
        //        TrainingIndDTO t_ = new();
        //        t_.RoomId = t.RoomId;
        //        t_.CoachId = t.CoachId;
        //        t_.Time = t.Time;
        //        lTrInd.Add(t_);
        //    }
        //    MakeSheduleView m = new();
        //    m.trainingInd = lTrInd;
        //    return View("GetTimetables", m);
        //}
        //----------------------------------------------------------------------

        [HttpPost]
        public async Task<IActionResult> SaveShedule( int rId)
        {
            SheduleDTO shedule = new SheduleDTO();
            RoomDTO r = await roomService.GetRoom(rId);
            foreach (var t in timetables)
            {
                
                shedule.timetables.Add(t);
            }
           await sheduleService.AddShedule(shedule,r);
            timetables.Clear();
            return RedirectToAction("Room_Shedule");
        }
        [HttpGet]
        public async Task<IActionResult> Room_Shedule()
        {
            IEnumerable<RoomDTO> r = await roomService.GetAllRooms();
            ViewData["RoomsId"] = new SelectList(r, "Id", "Name");
            return View();
        }
        
        public async Task<IActionResult> RoomWithShedule(/*int RoomId, int CoachId, string Time*/int Id)
        {
            RoomDTO room = await roomService.GetRoom(Id);
            SheduleDTO shDto = null;
            try
            {
                shDto = await sheduleService.GetShedule(room.sheduleId.Value);
            }
            catch { }
            MakeSheduleView m = new();
            m.room = room;
           // m.shedule = room.sheduleId;
            if (shDto != null)
            {
                m.times = new();
                foreach (var t in shDto.timetables)
                {
                    TimetableShow t1 = new();
                    t1.Id = t.Id;
                    /* foreach (int i in t.TimesId)
                     {
                         TimeTDTO td = await timeService.GetTimeT(i);
                         string st = td.StartTime + "/" + td.EndTime;
                         t1.Times.Add(st);
                     }*/
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
                }
                //foreach(var t in shDto.trainingInd)
                //{
                //    TrainingIndDTO trInd = new();
                //    trInd.RoomId = t.RoomId;
                //    trInd.CoachId = t.CoachId;
                //    trInd.Time = t.Time;
                //    if(t != null)
                //    {
                //        m.trainingInd.Add(trInd);
                //    }
                //}
            }
            else
            {
                m.message = "для зала не составлен график";
            }
            return View(m);
        }
        [HttpPost]
        public async Task<IActionResult> AddIndTraining(int day, int roomId, string time, string roomName)
        {
            TrainingIndDTO training = new();
            training.RoomId = roomId;
            training.Time = time;
            training.Day = day;
            training.RoomName=roomName;
            training.UserId = 0;
            if (day == 0) training.DayName = "Понедельник";
            else if (day == 1) training.DayName = "Вторник";
            else if (day == 2) training.DayName = "Среда";
            else if (day == 3) training.DayName = "Четверг";
            else if (day == 4) training.DayName = "Пятница";
            else if (day == 5) training.DayName = "Суббота";
            else if (day == 6) training.DayName = "Воскресенье";
            IEnumerable<CoachDTO> p = await coachService.GetAllCoaches();
            ViewData["CoachId"] = new SelectList(p, "Id", "Name");
            IEnumerable<UserDTO> p_ = await userService.GetAllUsers();
            ViewData["UserId"] = new SelectList(p_, "Id", "Name");
            return View(training);
        }
        [HttpPost]
        public async Task<IActionResult> ToAddTrainingInd(int day, int roomId, string time, string roomName, int coachId,int? userId, string? userName)
        {
            TrainingIndDTO tr = new();
            tr.CoachId = coachId;
            tr.RoomId = roomId;
            tr.RoomName = roomName;
            tr.Day = day;
            tr.Time = time;
            tr.UserId = userId;
            //tr.UserName = userName;
           
            await trainingIndService.AddTrainingInd(tr);
            return RedirectToAction("RoomWithShedule", /*new { RoomId = tr.RoomId, CoachId = tr.CoachId, Time = tr.Time }*/new { Id = roomId });
        }
        [HttpPost]
        public async Task<IActionResult> AddUserToTrainingInd(int day, int roomId, string time, string roomName, int coachId)
        {
            TrainingIndDTO tr = new();
            tr.CoachId = coachId;
            tr.RoomId = roomId;
            tr.RoomName = roomName;
            tr.Day = day;
            tr.Time = time;
            IEnumerable<UserDTO> p = await userService.GetAllUsers();
            ViewData["UserId"] = new SelectList(p, "Id", "Name");

            await trainingIndService.AddTrainingInd(tr);
            return View(tr);
        }
        [HttpPost]
        public async Task<IActionResult> AddingToTrainingInd(int day, int roomId, string time, string roomName, int coachId,int userId)
        {
            TrainingIndDTO tr = new();
            tr.CoachId = coachId;
            tr.RoomId = roomId;
            tr.RoomName = roomName;
            tr.Day = day;
            tr.Time = time;
            if (userId != 0)
            {
                UserDTO user = await userService.GetUser(userId);
                tr.UserId = user.Id;
                tr.UserName = user.Name;
            }
           
            await trainingIndService.AddTrainingInd(tr);
            return View("AddIndTraining",tr);
        } 
        private bool CheckTime(string Start, string End)
        {
            string pattern = @"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$";

            if (Regex.IsMatch(Start, pattern) && Regex.IsMatch(End, pattern))
            {

                string[] str1 = Start.Split(':');
                string[] str2 = End.Split(':');
                if (int.Parse(str1[0]) < int.Parse(str2[0]) || (int.Parse(str1[0]) == int.Parse(str2[0]) && int.Parse(str1[1]) < int.Parse(str2[1])))
                {
                    if ((int.Parse(str1[0]) == int.Parse(str2[0]) && (int.Parse(str2[1]) - int.Parse(str1[1])) < 30))
                    {
                        ModelState.AddModelError("", "Время тренировки не может быть короче 30 минут ");
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    ModelState.AddModelError("", "время окончаня должно быть позже, чем время начала ");

                }
            }
            else
            {
                ModelState.AddModelError("", "Введите время в формате 00:00 ");
            }
            return false;
        }
        [HttpPost]
        public async Task<IActionResult> AddTrainingGroup(int day, int roomId, string time, string roomName)
        {
            GroupAndTrainingGroup gatg = new GroupAndTrainingGroup();
            
            TrainingGroupDTO training = new();
            training.RoomId = roomId;
            training.Time = time;
            training.Day = day;
            training.RoomName = roomName;
            //training.UsersId = new List<UserDTO>();
            if (day == 0) training.DayName = "Понедельник";
            else if (day == 1) training.DayName = "Вторник";
            else if (day == 2) training.DayName = "Среда";
            else if (day == 3) training.DayName = "Четверг";
            else if (day == 4) training.DayName = "Пятница";
            else if (day == 5) training.DayName = "Суббота";
            else if (day == 6) training.DayName = "Воскресенье";
            gatg.trgroup = training;
            IEnumerable<CoachDTO> p = await coachService.GetAllCoaches();
            ViewData["CoachId"] = new SelectList(p, "Id", "Name");
            IEnumerable<GroupDTO> p_ = await groupService.GetAllGroups();
            ViewData["GroupId"] = new SelectList(p_, "Id", "Name");
            return View(gatg);
        }
        [HttpPost]
        public async Task<IActionResult> ToAddTrainingGroup(int day, int roomId, string time, string roomName, int coachId/*, int? userId, string? userName*/)
        {
           
            TrainingGroupDTO tr = new();
            tr.CoachId = coachId;
            tr.RoomId = roomId;
            tr.RoomName = roomName;
            tr.Day = day;
            tr.Time = time;
            //tr.UsersId = userId;
            //tr.UserName = userName;

            await trainingGroupService.AddTrainingGroup(tr);
            return RedirectToAction("RoomWithShedule", new { Id = roomId });
        }
    }
}
