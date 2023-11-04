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

        public SheduleController(IRoom r, ITimetable t, IShedule c,ITime n,  IWebHostEnvironment _appEnv)
        {
            roomService = r;
           timetableService = t;
            sheduleService = c;
            timeService= n;
            _appEnvironment = _appEnv;
        }
        public async Task <IActionResult> MakeShedule(MakeSheduleView? mv)
        {
            if (mv == null)
                mv = new();
            IEnumerable<RoomDTO> r = await roomService.GetAllRooms();
            ViewData["RoomId"] = new SelectList(r, "Id", "Name");
            IEnumerable<TimetableDTO> t = await timetableService.GetAllTimetables();
            ViewData["TimetableId"] = new SelectList(t, "Id", "Name");
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
    }
}
