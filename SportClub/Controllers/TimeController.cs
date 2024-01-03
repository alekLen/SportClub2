
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Packaging.Signing;
using SportClub.BLL.DTO;
using SportClub.BLL.Interfaces;
using SportClub.BLL.Services;
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
        public TimeController(IShedule sh,IRoom room,IAdmin adm, IUser us, ICoach c, ISpeciality sp, ITime t, ITimetable timetableService)
        {
            adminService = adm;
            userService = us;
            coachService = c;
            timeService = t;
            specialityService = sp;
            roomService=room;
            this.timetableService = timetableService;
            sheduleService = sh;
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
            //string pattern = @"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$";

            //if (Regex.IsMatch(Start, pattern) && Regex.IsMatch(End, pattern)) {

            //    string[] str1 = Start.Split(':');
            //    string[] str2 = End.Split(':');
            //    if (int.Parse(str1[0]) < int.Parse(str2[0]) || (int.Parse(str1[0]) == int.Parse(str2[0]) && int.Parse(str1[1]) < int.Parse(str2[1]))) {
            //        if ((int.Parse(str1[0]) == int.Parse(str2[0]) && (int.Parse(str2[1]) - int.Parse(str1[1])) < 30))
            //        {
            //            ModelState.AddModelError("", "Время тренировки не может быть короче 30 минут ");
            //        }
            //        else
            //        {
            //            try

            //            {
            //                await timeService.AddTimeT(Start, End);
            //            }
            //            catch { }
            //        }
            //    }
            //    else
            //    {
            //        ModelState.AddModelError("", "время окончаня должно быть позже, чем время начала ");

            //    }
            //}
            //else
            //{
            //    ModelState.AddModelError("", "Введите время в формате 00:00 ");
            //}
            if (CheckTime(Start, End))
            {
                try

                {
                    await timeService.AddTimeT(Start, End);
                }
                catch { }
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
                 return Redirect("/Home/Index"); 
             }
             else
             {
                await PutTimes();
                PutTimesToTable();
                 return View();
             }
         }
        [HttpPost]
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
        [HttpPost]
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
            foreach(var t in p)
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
           // IEnumerable<RoomDTO> r= await roomService.GetAllRooms();
           RoomDTO r=await roomService.GetRoom(Id);
            MakeSheduleView m = new();
            m.times = ts;
            m.room = r;
           // ViewData["RoomsId"] = new SelectList(r, "Id", "Name");
            // return View("GetTimetables",ts);
            return View("GetTimetables", m);
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
        [HttpPost]
        public async Task<IActionResult> RoomWithShedule(int Id)
        {
            RoomDTO room =await roomService.GetRoom(Id);
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
                }
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
            return View(training);
        }
        [HttpPost]
        public async Task<IActionResult> ToAddTrainingInd(int day, int roomId, string time, string roomName, int coachId,int userId,string userName)
        {
            TrainingIndDTO tr = new();
            tr.CoachId = coachId;
            tr.RoomId = roomId;
            tr.RoomName = roomName;
            tr.Day = day;
            tr.Time = time;
            tr.UserId = userId;
            tr.UserName = userName;
           
            await trainingIndService.AddTrainingInd(tr);
            return View("RoomwithShedule");
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
    }
}
