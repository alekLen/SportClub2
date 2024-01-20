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

        //public async Task AddTrainingGroup(TrainingGroupDTO pDto)
        //{
            ////TimeT t = await Database.Times.Get(pDto.TimeId);
            //Room r = await Database.Rooms.Get(pDto.RoomId);
            //Coach c = await Database.Coaches.Get(pDto.CoachId.Value);
            ////Group u = await Database.Groups.Get(pDto.GroupId.Value);
            ////Speciality s = await Database.Specialitys.Get(pDto.SpecialityId.Value);
            //var a = new TrainingGroup()
            //{
            //    Name = pDto.Name,
            //    //Number=pDto.Number,
            //    //Time = t,
            //    Room = r,
            //    Coach = c,
            //    //Group = u,
            //    //Speciality = s
            //};
            //await Database.TrainingGroups.AddItem(a);
            //await Database.Save();
             
        //}
        public async Task AddTrainingGroup(TrainingGroupDTO pDto)
        {
            //TimeT t = await Database.Times.Get(pDto.TimeId);
            Room r = await Database.Rooms.Get(pDto.RoomId);
            Coach c = await Database.Coaches.Get(pDto.CoachId.Value);
            List<User> u = null;
            try
            {
                if (pDto.UsersId.Count != null) {
                    u = new();
                    foreach (var user in pDto.UsersId)
                    {
                        u.Add(await Database.Users.Get(user.Id));
                    }
                    //u = await Database.Users.Get(pDto.UserId.Value);
                }
            }
            catch { }
            //  Speciality s = await Database.Specialitys.Get(pDto.SpecialityId.Value);
            var a = new TrainingGroup()
            {
                //Name = pDto.Name,
                Time = pDto.TimeName,
                Day = pDto.Day,
                Name = pDto.Name,
                //Number=pDto.Number,
                //Time = pDto.Time,
                Room = r,
                Coach = c,
                Users = u,
                // Speciality = s
            };
            await Database.TrainingGroups.AddItem(a);
            await Database.Save();
        }
        //public async Task<TrainingGroupDTO> GetTrainingGroup(int id)
        //{
        //    TrainingGroup a = await Database.TrainingGroups.Get(id);
        //    if (a == null)
        //        return null;
        //    /* return new AdminDTO
        //     {
        //         Id = a.Id,
        //         Name = a.Name,

        //     };*/
        //    try
        //    {
        //        var config = new MapperConfiguration(cfg => cfg.CreateMap<TrainingGroup, TrainingGroupDTO>()
        //        .ForMember("CoachName", opt => opt.MapFrom(c => c.Coach.Name))/*.ForMember("SpecialityName", opt => opt.MapFrom(c => c.Speciality.Name))
        //        .ForMember("GroupName", opt => opt.MapFrom(c => c.Group.Name))*/.ForMember("RoomName", opt => opt.MapFrom(c => c.Room.Name))
        //        /*.ForMember("TimeId", opt => opt.MapFrom(c => c.Time.Id))*/.ForMember("RoomId", opt => opt.MapFrom(c => c.Room.Id))
        //        //.ForMember("SpecialityId", opt => opt.MapFrom(c => c.Speciality.Id)).ForMember("TimeName", opt => opt.MapFrom(c => c.Time.StartTime +"/"+ c.Time.EndTime))
        //        .ForMember("CoachId", opt => opt.MapFrom(c => c.Coach.Id))/*.ForMember("GroupId", opt => opt.MapFrom(c => c.Group.Id))*/);
        //        var mapper = new Mapper(config);
        //        return mapper.Map<TrainingGroupDTO>(a);
        //    }
        //    catch { return null; }
        //}
        public async Task<TrainingGroupDTO> GetTrainingGroup(int id)
        {
            TrainingGroup a = await Database.TrainingGroups.Get(id);
            if (a == null)
                throw new ValidationException("Wrong", "");
            string str = "";
            if (a.Day == 0) str = "Понедельник";
            else if (a.Day == 1) str = "Вторник";
            else if (a.Day == 2) str = "Среда";
            else if (a.Day == 3) str = "Четверг";
            else if (a.Day == 4) str = "Пятница";
            else if (a.Day == 5) str = "Суббота";
            else if (a.Day == 6) str = "Воскресенье";
            
            try
            {
                TrainingGroupDTO dTO = new TrainingGroupDTO();
                dTO.Name = a.Name;
                dTO.TimeName = a.Time;
                dTO.Day = a.Day;
                dTO.DayName = str;
                dTO.RoomId = a.Room.Id;
                dTO.RoomName = a.Room.Name;
                dTO.CoachId = a.Coach.Id;
                dTO.CoachName = a.Coach.Name;
                dTO.UsersId = new List<UserDTO>();
                foreach (var users in a.Users)
                {
                    UserDTO userDTO = new UserDTO();
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>());
                    var mapper = new Mapper(config);
                    userDTO = mapper.Map<UserDTO>(users);
                    dTO.UsersId.Add(userDTO); 
                } 
                return dTO;// 
            }
            catch { return null; }
        }
        //public async Task<IEnumerable<TrainingGroupDTO>> GetAllTrainingGroups()
        //{
        //    try
        //    {
        //        var config = new MapperConfiguration(cfg => cfg.CreateMap<TrainingGroup, TrainingGroupDTO>()
        //         .ForMember("CoachName", opt => opt.MapFrom(c => c.Coach.Name)).ForMember("RoomId", opt => opt.MapFrom(c => c.Room.Id))
        //         .ForMember("CoachId", opt => opt.MapFrom(c => c.Coach.Id)));
        //        var mapper = new Mapper(config);
        //        return mapper.Map<IEnumerable<TrainingGroup>, IEnumerable<TrainingGroupDTO>>(await Database.TrainingGroups.GetAll());
        //    }
        //    catch { return null; }
        //}

        public async Task<IEnumerable<TrainingGroupDTO>> GetAllTrainingGroups()
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<TrainingGroup, TrainingGroupDTO>()
                 .ForMember("CoachName", opt => opt.MapFrom(c => c.Coach.Name))
                 .ForMember("UsersId", opt => opt.MapFrom(c => c.Users)).ForMember("CoachPhoto", opt => opt.MapFrom(c => c.Coach.Photo))
                 .ForMember("RoomName", opt => opt.MapFrom(c => c.Room.Name)).ForMember("RoomId", opt => opt.MapFrom(c => c.Room.Id))
                 .ForMember("CoachId", opt => opt.MapFrom(c => c.Coach.Id)).ForMember("TimeName", opt => opt.MapFrom(c => c.Time)));
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
                 .ForMember("CoachName", opt => opt.MapFrom(c => c.Coach.Name)).ForMember("RoomId", opt => opt.MapFrom(c => c.Room.Id))
                 .ForMember("CoachId", opt => opt.MapFrom(c => c.Coach.Id)));
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
                 .ForMember("CoachName", opt => opt.MapFrom(c => c.Coach.Name)).ForMember("RoomId", opt => opt.MapFrom(c => c.Room.Id))
                 .ForMember("CoachId", opt => opt.MapFrom(c => c.Coach.Id)));
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
            //TimeT t = await Database.Times.Get(a.TimeId);
            Room r = await Database.Rooms.Get(a.RoomId);
            Coach c = await Database.Coaches.Get(a.CoachId.Value);
            //Group u = await Database.Groups.Get(a.GroupId.Value);
            //Speciality s = await Database.Specialitys.Get(a.SpecialityId.Value);
            TrainingGroup tr = await Database.TrainingGroups.Get(a.Id);
            tr.Id = a.Id;
            tr.Name = a.Name;
            //tr.Number = a.Number;
            //tr.Time = t;
            tr.Room = r;
            tr.Coach = c;
            //tr.Group = u;
            //tr.Speciality = s;
            await Database.TrainingGroups.Update(tr);
            await Database.Save();
        }
    }
}
