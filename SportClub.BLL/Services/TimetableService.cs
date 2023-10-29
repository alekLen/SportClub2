using SportClub.BLL.DTO;
using SportClub.BLL.Interfaces;
using SportClub.BLL.Infrastructure;
using SportClub.DAL.Interfaces;
using SportClub.BLL.DTO;
using SportClub.DAL.Entities;
using AutoMapper;

namespace SportClub.BLL.Services
{
    public class TimetableService : ITime
    {
        IUnitOfWork Database { get; set; }

        public TimetableService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public async Task AddTimeT(TimeTDTO pDto)
        {
            var a = new TimeT()
            {
                Name = pDto.Name,
                Times= pDto.Times
            };
            await Database.Times.AddItem(a);
            await Database.Save();
        }
        public async Task<TimeTDTO> GetTimeT(int id)
        {
            TimeT a = await Database.Times.Get(id);
            if (a == null)
                throw new ValidationException("Wrong", "");
            return new TimeTDTO
            {
                Id = a.Id,
                Name = a.Name,
                Times = a.Times
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
        public async Task UpdateTimeT(TimeTDTO a)
        {
            TimeT t = await Database.Times.Get(a.Id);
            t.Name = a.Name;
            t.Times = a.Times;
            await Database.Times.Update(t);
            await Database.Save();
        }
    }
}
