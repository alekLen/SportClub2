﻿using AutoMapper;
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
            try
            {
                //TimeT t = await Database.Times.Get(pDto.TimeId);
                Room r = await Database.Rooms.Get(pDto.RoomId);
                Coach c = await Database.Coaches.Get(pDto.CoachId);
                // Group u = await Database.Groups.Get(pDto.GroupId);


                //  Speciality s = await Database.Specialitys.Get(pDto.SpecialityId.Value);//01.02.2024
                var a = new TrainingGroup()
                {
                    //Id = pDto.Id,
                    Name = pDto.Name,
                    Time = pDto.Time,
                    Day = pDto.Day,
                    //Name = pDto.Name,
                    //Number=pDto.Number,
                    //Time = pDto.Time,
                    Room = r,
                    Coach = c,

                    //Group = u,
                    Number = pDto.Number,//01.02.2024
                    users = new(),

                    available = true,
                    // Speciality = s
                };
                await Database.TrainingGroups.AddItem(a);
                await Database.Save();
            }
            catch
            {

            }
        }
       
        public async Task<TrainingGroupDTO> GetTrainingGroup(int id)
        {
            TrainingGroup a = await Database.TrainingGroups.Get(id);
            if (a != null)
            {
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
                    dTO.Id = a.Id;//
                    dTO.Name = a.Name;
                    dTO.Time = a.Time;
                    dTO.Day = a.Day;
                    dTO.DayName = str;
                    dTO.RoomId = a.Room.Id;
                    dTO.RoomName = a.Room.Name;
                    dTO.CoachId = a.Coach.Id;
                    dTO.CoachName = a.Coach.Name;

                    //dTO.GroupId = a.Group.Id;
                    //dTO.GroupName = a.Group.Name;
                    dTO.Number = a.Number;
                    dTO.UsersId = new List<UserDTO>();
                    foreach (var users in a.users)
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
            return null;
        }
        
        public async Task<IEnumerable<TrainingGroupDTO>> GetAllTrainingGroups()
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<TrainingGroup, TrainingGroupDTO>()
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                        .ForMember(dest => dest.DayName, opt => opt.MapFrom(src => src.Day))
                        .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room.Name))
                        .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.Room.Id))
                        .ForMember(dest => dest.CoachName, opt => opt.MapFrom(src => src.Coach.Name))
                        .ForMember(dest => dest.CoachId, opt => opt.MapFrom(src => src.Coach.Id))
                        .ForMember(dest => dest.CoachPhoto, opt => opt.MapFrom(src => src.Coach.Photo))
                        .ForMember(dest => dest.UsersId, opt => opt.MapFrom(src => src.users.Select(user => new UserDTO { Id = user.Id })));
                });
                //var config = new MapperConfiguration(cfg => cfg.CreateMap<TrainingGroup, TrainingGroupDTO>()
                // .ForMember("CoachName", opt => opt.MapFrom(c => c.Coach.Name)).ForMember("CoachPhoto", opt => opt.MapFrom(c => c.Coach.Photo))
                // .ForMember("RoomName", opt => opt.MapFrom(c => c.Room.Name)).ForMember("RoomId", opt => opt.MapFrom(c => c.Room.Id))
                // .ForMember("CoachId", opt => opt.MapFrom(c => c.Coach.Id)).ForMember("Time", opt => opt.MapFrom(c => c.Time))
                // /*.ForMember("UsersId", opt => opt.MapFrom(c => c.users)).ForMember("Number", opt => opt.MapFrom(c => c.Number))*/);
                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<TrainingGroup>, IEnumerable<TrainingGroupDTO>>(await Database.TrainingGroups.GetAll());
            }
            catch { return null; }
        }   
        //    public async Task<IEnumerable<GroupDTO>> GetAllGroups()
             //    {
             //        try
             //        {
             //            var config = new MapperConfiguration(cfg => cfg.CreateMap<Group, GroupDTO>()
             //             /*.ForMember("CoachName", opt => opt.MapFrom(c => c.Coach.Name)).ForMember("UsersId", opt => opt.MapFrom(c => c.users))*/);
             //            var mapper = new Mapper(config);
             //            return mapper.Map<IEnumerable<Group>, IEnumerable<GroupDTO>>(await Database.Groups.GetAll());
             //        }
             //        catch { return null; }
             //    }
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
            Room r = await Database.Rooms.Get(a.RoomId);
            Coach c = await Database.Coaches.Get(a.CoachId); 
            TrainingGroup tr = await Database.TrainingGroups.Get(a.Id);
            tr.Id = a.Id;
            tr.Name = a.Name;
            tr.Number = a.Number;
            //tr.Time = t;
            tr.Room = r;
            tr.Coach = c;
            //tr.Group = u;
            //tr.Speciality = s;


            List<User> list_us = new();
            for (int i = 0; i < a.UsersId.Count; i++)
            {
                list_us.Add(await Database.Users.Get(a.UsersId[i].Id));
            }
            tr.users = list_us;


            await Database.TrainingGroups.Update(tr);
            await Database.Save();
        }

        public async Task<IEnumerable<UserDTO>> GetTrainingGroupUsers(int id)
        {
            TrainingGroup a = await Database.TrainingGroups.Get(id);
            if (a == null)
                return null;

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
                dTO.Time = a.Time;
                dTO.Day = a.Day;
                dTO.DayName = str;
                dTO.RoomId = a.Room.Id;
                dTO.RoomName = a.Room.Name;
                dTO.CoachId = a.Coach.Id;
                dTO.CoachName = a.Coach.Name;
                 
                dTO.Number = a.Number;
                dTO.UsersId = new List<UserDTO>();
                foreach (var users in a.users)
                {
                    UserDTO userDTO = new UserDTO();
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>());
                    var mapper = new Mapper(config);
                    userDTO = mapper.Map<UserDTO>(users);
                    dTO.UsersId.Add(userDTO); 
                }

                return dTO.UsersId; 
            }
            catch { return null; }
        } 
    }
}
