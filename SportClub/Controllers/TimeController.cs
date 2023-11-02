
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportClub.BLL.DTO;
using SportClub.BLL.Interfaces;
using SportClub.BLL.Services;
using SportClub.Models;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace SportClub.Controllers
{
    public class TimeController : Controller
    {
        private readonly IAdmin adminService;
        private readonly IUser userService;
        private readonly ICoach coachService;
        private readonly ITime timeService;
        private readonly ITimetable timetableService;
        private readonly ISpeciality specialityService;
        private static List<TimeTDTO> timesT=new();
        public TimeController(IAdmin adm, IUser us, ICoach c, ISpeciality sp, ITime t, ITimetable timetableService)
        {
            adminService = adm;
            userService = us;
            coachService = c;
            timeService = t;
            specialityService = sp;
            this.timetableService = timetableService;
        }
        [HttpGet]
        public async Task<IActionResult> AddTimeT()
        {
            await PutTimes();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddTimeT(string Start,string End)
        {
            try
            {
                await timeService.AddTimeT(Start, End);
            }
            catch { }
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
                p.StartTime = Start;
                p.EndTime = End;
                await timeService.UpdateTimeT(p);
                await PutTimes();
                return View("AddTimeT");
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
             await PutTimesToTable();
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
                await PutTimesToTable();
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
                await PutTimesToTable();
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
        public async Task PutTimesToTable()
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
        public async Task<IActionResult> GetAllTimetable()
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
        
            return View("GetTimetables",ts);
        }
    }
}
