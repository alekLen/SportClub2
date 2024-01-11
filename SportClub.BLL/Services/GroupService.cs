using SportClub.BLL.Interfaces;
using SportClub.BLL.Infrastructure;
using SportClub.DAL.Interfaces;
using System.Text;
using SportClub.BLL.DTO;
using SportClub.DAL.Entities;
using System.Security.Cryptography;
using AutoMapper;


namespace SportClub.BLL.Services
{
    public class GroupService:IGroup
    {
        IUnitOfWork Database { get; set; }

        public GroupService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public async Task AddGroup(GroupDTO groupDto)
        {
            Coach coach = await Database.Coaches.Get(groupDto.CoachId);
            List<User> list_us = new();
            for (int i=0; i < groupDto.UsersId.Count;i++)
            {
                list_us.Add(await Database.Users.Get(groupDto.UsersId[i].Id));
            }

            var a = new Group()
            {
                Name = groupDto.Name,
                Number = groupDto.Number,
                Coach = coach,
                users = list_us
            };
            await Database.Groups.AddItem(a);
            await Database.Save();

            
        }

        public async Task<GroupDTO> GetGroup(int id)
        {
            Group a = await Database.Groups.Get(id);
            if (a == null)
                return null;
            /* return new AdminDTO
             {
                 Id = a.Id,
                 Name = a.Name,

             };*/
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Group, GroupDTO>()
                .ForMember("CoachName", opt => opt.MapFrom(c => c.Coach.Name)).ForMember("UsersId", opt => opt.MapFrom(c => c.users)));
                var mapper = new Mapper(config);
                return mapper.Map<GroupDTO>(a);
            }
            catch { return null; }
        }

        public async Task<IEnumerable<GroupDTO>> GetAllGroups()
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Group, GroupDTO>()
                 .ForMember("CoachName", opt => opt.MapFrom(c => c.Coach.Name))/*.ForMember("UsersId", opt => opt.MapFrom(c => c.users))*/);
                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<Group>, IEnumerable<GroupDTO>>(await Database.Groups.GetAll());
            }
            catch { return null; }
        }
        public async Task DeleteGroup(int id)
        {
            Group group = await Database.Groups.Get(id); 
            await Database.Groups.Delete(id);//
            await Database.Save();//
        }
        public async Task UpdateGroup(GroupDTO a)
        {
            Group group = await Database.Groups.Get(a.Id);
            group.Name = a.Name;
            // admin.Surname = a.Surname;
            // admin.Dopname = a.Dopname;
            group.Number = a.Number;
            Coach coach = await Database.Coaches.Get(a.CoachId);
            group.Coach = coach;
            
            List<User> list_us = new();
            for(int i=0; i< a.UsersId.Count;i++)
            {
                list_us.Add(await Database.Users.Get(a.UsersId[i].Id));
            }
            group.users = list_us;
            //group.Age = a.Age;
            //group.Gender = a.Gender;
            //group.Login = a.Login;
             
            await Database.Groups.Update(group);
            await Database.Save();
        }

        public async Task<GroupDTO> GetGroupByName(string name)
        {
            try
            {
                Group a = await Database.Groups.GetGroupName(name);
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Group, GroupDTO>());
                var mapper = new Mapper(config);
                return mapper.Map<GroupDTO>(a);
            }
            catch { return null; }
        }

    }
}
