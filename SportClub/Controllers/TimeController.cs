
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportClub.BLL.DTO;
using SportClub.BLL.Interfaces;
using SportClub.BLL.Services;
using SportClub.Models;
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
            /* IEnumerable<TimeTDTO> p= await timeService.GetAllTimeTs();
             IEnumerable<TimeTDTO>p2= p.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
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
             ViewData["TimeId"] = new SelectList(p1, "Id", "Time");*/
            await PutTimes();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddTimeT(string Start,string End)
        {
           await timeService.AddTimeT(Start,End);
            await PutTimes();
            return View();
        }
        public async Task PutTimes()
        {
            IEnumerable<TimeTDTO> p = await timeService.GetAllTimeTs();
            IEnumerable<TimeTDTO> p2 = p.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
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
                await PutTimes();
                return View("EditTimeT",p);
            }         
            return View("AddTimeT");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTime( TimeTDTO t)
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
                p.StartTime = t.StartTime;
                p.EndTime = t.EndTime;
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
        [HttpGet]
       /* public async Task<IActionResult> AddTimetable()
        {
            IEnumerable < TimeTDTO > t=await timeService.GetAllTimeTs();
            IEnumerable<TimeTDTO> t1 = t.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
            TimetableDTO timetable = new();
            foreach(TimeTDTO td in t1)
            {
                timetable.TimesId.Add(td.Id);
            }
            await timetableService.AddTimetable(timetable);
           // await timeService.DeleteAllTimeT();
            return View();
        }*/
         public async Task<IActionResult> AddTimetable(TimetableDTO? t )
         {           
             await PutTimes();
             return View(t);
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
                 return View("Index");
             }
             else
             {
                 await PutTimesToTable();
                 return View();
             }
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
    }
}
