using SportClub.BLL.Interfaces;
using SportClub.DAL.Interfaces;
using SportClub.BLL.DTO;
using SportClub.DAL.Entities;
using AutoMapper;

namespace SportClub.BLL.Services
{
    public class RoomService :IRoom
    {
        IUnitOfWork Database { get; set; }

        public RoomService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public async Task AddRoom(RoomDTO pDto)
        {
           // Shedule sh = await Database.Shedules.Get(pDto.sheduleId.Value);
            var a = new Room()
            {
                Name = pDto.Name,
                Photo=pDto.Photo
               // Shedule=sh
            };
            await Database.Rooms.AddItem(a);
            await Database.Save();
        }
        public async Task AddSheduleToRoom(RoomDTO pDto,int id)
        {
            Room a = await Database.Rooms.Get(pDto.Id);
            Shedule sh = await Database.Shedules.Get(id);
            a.Shedule = sh;           
            await Database.Rooms.Update(a);
            await Database.Save();
        }
        public async Task AddDeleteSheduleFromRoom(RoomDTO pDto)
        {
            Room a = await Database.Rooms.Get(pDto.Id);          
            a.Shedule = null;
            await Database.Rooms.Update(a);
            await Database.Save();
        }
        public async Task<RoomDTO> GetRoom(int id)
        {
            int shId = 0;
            Room a = await Database.Rooms.Get(id);
            if (a == null)
                return null;
            if (a.Shedule != null)
            {
                Shedule sh = await Database.Shedules.Get(a.Shedule.Id);
                shId = sh.Id;
            }
          
            return new RoomDTO
            {
                Id = a.Id,
                Name = a.Name,
                sheduleId = shId
            };
        }
        public async Task<IEnumerable<RoomDTO>> GetAllRooms()
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Room, RoomDTO>()
                 .ForMember("sheduleId", opt => opt.MapFrom(c => c.Shedule.Id)));
                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<Room>, IEnumerable<RoomDTO>>(await Database.Rooms.GetAll());
            }
            catch { return null; }
        }
        public async Task DeleteRoom(int id)
        {
            await Database.Rooms.Delete(id);
            await Database.Save();
        }
        public async Task UpdateRoom(RoomDTO pDto,int id)
        {
            Room a = await Database.Rooms.Get(pDto.Id);
            Shedule sh = await Database.Shedules.Get(id);
            a.Name = pDto.Name;
            a.Photo = pDto.Photo;
            a.Shedule = sh;
            await Database.Rooms.Update(a);
            await Database.Save();
        }
    }
}
