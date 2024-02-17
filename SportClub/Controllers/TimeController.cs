
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Packaging.Signing;
using SportClub.BLL.DTO;
using SportClub.BLL.Interfaces;
using SportClub.BLL.Services;
using SportClub.DAL.Entities;
using SportClub.Filters;
using SportClub.Models;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml;

namespace SportClub.Controllers
{
    [Culture]
    public class TimeController : Controller
    {
        private readonly ITrainingGroup trainingGroupService;
        //private readonly IGroup groupService;
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
       
 
        public TimeController(ITrainingGroup tg, IShedule sh,IRoom room,IAdmin adm, IUser us, ICoach c, ISpeciality sp, ITime t, ITimetable timetableService, ITrainingInd tr)
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
            //groupService = g;
            trainingGroupService = tg;
        }
        [HttpGet]
        public async Task<IActionResult> AddTimeT()
        {
            HttpContext.Session.SetString("path", Request.Path);
            await PutTimes();
            PutTimesToTable();
            TimeTimetableModel model = new();
            model.times = await makeListTimetables();
            return View(model);
        }
        [Culture]
        [HttpPost]
        public async Task<IActionResult> AddTimeT(string Start, string End)
        {
            HttpContext.Session.SetString("path", Request.Path);
            if (Start != null && End != null)
            {
                if (CheckTime(Start, End))
                {
                    try

                    {
                        if (await timeService.CheckTimeT(Start, End))
                        {
                            HttpContext.Session.SetString("TimeMessage", Resources.Resource.TimeMes2);
                        }
                        else
                        {
                            await timeService.AddTimeT(Start, End);
                        }
                    }
                    catch { }
                }
            }
            else
            {
                HttpContext.Session.SetString("TimeMessage", Resources.Resource.TimeMes1);
            }
           // await PutTimes();
            return RedirectToAction("AddTimeT");
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
        public async Task<IActionResult> EditTime(int timeId)
        {
           // HttpContext.Session.SetString("path", Request.Path);
            TimeTDTO p = await timeService.GetTimeT(timeId);
            if (p != null)
            {
                if (TempData.ContainsKey("ErrorMessage"))
                {
                    // Retrieve the error message
                    ViewBag.ErrorMessage = TempData["ErrorMessage"] as string;
                }
                return View("EditTimeT",p);
            }
            //await PutTimes();
            //return View("AddTimeT");

            return RedirectToAction("AddTimeT");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTime( TimeTDTO t, string Start, string End)
        {
            HttpContext.Session.SetString("path", Request.Path);
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
                    TempData["ErrorMessage"] = Resources.Resource.TimeMes6;
                    return RedirectToAction("EditTime", new { Id = t.Id });
                   
                }
                timesT.Clear();
                return RedirectToAction("AddTimeT");
            }
            catch
            {
                //await PutTimes();
                //return View("AddTimeT");
              return   RedirectToAction("AddTimeT");
            }
        }
      
        //public async Task<IActionResult> DeleteTime(int idp)
        //{
            // HttpContext.Session.SetString("path", Request.Path);
            //TimeTDTO p = await timeService.GetTimeT(idp);
            //if (p != null)
            //{
            //    return Json(true);
            //    //return View("DeleteTimeT", p);
            //}
            //await PutTimes();
            //return View("AddTimeT");
            //return RedirectToAction("AddTimeT");
        //    return Json(true);
        //}
        
        public async Task<IActionResult> ConfirmDeleteTime(int Id)
        {
            //  HttpContext.Session.SetString("path", Request.Path);
            try
            {
                TimeTDTO p = await timeService.GetTimeT(Id);
                if (p == null)
                {
                    //await PutTimes();
                    //return View("AddTimeT");
                    return Json(false);
                }               
                await timeService.DeleteTimeT(p.Id);
                //await PutTimes();
                //return View("AddTimeT");
                timesT.Clear();
                //return RedirectToAction("AddTimeT");
                return Json(true);
            }
            catch
            {
                //await PutTimes();
                //return View("AddTimeT");
                //return RedirectToAction("AddTimeT");
                return Json(false);
            }
        }
        [HttpGet]
      
         public async Task<IActionResult> AddTimetable()
         {
            //await PutTimes();
            //return View();
            return RedirectToAction("AddTimeT");
        }
         public async Task<IActionResult> AddTimesToTable(int Tid)
         {
             TimeTDTO p = await timeService.GetTimeT(Tid);
             timesT.Add(p);
             await PutTimes();
              PutTimesToTable();
            //return View("AddTimetable");
            //return View ("AddTimeT");
            return RedirectToAction("AddTimeT");
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
                //return RedirectToAction("GetAllTimetable");
                return RedirectToAction("AddTimeT");
            }
             else
             {
                //await PutTimes();
                //PutTimesToTable();
                // return View();
                return RedirectToAction("AddTimeT");
            }
         }
        [HttpGet]
        public async Task<IActionResult> Cancel()
        {
            if (timesT.Count > 0)
            {              
                timesT.RemoveAt(timesT.Count-1);
                //await PutTimes();
                //PutTimesToTable();
                //return View("AddTimetable");              
                return RedirectToAction("AddTimeT");
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
            //return RedirectToAction("AddTimetable");
            return RedirectToAction("AddTimeT");
        }
        [HttpGet]
        public IActionResult BackToAll()
        {
            return RedirectToAction("GetAllTimetable");
        }
        public void PutTimesToTable()
         {

             List<TimeShow> p1 = new();
            if (timesT.Count > 0)
            {
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
        public async Task< List<TimetableShow>> makeListTimetables()
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
            return ts;
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
        //[HttpGet]
        //public async Task<IActionResult> DeleteTimetable(int id)
        //{
        //    if (id != 0)
        //    {
        //        TimetableDTO tt = await timetableService.GetTimetable(id);
        //        List<TimetableShow> ts = new();              
        //        List<TimeTDTO> pp = new();
        //        TimetableShow tss = new();
        //        tss.Id = tt.Id;
        //            foreach (var i in tt.TimesId)
        //            {
        //                TimeTDTO td = await timeService.GetTimeT(i);
        //                pp.Add(td);
        //            }
        //            IEnumerable<TimeTDTO> p2 = pp.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
        //            foreach (var i in p2)
        //            {
        //                string st = i.StartTime + "/" + i.EndTime;
        //                tss.Times.Add(st);
        //            }
        //            ts.Add(tss);                             
        //        MakeSheduleView m = new();
        //        m.times = ts;
        //        return View(m);
        //    }
        //    return Redirect("GetAllTimetable");
        //}
        [HttpPost]
        public async Task<IActionResult> ConfirmDeleteTimetable(int Id)
        {
            if (Id != 0)
            {
                await timetableService.DeleteTimetable(Id);

            }
            return RedirectToAction("AddTimeT");
        }

       
        public async Task<IActionResult> AddTimetableToShedule(int id, int roomId)
        {
            if (id != 0)
            {
                TimetableDTO tt = await timetableService.GetTimetable(id);
                if (timetables.Count > 0 && timetables[timetables.Count-1] == null)
                {
                    timetables[timetables.Count - 1] = tt;
                }
               else if (timetables.Count < 7)
                    timetables.Add(tt);
            }
            if(id==0)
            {
                if (timetables.Count > 0 && timetables[timetables.Count - 1] == null)
                {
                    TimetableDTO tt1 = new();
                    tt1.Id = id;
                    timetables[timetables.Count - 1] = tt1;
                }
                else if (timetables.Count < 7)
                {
                    TimetableDTO tt1 = new();
                    tt1.Id = id;
                    timetables.Add(tt1);
                }
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
                if (t != null)
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
            }

            MakeSheduleView m = new();
            m.times = ts;
            m.room = r;
            m.timesAdded = ts2;
            return View("GetTimetables", m);
        }
        public IActionResult CancelAddTimetable(int roId)
        {
            try
            {
                int nullCount = timetables.Count(item => item == null);
                timetables.RemoveAt(timetables.Count - nullCount- 1);
                
            }
            catch { }
            return RedirectToAction("AddTimetableToShedule", new { id = -1 , roomId = roId });
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
            for (int t = 0; t < 7; t++)
            {
                try
                {
                    if (timetables[t] != null)
                        shedule.timetables.Add(timetables[t]);
                    else
                    {
                        TimetableDTO t1 = new();
                        shedule.timetables.Add(t1);
                    }
                }
                catch 
                {
                    TimetableDTO t1 = new();                                        
                    shedule.timetables.Add(t1);
                }               
            }
           await sheduleService.AddShedule(shedule,r);
            timetables.Clear();
            return RedirectToAction("Room_Shedule");
        }
       
        public async Task<IActionResult> DeleteShedule(int RomId)
        {
           // SheduleDTO shedule = await sheduleService.GetShedule(SheduleId);
            RoomDTO r = await roomService.GetRoom(RomId);
           
            return PartialView(r);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSheduleInRoom(int RomId/*,int SheduleId*/)
        {
            
            RoomDTO r = await roomService.GetRoom(RomId);
            SheduleDTO shedule = await sheduleService.GetShedule(r.sheduleId.Value);
            IEnumerable<TrainingIndDTO> training = await trainingIndService.GetAllTrainingInds();
            foreach (TrainingIndDTO trainingInd in training)
            {
                if (trainingInd.RoomId == RomId)
                    await trainingIndService.DeleteTrainingInd(trainingInd.Id.Value);
            }    
            if (shedule != null && r!=null)
            {
                int s = r.sheduleId.Value;
                r.sheduleId = null;
                await roomService.Update(r);
                await sheduleService.DeleteShedule(s);
                return Json(true);
            }
            //  return RedirectToAction("RoomWithShedule", new { RoomId = RomId });
            return Json(false);
        }
        [HttpGet]
        public async Task<IActionResult> Room_Shedule()
        {
            IEnumerable<RoomDTO> r = await roomService.GetAllRooms();
           // ViewData["RoomsId"] = new SelectList(r, "Id", "Name");
            return View(r);
        }
        public IActionResult BackToRooms()
        {
            return RedirectToAction("Room_Shedule");
        }
        public async Task<IActionResult> RoomWithShedule(/*int RoomId, int CoachId, string Time*/int RoomId)
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

                        IEnumerable<TrainingGroupDTO> trg = await trainingGroupService.GetAllTrainingGroups();
                        List<TrainingGrToSee> trg1 = new();
                        if (trg != null)
                        {
                            foreach (var tr in trg)
                            {
                                TrainingGrToSee train = new();
                                train.Id = tr.Id;
                                //train.Group = await groupService.GetGroup(tr.GroupId);
                                RoomDTO r = await roomService.GetRoom(tr.RoomId);
                                train.Room = r;
                                //train.Group = await groupService.GetGroup(tr.GroupId);
                                train.Number = tr.Number;
                                train.Room = room;
                                train.Coach = await coachService.GetCoach(tr.CoachId);

                                //IEnumerable<UserDTO> us = await groupService.GetGroupUsers(tr.GroupId);
                                IEnumerable<UserDTO> us = await trainingGroupService.GetTrainingGroupUsers(tr.Id);
                                train.Users = us.ToList();
                                train.Time = tr.Time;
                                train.Day = tr.Day;
                                trg1.Add(train);
                            }
                            m.traininggroup = trg1.ToList();
                        }
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
            else { return RedirectToAction("Room_Shedule"); }
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
                        if (trg != null)
                        {
                            foreach (var tr in trg)
                            {
                                TrainingGrToSee train = new();
                                train.Id = tr.Id;
                                //train.Group = await groupService.GetGroup(tr.GroupId);
                                RoomDTO r = await roomService.GetRoom(tr.RoomId);
                                train.Room = r;
                                //train.Group = await groupService.GetGroup(tr.GroupId);
                                train.Number = tr.Number;
                                train.Room = room;
                                train.Coach = await coachService.GetCoach(tr.CoachId);

                                //IEnumerable<UserDTO> us = await groupService.GetGroupUsers(tr.GroupId);
                                IEnumerable<UserDTO> us = await trainingGroupService.GetTrainingGroupUsers(tr.Id);
                                train.Users = us.ToList();
                                train.Time = tr.Time;
                                train.Day = tr.Day;
                                trg1.Add(train);
                            }
                            m.traininggroup = trg1.ToList();
                        }
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
                    m.message = Resources.Resource.NoSh;
                }
                ViewBag.MyId = HttpContext.Session.GetString("Id");
                return View(m);
            }
            else { return RedirectToAction("Room_Shedule"); }
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
            return RedirectToAction("RoomWithShedule", /*new { RoomId = tr.RoomId, CoachId = tr.CoachId, Time = tr.Time }*/new { RoomId = roomId });
        }
        [HttpPost]
        public async Task<IActionResult> AddUserToTrainingInd(int id, int day, int roomId, string time, string roomName, int coachId)
        {
            TrainingIndDTO tr = await trainingIndService.GetTrainingInd(id);
            tr.CoachId = coachId;
            tr.RoomId = roomId;
            tr.RoomName = roomName;
            tr.Day = day;
            tr.Time = time;
            IEnumerable<UserDTO> p = await userService.GetAllUsers();
            ViewData["UserId"] = new SelectList(p, "Id", "Name");

            await trainingIndService.UpdateTrainingInd(tr);
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
                        HttpContext.Session.SetString("TimeMessage", Resources.Resource.TimeMes3);
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    HttpContext.Session.SetString("TimeMessage", Resources.Resource.TimeMes4);

                }
            }
            else
            {
                HttpContext.Session.SetString("TimeMessage", Resources.Resource.TimeMes5);
            }
            return false;
        }
        public async Task<IActionResult> GetTimeTableToEdit(int SheduleId,int day, int RoomId)
        {
            if (SheduleId != 0)
            {
                List<TimetableShow> ts = new();
                SheduleDTO sh = await sheduleService.GetShedule(SheduleId);
                TimetableDTO t = sh.timetables[day];
                List<TimeTDTO> pp = new();
                //TimetableShow t1 = new();
                //t1.Id = t.Id;

                //foreach (int i in t.TimesId)
                //{
                //    TimeTDTO td = await timeService.GetTimeT(i);
                //    string st = td.StartTime + "/" + td.EndTime;
                //    t1.Times.Add(st);
                //}
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
                RoomDTO r = await roomService.GetRoom(RoomId);
                MakeSheduleView m = new();
                m.times = ts;
                m.room = r;
                return View(m);
            }
            else
                return RedirectToAction("Room_Shedule");

        }
        public IActionResult BackToShedule(int roomId)
        {
            return RedirectToAction("RoomWithShedule", new { RoomId = roomId });

        }


        [HttpPost]
        public async Task<IActionResult> AddTrainingGroup(int day, int roomId, string time, string roomName, /*int groupId*/ int number)
        {
            //GroupAndTrainingGroup gatg = new GroupAndTrainingGroup();

            TrainingGroupDTO training = new();
            training.RoomId = roomId;
            training.Time = time;
            training.Day = day;
            training.RoomName = roomName;
            training.Number = number;//groupId
            training.UsersId = new List<UserDTO>();
            if (day == 0) training.DayName = "Понедельник";
            else if (day == 1) training.DayName = "Вторник";
            else if (day == 2) training.DayName = "Среда";
            else if (day == 3) training.DayName = "Четверг";
            else if (day == 4) training.DayName = "Пятница";
            else if (day == 5) training.DayName = "Суббота";
            else if (day == 6) training.DayName = "Воскресенье";
            //gatg.trgroup = training;
            IEnumerable<CoachDTO> p = await coachService.GetAllCoaches();
            ViewData["CoachId"] = new SelectList(p, "Id", "Name");
            //IEnumerable<GroupDTO> p_ = await groupService.GetAllGroups();
            //ViewData["GroupId"] = new SelectList(p_, "Id", "Name");
            return View(training);/*gatg*/
        }
        [HttpPost]
        public async Task<IActionResult> ToAddTrainingGroup(int day, int roomId, string time, string roomName, int coachId, int number)
        {

            TrainingGroupDTO tr = new();
            tr.UsersId = new();
            tr.CoachId = coachId;
            tr.RoomId = roomId;
            tr.RoomName = roomName;
            tr.Day = day;
            tr.Time = time;
            tr.Number = number; 
            tr.UsersId = new();
            //tr.UsersId = userId;
            //tr.UserName = userName;

            await trainingGroupService.AddTrainingGroup(tr);
            return RedirectToAction("RoomWithShedule", new { Id = roomId });
        }


        public async Task<IActionResult> EditTrainingGroup(int id)
        {
            HttpContext.Session.SetString("path", Request.Path);
            TrainingGroupDTO trainingGroupdto = await trainingGroupService.GetTrainingGroup(id);
            if (trainingGroupdto != null)
            {
                //await putCoaches();
                //await putGroups();
                //await putRooms();//
                //await putTimes();//
                //await putSpecialitys();//
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
            int roomId = trainingGroup.RoomId;
            //int groupId = trainingGroup.GroupId;
            await trainingGroupService.DeleteTrainingGroup(id);
            //await groupService.DeleteGroup(groupId);
            return RedirectToAction("RoomWithShedule", new { Id = roomId });
            //return RedirectToAction("GetTrainingGroups", "TrainingGroup");
        }

        public async Task<IActionResult> DetailsTrainingGroup(int id)
        {
            HttpContext.Session.SetString("path", Request.Path);
            TrainingGroupDTO trainingGroup = await trainingGroupService.GetTrainingGroup(id);
            if (trainingGroup == null)
            {
                return NotFound();
            }
            trainingGroup.Id = id;
            return View(trainingGroup);
        }
    }
}
