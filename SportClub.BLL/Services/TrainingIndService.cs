using AutoMapper;
using SportClub.BLL.DTO;
using SportClub.DAL.Entities;
using SportClub.DAL.Interfaces;
using SportClub.BLL.Interfaces;
using SportClub.BLL.Infrastructure;


namespace SportClub.BLL.Services
{
    public class TrainingIndService : ITrainingInd
    {
        IUnitOfWork Database { get; set; }

        public TrainingIndService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task AddTrainingInd(TrainingIndDTO pDto)
        {
            TimeT t = await Database.Times.Get(pDto.TimeId);
            Room r = await Database.Rooms.Get(pDto.RoomId);
            Coach c = await Database.Coaches.Get(pDto.CoachId.Value);
            User u = await Database.Users.Get(pDto.UserId.Value);
            Speciality s = await Database.Specialitys.Get(pDto.SpecialityId.Value);
            var a = new TrainingInd()
            {
                Name = pDto.Name,
                Time = t,
                Room = r,
                Coach = c,
                User =u,
                Speciality = s
            };
            await Database.TrainingInds.AddItem(a);
            await Database.Save();
        }
        public async Task<TrainingIndDTO> GetTrainingInd(int id)
        {
            TrainingInd a = await Database.TrainingInds.Get(id);
            if (a == null)
                throw new ValidationException("Wrong", "");
            return new TrainingIndDTO
            {
                Id = a.Id,
                Name = a.Name,
         TimeId =a.Time.Id,
         RoomId =a.Room.Id,
         CoachName =a.Coach.Name,
         CoachId =a.Coach.Id,
         UserName =a.User.Name,
         UserId =a.User.Id,
        SpecialityName =a.Speciality.Name,
        SpecialityId =a.Speciality.Id
            };
        }
        public async Task<IEnumerable<TrainingIndDTO>> GetAllTrainingInds()
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<TrainingInd, TrainingIndDTO>()
                 .ForMember("CoachName", opt => opt.MapFrom(c => c.Coach.Name)).ForMember("SpecialityName", opt => opt.MapFrom(c => c.Speciality.Name))
                 .ForMember("UserName", opt => opt.MapFrom(c => c.User.Name)).ForMember("SpecialityId", opt => opt.MapFrom(c => c.Speciality.Id))
                 .ForMember("TimeId", opt => opt.MapFrom(c => c.Time.Id)).ForMember("RoomId", opt => opt.MapFrom(c => c.Room.Id))
                 .ForMember("CoachId", opt => opt.MapFrom(c => c.Coach.Id)).ForMember("UserId", opt => opt.MapFrom(c => c.User.Id)));
                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<TrainingInd>, IEnumerable<TrainingIndDTO>>(await Database.TrainingInds.GetAll());
            }
            catch { return null; }
        }
        public async Task<IEnumerable<TrainingIndDTO>> GetAllOfCoachTrainingInds(int id)
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<TrainingInd, TrainingIndDTO>()
                 .ForMember("CoachName", opt => opt.MapFrom(c => c.Coach.Name)).ForMember("SpecialityName", opt => opt.MapFrom(c => c.Speciality.Name))
                 .ForMember("UserName", opt => opt.MapFrom(c => c.User.Name)).ForMember("SpecialityId", opt => opt.MapFrom(c => c.Speciality.Id))
                 .ForMember("TimeId", opt => opt.MapFrom(c => c.Time.Id)).ForMember("RoomId", opt => opt.MapFrom(c => c.Room.Id))
                 .ForMember("CoachId", opt => opt.MapFrom(c => c.Coach.Id)).ForMember("UserId", opt => opt.MapFrom(c => c.User.Id)));
                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<TrainingInd>, IEnumerable<TrainingIndDTO>>(await Database.TrainingInds.GetAllOfCoach(id));
            }
            catch { return null; }
        }
        public async Task<IEnumerable<TrainingIndDTO>> GetAllOfClientTrainingInds(int id)
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<TrainingInd, TrainingIndDTO>()
                 .ForMember("CoachName", opt => opt.MapFrom(c => c.Coach.Name)).ForMember("SpecialityName", opt => opt.MapFrom(c => c.Speciality.Name))
                 .ForMember("UserName", opt => opt.MapFrom(c => c.User.Name)).ForMember("SpecialityId", opt => opt.MapFrom(c => c.Speciality.Id))
                 .ForMember("TimeId", opt => opt.MapFrom(c => c.Time.Id)).ForMember("RoomId", opt => opt.MapFrom(c => c.Room.Id))
                 .ForMember("CoachId", opt => opt.MapFrom(c => c.Coach.Id)).ForMember("UserId", opt => opt.MapFrom(c => c.User.Id)));
                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<TrainingInd>, IEnumerable<TrainingIndDTO>>(await Database.TrainingInds.GetAllOfClient(id));
            }
            catch { return null; }
        }
        public async Task DeleteTrainingInd(int id)
        {
            await Database.TrainingInds.Delete(id);
            await Database.Save();
        }
        public async Task UpdateTrainingInd(TrainingIndDTO a)
        {
            TimeT t = await Database.Times.Get(a.TimeId);
            Room r = await Database.Rooms.Get(a.RoomId);
            Coach c = await Database.Coaches.Get(a.CoachId.Value);
            User u = await Database.Users.Get(a.UserId.Value);
            Speciality s = await Database.Specialitys.Get(a.SpecialityId.Value);
            TrainingInd tr = await Database.TrainingInds.Get(a.Id);
            tr.Id = a.Id;
            tr.Name = a.Name;
            tr.Time = t;
            tr.Room = r;
            tr.Coach = c;
            tr.User = u;
            tr.Speciality = s;
            await Database.TrainingInds.Update(tr);
            await Database.Save();
        }
    }
}
