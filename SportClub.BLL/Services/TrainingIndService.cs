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
            //TimeT t = await Database.Times.Get(pDto.TimeId);
            Room r = await Database.Rooms.Get(pDto.RoomId);
            Coach c = await Database.Coaches.Get(pDto.CoachId.Value);
            User u=null;
            try
            {
                if (pDto.UserId.Value != null)
                   u = await Database.Users.Get(pDto.UserId.Value); 
            }
            catch { }
            //  Speciality s = await Database.Specialitys.Get(pDto.SpecialityId.Value);
            var a = new TrainingInd()
            {
                //Name = pDto.Name,
                Time = pDto.Time,
                Day = pDto.Day,
                Room = r,
                Coach = c,
                User = u,
               // Speciality = s
            };
            await Database.TrainingInds.AddItem(a);
            await Database.Save();
        }
        public async Task<TrainingIndDTO> GetTrainingInd(int id)
        {
            TrainingInd a = await Database.TrainingInds.Get(id);
            if (a == null)
                throw new ValidationException("Wrong", "");
            string str="";
            if (a.Day == 0) str = "Понедельник";
            else if (a.Day == 1) str = "Вторник";
            else if (a.Day == 2) str = "Среда";
            else if (a.Day == 3) str = "Четверг";
            else if (a.Day == 4) str = "Пятница";
            else if (a.Day == 5) str = "Суббота";
            else if (a.Day == 6) str = "Воскресенье";
            int usId = 0;
            string usName = "";
            if(a.User!=null)
            {
                usName = a.User.Name;
                usId = a.User.Id;
            }
            return new TrainingIndDTO
            {
                Id = a.Id,
                Name = a.Name,
                Time =a.Time,
                Day=a.Day,
                DayName=str,
                RoomId =a.Room.Id,
                RoomName=a.Room.Name,
                CoachName =a.Coach.Name,
                CoachId =a.Coach.Id,        
                UserName = usName,
                UserId = usId
      //  SpecialityName =a.Speciality.Name,
       // SpecialityId =a.Speciality.Id
            };
        }
        public async Task<IEnumerable<TrainingIndDTO>> GetAllTrainingInds()
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<TrainingInd, TrainingIndDTO>()
                 .ForMember("CoachName", opt => opt.MapFrom(c => c.Coach.Name))
                 .ForMember("UserName", opt => opt.MapFrom(c => c.User.Name))
                 .ForMember("RoomName", opt => opt.MapFrom(c => c.Room.Name)).ForMember("RoomId", opt => opt.MapFrom(c => c.Room.Id))
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
                 .ForMember("CoachName", opt => opt.MapFrom(c => c.Coach.Name))
                 .ForMember("UserName", opt => opt.MapFrom(c => c.User.Name))
                 .ForMember("RoomName", opt => opt.MapFrom(c => c.Room.Name)).ForMember("RoomId", opt => opt.MapFrom(c => c.Room.Id))
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
                 .ForMember("CoachName", opt => opt.MapFrom(c => c.Coach.Name))
                 .ForMember("UserName", opt => opt.MapFrom(c => c.User.Name))
                 .ForMember("RoomName", opt => opt.MapFrom(c => c.Room.Name)).ForMember("RoomId", opt => opt.MapFrom(c => c.Room.Id))
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
          //  TimeT t = await Database.Times.Get(a.TimeId);
            Room r = await Database.Rooms.Get(a.RoomId);
            Coach c = await Database.Coaches.Get(a.CoachId.Value);
            User u = null;
            if (a.UserId.Value!=0)
             u = await Database.Users.Get(a.UserId.Value);
           // Speciality s = await Database.Specialitys.Get(a.SpecialityId.Value);
            TrainingInd tr = await Database.TrainingInds.Get(a.Id.Value);
           // tr.Id = a.Id;
            tr.Name = a.Name;
            //tr.Time = t;
            tr.Room = r;
            tr.Coach = c;
            tr.User = u;
         //   tr.Speciality = s;
            await Database.TrainingInds.Update(tr);
            await Database.Save();
        }
    }
}
