using SportClub.BLL.DTO;
using SportClub.BLL.Interfaces;
using SportClub.DAL.Entities;
using SportClub.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.Services
{
    public class SheduleService : IShedule
    {
        IUnitOfWork Database { get; set; }

        public SheduleService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public async Task AddShedule(SheduleDTO pDto)
        {
            var a = new Shedule();
           /* Timetable t1 = await Database.Timetables.Get(pDto.MondayId);
            a.Monday = t1;
            Timetable t2 = await Database.Timetables.Get(pDto.TuesdayId);
            a.Tuesday = t2;
            Timetable t3 = await Database.Timetables.Get(pDto.WednesdayId);
            a.Wednesday = t3;
            Timetable t4 = await Database.Timetables.Get(pDto.ThursdayId);
            a.Thursday = t4;
            Timetable t5 = await Database.Timetables.Get(pDto.FridayId);
            a.Friday = t5;
            Timetable t6 = await Database.Timetables.Get(pDto.SaturdayId);
            a.Saturday = t6;
            Timetable t7 = await Database.Timetables.Get(pDto.SundayId);
            a.Sunday = t7;
            await Database.Shedules.AddItem(a);
            await Database.Save();*/
        }
       /* public async Task AddTimetableToShedule(string start, string end, TimetableDTO time)
        {
            TimeT t = await Database.Times.Find(start, end);
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
        }*/
        public async Task<SheduleDTO> GetShedule(int id)
        {
            Shedule a = await Database.Shedules.Get(id);
            if (a == null)
                return null;
            SheduleDTO tt = new();
            tt.Id= id;
           /* if (a.Monday != null)
                tt.MondayId = a.Monday.Id;
            else
                tt.MondayId = 0;
            if (a.Tuesday != null)
                tt.TuesdayId = a.Tuesday.Id;
            else
                tt.TuesdayId = 0;
            if (a.Wednesday != null)
                tt.WednesdayId = a.Wednesday.Id;
            else
                tt.WednesdayId = 0;
            if (a.Thursday != null)
                tt.ThursdayId = a.Thursday.Id;
            else
                tt.ThursdayId = 0;
           if (a.Friday != null)
                tt.FridayId = a.Friday.Id;
            else
                tt.FridayId = 0;
            if (a.Saturday != null)
                tt.SaturdayId = a.Saturday.Id;
            else
                tt.SaturdayId = 0;
            if (a.Sunday != null)
                tt.SundayId = a.Sunday.Id;
            else
                tt.SundayId = 0;*/
            return tt;
        }
        public async Task<IEnumerable<SheduleDTO>> GetAllShedules()
        {
            IEnumerable<Shedule> timetables = await Database.Shedules.GetAll();
            IEnumerable<SheduleDTO> timetables2 = new List<SheduleDTO>();
            List<Shedule> ti = timetables.ToList();
            List<SheduleDTO> ti2 = timetables2.ToList();
            for (var i = 0; i < ti.Count; i++)
            {
                SheduleDTO tt = new();
                tt.Id = ti[i].Id;
               /* if (ti[i].Monday != null)
                    tt.MondayId = ti[i].Monday.Id;
                else
                    tt.MondayId = 0;
                if (ti[i].Tuesday != null)
                    tt.TuesdayId = ti[i].Tuesday.Id;
                else
                    tt.TuesdayId = 0;
                if (ti[i].Wednesday != null)
                    tt.WednesdayId = ti[i].Wednesday.Id;
                else
                    tt.WednesdayId = 0;
                if (ti[i].Thursday != null)
                    tt.ThursdayId = ti[i].Thursday.Id;
                else
                    tt.ThursdayId = 0;
                if (ti[i].Friday != null)
                    tt.FridayId = ti[i].Friday.Id;
                else
                    tt.FridayId = 0;
                if (ti[i].Saturday != null)
                    tt.SaturdayId = ti[i].Saturday.Id;
                else
                    tt.SaturdayId = 0;
                if (ti[i].Sunday != null)
                    tt.SundayId = ti[i].Sunday.Id;
                else
                    tt.SundayId = 0;
                ti2.Add(tt);*/
            }
            return ti2;
        }
        public async Task DeleteShedule(int id)
        {
            await Database.Shedules.Delete(id);
            await Database.Save();
        }
        public async Task UpdateShedule(SheduleDTO pDto)
        {
            Shedule a = await Database.Shedules.Get(pDto.Id);
           /* Timetable t1 = await Database.Timetables.Get(pDto.MondayId);
            a.Monday = t1;
            Timetable t2 = await Database.Timetables.Get(pDto.TuesdayId);
            a.Tuesday = t2;
            Timetable t3 = await Database.Timetables.Get(pDto.WednesdayId);
            a.Wednesday = t3;
            Timetable t4 = await Database.Timetables.Get(pDto.ThursdayId);
            a.Thursday = t4;
           Timetable t5 = await Database.Timetables.Get(pDto.FridayId);
            a.Friday = t5;
           Timetable t6 = await Database.Timetables.Get(pDto.SaturdayId);
            a.Saturday = t6;
            Timetable t7 = await Database.Timetables.Get(pDto.SundayId);
            a.Sunday = t7;*/
            await Database.Shedules.Update(a);
            await Database.Save();
        }
    }
}
