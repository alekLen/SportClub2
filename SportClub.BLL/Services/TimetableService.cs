using SportClub.BLL.DTO;
using SportClub.BLL.Interfaces;
using SportClub.BLL.Infrastructure;
using SportClub.DAL.Interfaces;
using SportClub.DAL.Entities;
using AutoMapper;

namespace SportClub.BLL.Services
{
    public class TimetableService: ITimetable
    {
        IUnitOfWork Database { get; set; }

        public TimetableService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public async Task AddTimetable(TimetableDTO pDto)
        {
            var a = new Timetable();
            foreach(var t in pDto.TimesId)
            {
                TimeT time=await Database.Times.Get(t);
                a.Times.Add(time);
            }
            
            await Database.Timetables.AddItem(a);
            await Database.Save();
        }
        public async Task AddTimeToTimetable(string start,string end,TimetableDTO time)
        {
            TimeT t =await Database.Times.Find(start, end);
            if (t != null)
                time.TimesId.Add(t.Id);
            else
            {  
                var a = new TimeT()
                {
                    StartTime = start,
                    EndTime = end
                };
                await Database.Times.AddItem(a);
                await Database.Save();
                TimeT t1 = await Database.Times.Find(start, end);
                time.TimesId.Add(t1.Id);
            }
        }
        public async Task<TimetableDTO> GetTimetable(int id)
        {
            Timetable a = await Database.Timetables.Get(id);         
            if (a == null)
                return null;        
            TimetableDTO tt = new();
            tt.Id = a.Id;
            foreach (var t in a.Times)
            {
               tt.TimesId.Add(t.Id);
            }
            foreach (var t1 in a.Shedules)
            {
                tt.SheduleId.Add(t1.Id);
            }
            return tt;
        }
        public async Task<IEnumerable<TimetableDTO>> GetAllTimetables()
        {
            /*  try
              {
                  var config = new MapperConfiguration(cfg => cfg.CreateMap<Timetable, TimetableDTO>());
                  var mapper = new Mapper(config);
                  return mapper.Map<IEnumerable<Timetable>, IEnumerable<TimetableDTO>>(await Database.Timetables.GetAll());
              }
              catch { return null; }*/
            IEnumerable<Timetable> timetables = await Database.Timetables.GetAll();
            IEnumerable<TimetableDTO> timetables2 = new List<TimetableDTO> ();
            List<Timetable> ti =timetables.ToList();
            List<TimetableDTO> ti2 = timetables2.ToList();
            for (var i=0;i<ti.Count;i++)
            {
                TimetableDTO a =new ();
                a.Id = ti[i].Id;
                a.TimesId = new List<int>();
                foreach (var t in ti[i].Times)
                {
                    a.TimesId.Add(t.Id);
                }
                ti2.Add(a);
            }
            return ti2;
        }
        public async Task DeleteTimetable(int id)
        {
            await Database.Timetables.Delete(id);
            await Database.Save();
        }
        public async Task UpdateTimetable(TimetableDTO a)
        {

            Timetable t = await Database.Timetables.Get(a.Id);
            t.Times.Clear();
            for (var i = 0; i < a.TimesId.Count; i++)
            {
                TimeT time = await Database.Times.Get(a.TimesId[i]);
                t.Times.Add(time);
            }
            await Database.Timetables.Update(t);
            await Database.Save();
        }
    }
}
