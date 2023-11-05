using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportClub.BLL.DTO;
using SportClub.BLL.Interfaces;
using SportClub.BLL.Services;
using SportClub.Models;

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
            return View(mv);
        }
        public async Task<IActionResult> AddTimetabletoShedule(MakeSheduleView mv,int id)
        {
            TimetableDTO ti = await timetableService.GetTimetable(id);

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

            return View("MakeShedule",mv);
        }
        public async Task<IActionResult> AddTimesToTable(int id)
        {
            TimeTDTO p = await timeService.GetTimeT(id);
            timesT.Add(p);
            await PutTimes();
            PutTimesToTable();
            return View("MakeShedule");
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
    }
}
