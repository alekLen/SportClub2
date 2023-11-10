using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportClub.BLL.DTO;
using SportClub.BLL.Interfaces;
using SportClub.BLL.Services;
using SportClub.DAL.Entities;
using SportClub.Models;
using System.Collections.Generic;

namespace SportClub.Controllers
{
    public class SheduleController : Controller
    {
        IWebHostEnvironment _appEnvironment;
        private readonly IRoom roomService;
        private readonly ITimetable timetableService;
        private readonly IShedule sheduleService;
        private readonly ITime timeService;
        private static List<TimeTDTO> timesT = new();
        public static List<TimetableShow> times { get; set; } = new();

        public SheduleController(IRoom r, ITimetable t, IShedule c, ITime n, IWebHostEnvironment _appEnv)
        {
            roomService = r;
            timetableService = t;
            sheduleService = c;
            timeService = n;
            _appEnvironment = _appEnv;
        }
        [HttpGet, HttpPost]
        public async Task <IActionResult> MakeShedule(MakeSheduleView? mv)
        {
            if (mv == null)
                mv = new();
           // IEnumerable<RoomDTO> r = await roomService.GetAllRooms();
            //ViewData["RoomId"] = new SelectList(r, "Id", "Name");
            IEnumerable<TimetableDTO> t = await timetableService.GetAllTimetables();
            ViewData["TimetableId"] = new SelectList(t, "Id", "Name");
            await PutTimes();
            PutTimesToTable();
            PutTimetables(mv);
            return View(mv);
        }
        public async Task<IActionResult> AddTimetableToShedule(MakeSheduleView mv,int id)
        {
            TimetableDTO ti = await timetableService.GetTimetable(id);

           // List<TimetableShow> ts = new();
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
                //ts.Add(t1);
                //mv.times.Add(t1);
                times.Add(t1);

            }
            PutTimesToTable();
            PutTimetables(mv);
            return View("MakeShedule",mv);
        }
       /* public async Task<IActionResult> AddTimesToTable(MakeSheduleView mv)
        {
            TimeTDTO p = await timeService.GetTimeT(mv.newT.Id);
            timesT.Add(p);
            await PutTimes();
            PutTimesToTable();
            PutTimetables(mv);
            return View("MakeShedule",mv);
        }*/
       // [HttpPost]
     /*   public async Task<IActionResult> AddTimeTable(MakeSheduleView mv, int id)
        {
            if (timesT.Count > 0)
            {
                TimetableDTO t = new();
                TimetableShow t1 = new();
                foreach (var time in timesT)
                { 
                    t.TimesId.Add(time.Id);
                    TimeTDTO td = await timeService.GetTimeT(time.Id);
                    string st = td.StartTime + "/" + td.EndTime;
                    t1.Times.Add(st);
                    t1.T.Add(td);
                }
                mv.timetables.Add(t);
                // mv.times.Add(t1);
                times.Add(t1);
                timesT.Clear();
                PutTimetables(mv);
                IEnumerable<TimetableDTO> t2 = await timetableService.GetAllTimetables();
                ViewData["TimetableId"] = new SelectList(t2, "Id", "Name");
                await PutTimes();
                PutTimesToTable();
                PutTimetables(mv);
                return View("MakeShedule",mv);
            }
            else
            {
                await PutTimes();
                PutTimesToTable();
                IEnumerable<TimetableDTO> t3 = await timetableService.GetAllTimetables();
                ViewData["TimetableId"] = new SelectList(t3, "Id", "Name");
                await PutTimes();
                PutTimetables(mv);
                return View();
            }
        }*/
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
        public async Task PutTimes()
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
        void PutTimetables(MakeSheduleView mv)
        {
            // if (mv.times.Count == 1)
            if(times.Count == 1)
            {
                List<TimeShow> p1 = new();
                //  IEnumerable<TimeTDTO> t1 = mv.times[0].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                IEnumerable<TimeTDTO> t1 = times[0].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                foreach (var t in t1)
                {
                    TimeShow ts = new()
                    {
                        Id = t.Id,
                        Time = t.StartTime + "/" + t.EndTime
                    };
                    p1.Add(ts);
                }            
                ViewData["MondayId"] = new SelectList(p1, "Id", "Time");
            }
            // if (mv.times.Count == 2)
            if (times.Count == 2)
            {
                List<TimeShow> p1 = new();
                IEnumerable<TimeTDTO> t1 = times[0].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                foreach (var t in t1)
                {
                    TimeShow ts = new()
                    {
                        Id = t.Id,
                        Time = t.StartTime + "/" + t.EndTime
                    };
                    p1.Add(ts);
                }
                ViewData["MondayId"] = new SelectList(p1, "Id", "Time");
                List<TimeShow> p2 = new();
                IEnumerable<TimeTDTO> t2 = times[0].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                foreach (var t in t2)
                {
                    TimeShow ts = new()
                    {
                        Id = t.Id,
                        Time = t.StartTime + "/" + t.EndTime
                    };
                    p2.Add(ts);
                }
                ViewData["TuesdayId"] = new SelectList(p2, "Id", "Time");
            }
            if (mv.times.Count == 3)
            {
                List<TimeShow> p1 = new();
                IEnumerable<TimeTDTO> t1 = mv.times[0].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                MakeViewData(p1, t1, "MondayId");
                List<TimeShow> p2 = new();
                IEnumerable<TimeTDTO> t2 = mv.times[1].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                MakeViewData(p2, t2, "TuesdayId");
                List<TimeShow> p3 = new();
                IEnumerable<TimeTDTO> t3 = mv.times[2].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                MakeViewData(p3, t3, "WednesdayId");
            }
            if (mv.times.Count == 4)
            {
                List<TimeShow> p1 = new();
                IEnumerable<TimeTDTO> t1 = mv.times[0].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                MakeViewData(p1, t1, "MondayId");
                List<TimeShow> p2 = new();
                IEnumerable<TimeTDTO> t2 = mv.times[1].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                MakeViewData(p2, t2, "TuesdayId");
                List<TimeShow> p3 = new();
                IEnumerable<TimeTDTO> t3 = mv.times[2].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                MakeViewData(p3, t3, "WednesdayId");
                List<TimeShow> p4 = new();
                IEnumerable<TimeTDTO> t4 = mv.times[3].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                MakeViewData(p4, t4, "ThursdayId");
            }
            if (mv.times.Count == 5)
            {
                List<TimeShow> p1 = new();
                IEnumerable<TimeTDTO> t1 = mv.times[0].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                MakeViewData(p1, t1, "MondayId");
                List<TimeShow> p2 = new();
                IEnumerable<TimeTDTO> t2 = mv.times[1].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                MakeViewData(p2, t2, "TuesdayId");
                List<TimeShow> p3 = new();
                IEnumerable<TimeTDTO> t3 = mv.times[2].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                MakeViewData(p3, t3, "WednesdayId");
                List<TimeShow> p4 = new();
                IEnumerable<TimeTDTO> t4 = mv.times[3].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                MakeViewData(p4, t4, "ThursdayId");
                List<TimeShow> p5 = new();
                IEnumerable<TimeTDTO> t5 = mv.times[4].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                MakeViewData(p5, t5, "FridayId");
                List<TimeShow> p6 = new();
                IEnumerable<TimeTDTO> t6 = mv.times[5].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
            }
            if (mv.times.Count == 6)
            {
                List<TimeShow> p1 = new();
                IEnumerable<TimeTDTO> t1 = mv.times[0].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                MakeViewData(p1, t1, "MondayId");
                List<TimeShow> p2 = new();
                IEnumerable<TimeTDTO> t2 = mv.times[1].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                MakeViewData(p2, t2, "TuesdayId");
                List<TimeShow> p3 = new();
                IEnumerable<TimeTDTO> t3 = mv.times[2].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                MakeViewData(p3, t3, "WednesdayId");
                List<TimeShow> p4 = new();
                IEnumerable<TimeTDTO> t4 = mv.times[3].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                MakeViewData(p4, t4, "ThursdayId");
                List<TimeShow> p5 = new();
                IEnumerable<TimeTDTO> t5 = mv.times[4].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                MakeViewData(p5, t5, "FridayId");
                List<TimeShow> p6 = new();
                IEnumerable<TimeTDTO> t6 = mv.times[5].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                MakeViewData(p6, t6, "SaturdayId");
            }
            if (mv.times.Count == 7)
            {
                List<TimeShow> p1 = new();
                IEnumerable<TimeTDTO> t1 = mv.times[0].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                foreach (var t in t1)
                {
                    TimeShow ts = new()
                    {
                        Id = t.Id,
                        Time = t.StartTime + "/" + t.EndTime
                    };
                    p1.Add(ts);
                }
                ViewData["MondayId"] = new SelectList(p1, "Id", "Time");
                List<TimeShow> p2 = new();
                IEnumerable<TimeTDTO> t2 = mv.times[0].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                foreach (var t in t2)
                {
                    TimeShow ts = new()
                    {
                        Id = t.Id,
                        Time = t.StartTime + "/" + t.EndTime
                    };
                    p2.Add(ts);
                }
                ViewData["TuesdayId"] = new SelectList(p2, "Id", "Time");
              
                List<TimeShow> p3 = new();
                IEnumerable<TimeTDTO> t3 = mv.times[2].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                MakeViewData(p3, t3, "WednesdayId");
                List<TimeShow> p4 = new();
                IEnumerable<TimeTDTO> t4 = mv.times[3].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                MakeViewData(p4, t4, "ThursdayId");
                List<TimeShow> p5 = new();
                IEnumerable<TimeTDTO> t5 = mv.times[4].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                MakeViewData(p5, t5, "FridayId");
                List<TimeShow> p6 = new();
                IEnumerable<TimeTDTO> t6 = mv.times[5].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                MakeViewData(p6, t6, "SaturdayId");
                List<TimeShow> p7 = new();
                IEnumerable<TimeTDTO> t7 = mv.times[6].T.OrderBy(x => int.Parse(x.StartTime.Split(':')[0]));
                MakeViewData(p7, t7, "SundayId");
            }
        }
        void MakeViewData(List<TimeShow> p1, IEnumerable<TimeTDTO> p2,string str)
        {
            foreach (var t in p2)
            {
                TimeShow ts = new()
                {
                    Id = t.Id,
                    Time = t.StartTime + "/" + t.EndTime
                };
                p1.Add(ts);
            }
            ViewData[str] = new SelectList(p1, "Id", "Time");
        }
        [HttpGet, HttpPost]
        public async Task<IActionResult> AddShedule(MakeSheduleView mv)
        {
           
            return View("Shedule");
        }
    }
}
