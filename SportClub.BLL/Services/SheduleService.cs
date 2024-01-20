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
        public async Task AddShedule(SheduleDTO pDto, RoomDTO room)
        {
            var a = new Shedule();
            //  a.Week.Clear();
            /* foreach (var sh in pDto.timetables)
                {
                    Timetable t = await Database.Timetables.Get(sh.Id);
                 a.Week.Add(t);         
                  }*/
            a.Monday = pDto.timetables[0].Id;
            // Timetable t2 = await Database.Timetables.Get(pDto.timetables[1].Id);
            a.Tuesday = pDto.timetables[1].Id;
            //Timetable t3 = await Database.Timetables.Get(pDto.timetables[2].Id);
            a.Wednesday = pDto.timetables[2].Id;
            // Timetable t4 = await Database.Timetables.Get(pDto.timetables[3].Id);
            a.Thursday = pDto.timetables[3].Id;
            //Timetable t5 = await Database.Timetables.Get(pDto.timetables[4].Id);
            a.Friday = pDto.timetables[4].Id;
            //  Timetable t6 = await Database.Timetables.Get(pDto.timetables[5].Id);
            a.Saturday = pDto.timetables[5].Id;
            //  Timetable t7 = await Database.Timetables.Get(pDto.timetables[6].Id);
            a.Sunday = pDto.timetables[6].Id;

            await Database.Shedules.AddItem(a);
            await Database.Save();
            Room r = await Database.Rooms.Get(room.Id);
            r.Shedule = a;
            foreach (var sh in pDto.timetables)
            {
                if (sh.Id != 0)
                {
                    Timetable t = await Database.Timetables.Get(sh.Id);
                    t.Shedules.Add(a);
                    await Database.Timetables.Update(t);

                    await Database.Save();
                }
            }
            await Database.Rooms.Update(r);
            await Database.Save();
            
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
            tt.Id = a.Id;
            /* foreach (var t in a.Week)
             {
                 Timetable t1 = await Database.Timetables.Get(t.Id);
                 TimetableDTO tm1 = new();
                 foreach (var time in t1.Times)
                 {
                     tm1.TimesId.Add(time.Id);
                 }
                 tt.timetables.Add(tm1);
             }*/
            if (a.Monday != 0)
            {
                Timetable t1 = await Database.Timetables.Get(a.Monday);
                TimetableDTO tm1 = new();
                foreach (var time in t1.Times)
                {
                    tm1.TimesId.Add(time.Id);
                }
                tt.timetables.Add(tm1);
            }
            else
            {
                TimetableDTO tm1 = new();
                tt.timetables.Add(tm1);
            }
            if (a.Tuesday != 0)
            {
                Timetable t2 = await Database.Timetables.Get(a.Tuesday);
                TimetableDTO tm2 = new();
                foreach (var time in t2.Times)
                {
                    tm2.TimesId.Add(time.Id);
                }
                tt.timetables.Add(tm2);
            }
            else
            {
                TimetableDTO tm1 = new();
                tt.timetables.Add(tm1);
            }
            if (a.Wednesday != 0)
            {
                Timetable t3 = await Database.Timetables.Get(a.Wednesday);
            TimetableDTO tm3 = new();
            foreach (var time in t3.Times)
            {
                tm3.TimesId.Add(time.Id);
            }
            tt.timetables.Add(tm3);
            }
            else
            {
                TimetableDTO tm1 = new();
                tt.timetables.Add(tm1);
            }
            if (a.Thursday != 0)
            {
                Timetable t4 = await Database.Timetables.Get(a.Thursday);
            TimetableDTO tm4 = new();
            foreach (var time in t4.Times)
            {
                tm4.TimesId.Add(time.Id);
            }
            tt.timetables.Add(tm4);
            }
            else
            {
                TimetableDTO tm1 = new();
                tt.timetables.Add(tm1);
            }
            if (a.Friday != 0)
            {
                Timetable t5 = await Database.Timetables.Get(a.Friday);
            TimetableDTO tm5 = new();
            foreach (var time in t5.Times)
            {
                tm5.TimesId.Add(time.Id);
            }
            tt.timetables.Add(tm5);
            }
            else
            {
                TimetableDTO tm1 = new();
                tt.timetables.Add(tm1);
            }
            if (a.Saturday != 0)
            {
                Timetable t6 = await Database.Timetables.Get(a.Saturday);
            TimetableDTO tm6 = new();
            foreach (var time in t6.Times)
            {
                tm6.TimesId.Add(time.Id);
            }
            tt.timetables.Add(tm6);
            }
            else
            {
                TimetableDTO tm1 = new();
                tt.timetables.Add(tm1);
            }
            if (a.Sunday != 0)
            {
                Timetable t7 = await Database.Timetables.Get(a.Sunday);
            TimetableDTO tm7 = new();
            foreach (var time in t7.Times)
            {
                tm7.TimesId.Add(time.Id);
            }
            tt.timetables.Add(tm7);
            }
            else
            {
                TimetableDTO tm1 = new();
                tt.timetables.Add(tm1);
            }

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
