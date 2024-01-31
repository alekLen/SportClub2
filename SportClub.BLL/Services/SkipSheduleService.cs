using AutoMapper;
using SportClub.BLL.DTO;
using SportClub.BLL.Interfaces;
using SportClub.DAL.Entities;
using SportClub.DAL.Interfaces;
using SportClub.BLL.Infrastructure;
using System.Security.Cryptography;
using System.Text;

namespace SportClub.BLL.Services
{
    public class SkipSheduleService : ISkipShedule
    {
        IUnitOfWork Database { get; set; }

        public SkipSheduleService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public async Task AddSkipShedule(SkipSheduleDTO sk)
        {
            try
            { 
                TrainingGroup tg = await Database.TrainingGroups.Get(sk.trainingGroupId);
                User c = await Database.Users.Get(sk.UserId);
                //Group u = await Database.Groups.Get(pDto.GroupId);
                 
                var a = new SkipShedule()
                { 
                    trainingGroup = tg,
                    User = c,
                    SkipMonday = sk.SkipMonday, 
                    SkipWednesday = sk.SkipWednesday,
                    SkipTuesday = sk.SkipTuesday,
                    SkipThursday = sk.SkipThursday,
                    SkipFriday = sk.SkipFriday, 
                    SkipSunday = sk.SkipSunday,
                    SkipSaturday = sk.SkipSaturday,
                };
                await Database.SkipShedule.AddItem(a);
                await Database.Save();
            }
            catch
            {

            }
        }
        //public async Task ChangeUserPassword(UserDTO uDto,string pass)
        //{
        //    User user = await Database.Users.Get(uDto.Id);
        //    Salt s = await Database.Salts.GetUserSalt(user);
        //    string password = s.salt + pass;
        //    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        //    user.Password = hashedPassword;
        //    await Database.Users.Update(user);
        //    await Database.Save();
        //}
        public async Task<SkipSheduleDTO> GetSkipShedule(int id)
        {
            SkipShedule a = await Database.SkipShedule.Get(id);
            if (a == null)
                throw new ValidationException("Wrong!", "");
            /* return new AdminDTO
             {
                 Id = a.Id,
                 Name = a.Name,

             };*/
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<SkipShedule, SkipSheduleDTO>());
                var mapper = new Mapper(config);
                return mapper.Map<SkipSheduleDTO>(a);
            }
            catch { return null; }
        }
        public async Task<SkipSheduleDTO> GetSkipSheduleByTrainingId(int id)
        {
            //одна группа и одна неделя пропусков(SkipSheduleDTO)
            //добавити поле в класс GroupDTO



            SkipShedule a = await Database.SkipShedule.Get(id);
            if (a == null)
                throw new ValidationException("Wrong!", "");
            /* return new AdminDTO
             {
                 Id = a.Id,
                 Name = a.Name,

             };*/
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<SkipShedule, SkipSheduleDTO>());
                var mapper = new Mapper(config);
                return mapper.Map<SkipSheduleDTO>(a);
            }
            catch { return null; }
        }
        public async Task DeleteSkipShedule(int id)
        {
            User user = await Database.Users.Get(id);
            Salt salt = await Database.Salts.GetUserSalt(user);
            await Database.Salts.Delete(salt.Id);
            await Database.Users.Delete(id);
            await Database.Save();
        }
        public async Task UpdateSkipShedule(SkipSheduleDTO sk)
        {
            SkipShedule u = await Database.SkipShedule.Get(sk.Id);
            TrainingGroup tg = await Database.TrainingGroups.Get(sk.trainingGroupId);
            User c = await Database.Users.Get(sk.UserId);

            u.trainingGroup = tg;
            u.User = c;
            u.SkipMonday = sk.SkipMonday; 
            u.SkipWednesday = sk.SkipWednesday;
            u.SkipTuesday = sk.SkipTuesday;
            u.SkipThursday = sk.SkipThursday;
            u.SkipFriday = sk.SkipFriday;
            u.SkipSunday = sk.SkipSunday;
            u.SkipSaturday = sk.SkipSaturday;


            await Database.SkipShedule.Update(u);
            await Database.Save();
        }

       
        //public async Task<bool> CheckPasswordU(UserDTO u, string p)
        //{
        //    var us = new User
        //    {
        //        Id = u.Id,
        //        Login = u.Login,
        //        Password = u.Password,

        //    };
        //    Salt s = await Database.Salts.GetUserSalt(us);
        //    string conf = s.salt + p;
        //    if (BCrypt.Net.BCrypt.Verify(conf, us.Password))
        //        return true;
        //    else
        //        return false;
        //}
        //public async Task<UserDTO> GetUserByLogin(string login)
        //{
        //    try
        //    {
        //        User a = await Database.Users.GetUserLogin(login);
        //        var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>());
        //        var mapper = new Mapper(config);
        //        return mapper.Map<UserDTO>(a);
        //    }
        //    catch { return null; }
        //}
        //public async Task<UserDTO> GetUserByEmail(string email)
        //{
        //    try
        //    {
        //        User a = await Database.Users.GetUserEmail(email);
        //        var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>());
        //        var mapper = new Mapper(config);
        //        return mapper.Map<UserDTO>(a);
        //    }
        //    catch { return null; }
        //}
    }
}
