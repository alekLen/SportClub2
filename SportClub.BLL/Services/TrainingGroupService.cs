using AutoMapper;
using SportClub.BLL.DTO;
using SportClub.DAL.Entities;
using SportClub.DAL.Interfaces;
using SportClub.BLL.Interfaces;
using SportClub.BLL.Infrastructure;

namespace SportClub.BLL.Services
{
    public class TrainingGroupService : ITrainingGroup
    {

        IUnitOfWork Database { get; set; }

        public TrainingGroupService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task AddTrainingGroup(TrainingGroupDTO pDto)
        {
            TimeT t = await Database.Times.Get(pDto.TimeId);
            Room r = await Database.Rooms.Get(pDto.RoomId);
            Coach c = await Database.Coaches.Get(pDto.CoachId.Value);
            Group u = await Database.Groups.Get(pDto.GroupId.Value);
            Speciality s = await Database.Specialitys.Get(pDto.SpecialityId.Value);
            var a = new TrainingGroup()
            {
                Name = pDto.Name,
                Number=pDto.Number,
                Time = t,
                Room = r,
                Coach = c,
                Group = u,
                Speciality = s
            };
            await Database.TrainingGroups.AddItem(a);
            await Database.Save();
        }
        public async Task<TrainingGroupDTO> GetTrainingGroup(int id)
        {
            TrainingGroup a = await Database.TrainingGroups.Get(id);
            if (a == null)
                throw new ValidationException("Wrong", "");
            return new TrainingGroupDTO
            {
                Id = a.Id,
                Name = a.Name,
                Number=a.Number,
                TimeId = a.Time.Id,
                RoomId = a.Room.Id,
                CoachName = a.Coach.Name,
                CoachId = a.Coach.Id,
                GroupName = a.Group.Name,
                GroupId = a.Group.Id,
                SpecialityName = a.Speciality.Name,
                SpecialityId = a.Speciality.Id
            };
        }
        public async Task<IEnumerable<TrainingGroupDTO>> GetAllTrainingGroups()
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<TrainingGroup, TrainingGroupDTO>()
                 .ForMember("CoachName", opt => opt.MapFrom(c => c.Coach.Name)).ForMember("SpecialityName", opt => opt.MapFrom(c => c.Speciality.Name))
                 .ForMember("GroupName", opt => opt.MapFrom(c => c.Group.Name)).ForMember("SpecialityId", opt => opt.MapFrom(c => c.Speciality.Id))
                 .ForMember("TimeId", opt => opt.MapFrom(c => c.Time.Id)).ForMember("RoomId", opt => opt.MapFrom(c => c.Room.Id))
                 .ForMember("CoachId", opt => opt.MapFrom(c => c.Coach.Id)).ForMember("GroupId", opt => opt.MapFrom(c => c.Group.Id)));
                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<TrainingGroup>, IEnumerable<TrainingGroupDTO>>(await Database.TrainingGroups.GetAll());
            }
            catch { return null; }
        }
        public async Task<IEnumerable<TrainingGroupDTO>> GetAllOfCoachTrainingGroups(int id)
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<TrainingGroup, TrainingGroupDTO>()
                 .ForMember("CoachName", opt => opt.MapFrom(c => c.Coach.Name)).ForMember("SpecialityName", opt => opt.MapFrom(c => c.Speciality.Name))
                 .ForMember("GroupName", opt => opt.MapFrom(c => c.Group.Name)).ForMember("SpecialityId", opt => opt.MapFrom(c => c.Speciality.Id))
                 .ForMember("TimeId", opt => opt.MapFrom(c => c.Time.Id)).ForMember("RoomId", opt => opt.MapFrom(c => c.Room.Id))
                 .ForMember("CoachId", opt => opt.MapFrom(c => c.Coach.Id)).ForMember("GroupId", opt => opt.MapFrom(c => c.Group.Id)));
                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<TrainingGroup>, IEnumerable<TrainingGroupDTO>>(await Database.TrainingGroups.GetAllOfCoach(id));
            }
            catch { return null; }
        }
        public async Task<IEnumerable<TrainingGroupDTO>> GetAllOfClientTrainingGroups(int id)
        {
            try
            {
                User user = await Database.Users.Get(id);
                var config = new MapperConfiguration(cfg => cfg.CreateMap<TrainingGroup, TrainingGroupDTO>()
                 .ForMember("CoachName", opt => opt.MapFrom(c => c.Coach.Name)).ForMember("SpecialityName", opt => opt.MapFrom(c => c.Speciality.Name))
                 .ForMember("GroupName", opt => opt.MapFrom(c => c.Group.Name)).ForMember("SpecialityId", opt => opt.MapFrom(c => c.Speciality.Id))
                 .ForMember("TimeId", opt => opt.MapFrom(c => c.Time.Id)).ForMember("RoomId", opt => opt.MapFrom(c => c.Room.Id))
                 .ForMember("CoachId", opt => opt.MapFrom(c => c.Coach.Id)).ForMember("GroupName", opt => opt.MapFrom(c => c.Group.Id)));
                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<TrainingGroup>, IEnumerable<TrainingGroupDTO>>(await Database.TrainingGroups.GetAllOfClient(user));
            }
            catch { return null; }
        }
        public async Task DeleteTrainingGroup(int id)
        {
            await Database.TrainingGroups.Delete(id);
            await Database.Save();
        }
        public async Task UpdateTrainingGroup(TrainingGroupDTO a)
        {
            TimeT t = await Database.Times.Get(a.TimeId);
            Room r = await Database.Rooms.Get(a.RoomId);
            Coach c = await Database.Coaches.Get(a.CoachId.Value);
            Group u = await Database.Groups.Get(a.GroupId.Value);
            Speciality s = await Database.Specialitys.Get(a.SpecialityId.Value);
            TrainingGroup tr = await Database.TrainingGroups.Get(a.Id);
            tr.Id = a.Id;
            tr.Name = a.Name;
            tr.Number = a.Number;
            tr.Time = t;
            tr.Room = r;
            tr.Coach = c;
            tr.Group = u;
            tr.Speciality = s;
            await Database.TrainingGroups.Update(tr);
            await Database.Save();
        }
    }
}
