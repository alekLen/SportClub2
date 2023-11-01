using SportClub.BLL.DTO;
using SportClub.BLL.Interfaces;
using SportClub.BLL.Infrastructure;
using SportClub.DAL.Interfaces;
using SportClub.DAL.Entities;
using AutoMapper;

namespace SportClub.BLL.Services
{
    public class TimeTService :ITime
    {
        IUnitOfWork Database { get; set; }

        public TimeTService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public async Task<TimeTDTO> AddTimeT(string s, string e)
        {
            TimeT a1 = await Database.Times.Find(s, e);
            if (a1 != null)
            {
                return new TimeTDTO
                {
                    Id = a1.Id,
                    StartTime = a1.StartTime,
                    EndTime = a1.EndTime
                };
            }
            else {
                var a = new TimeT()
                {
                    StartTime = s,
                    EndTime = e
                };
                await Database.Times.AddItem(a);
                await Database.Save();
                TimeT a2 = await Database.Times.Find(s, e);
                return new TimeTDTO
                {
                    Id = a2.Id,
                    StartTime = a2.StartTime,
                    EndTime = a2.EndTime
                };
            }
        }

      /* public async Task AddTimeT(TimeTDTO pDto)
        {
            var a = new TimeT()
            {
                StartTime = pDto.StartTime,
                EndTime = pDto.EndTime
            };
            await Database.Times.AddItem(a);
            await Database.Save();
        }*/
        public async Task<TimeTDTO> GetTimeT(int id)
        {
            TimeT a = await Database.Times.Get(id);
            if (a == null)
                throw new ValidationException("Wrong", "");
            return new TimeTDTO
            {
                Id = a.Id,
                StartTime = a.StartTime,
                EndTime = a.EndTime
            };
        }
       public async Task<TimeTDTO> FindTimeT(string s,string e)
        {
            TimeT a = await Database.Times.Find(s,e);
            if (a == null)
                throw new ValidationException("Wrong", "");
            return new TimeTDTO
            {
                Id = a.Id,
                StartTime = a.StartTime,
                EndTime = a.EndTime
            };
        }
        public async Task<IEnumerable<TimeTDTO>> GetAllTimeTs()
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<TimeT, TimeTDTO>());
                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<TimeT>, IEnumerable<TimeTDTO>>(await Database.Times.GetAll());
            }
            catch { return null; }
        }
        public async Task DeleteTimeT(int id)
        {
            await Database.Times.Delete(id);
            await Database.Save();
        }
        public async Task DeleteAllTimeT()
        {
            await Database.Times.DeleteAll();
            await Database.Save();
        }
        public async Task UpdateTimeT(TimeTDTO a)
        {
            TimeT t = await Database.Times.Get(a.Id);
            t.StartTime = a.StartTime;
            t.EndTime = a.EndTime;
            await Database.Times.Update(t);
            await Database.Save();
        }
    }
}
