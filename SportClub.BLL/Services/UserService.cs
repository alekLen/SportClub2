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
    public class UserService : IUser
    {
        IUnitOfWork Database { get; set; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public async Task AddUser(UserDTO uDto)
        {
            byte[] saltbuf = new byte[16];
            RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(saltbuf);
            StringBuilder sb = new StringBuilder(16);
            for (int i = 0; i < 16; i++)
                sb.Append(string.Format("{0:X2}", saltbuf[i]));
            string salt = sb.ToString();
            Salt s = new();
            s.salt = salt;
            string password = salt + uDto.Password;
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var a = new User()
            {
                Name = uDto.Name,
              //  Surname = uDto.Surname,
               // Dopname = uDto.Dopname,
                DateOfBirth = uDto.DateOfBirth,
                Phone = uDto.Phone,
                Email = uDto.Email,
                Age = uDto.Age,
                Gender = uDto.Gender,
                Login = uDto.Login,
                Password = hashedPassword
            };
            await Database.Users.AddItem(a);
            await Database.Save();
            s.user = a;
            await Database.Salts.AddItem(s);
            await Database.Save();
        }
        public async Task ChangeUserPassword(UserDTO uDto,string pass)
        {
            User user = await Database.Users.Get(uDto.Id);
            Salt s = await Database.Salts.GetUserSalt(user);
            string password = s.salt + pass;
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            user.Password = hashedPassword;
            await Database.Users.Update(user);
            await Database.Save();
        }
        public async Task<UserDTO> GetUser(int id)
        {
            User a = await Database.Users.Get(id);
            if (a == null)
                throw new ValidationException("Wrong!", "");
            /* return new AdminDTO
             {
                 Id = a.Id,
                 Name = a.Name,

             };*/
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>());
                var mapper = new Mapper(config);
                return mapper.Map<UserDTO>(a);
            }
            catch { return null; }
        }
        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>());
                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(await Database.Users.GetAll());
            }
            catch { return null; }
        }
        public async Task DeleteUser(int id)
        {
            User user = await Database.Users.Get(id);
            Salt salt = await Database.Salts.GetUserSalt(user);
            await Database.Salts.Delete(salt.Id);
            await Database.Users.Delete(id);
            await Database.Save();
        }
        public async Task UpdateUser(UserDTO a)
        {
            User u = await Database.Users.Get(a.Id);
            u.Name = a.Name;
          //  u.Surname = a.Surname;
          //  u.Dopname = a.Dopname;
            u.DateOfBirth = a.DateOfBirth;
            u.Phone = a.Phone;
            u.Email = a.Email;
            u.Age = a.Age;
            u.Gender = a.Gender;
            u.Login = a.Login;
            if (u.Password != a.Password)
            {

            }
            await Database.Users.Update(u);
            await Database.Save();
        }
        public async Task<bool> CheckPasswordU(UserDTO u, string p)
        {
            var us = new User
            {
                Id = u.Id,
                Login = u.Login,
                Password = u.Password,

            };
            Salt s = await Database.Salts.GetUserSalt(us);
            string conf = s.salt + p;
            if (BCrypt.Net.BCrypt.Verify(conf, us.Password))
                return true;
            else
                return false;
        }
        public async Task<UserDTO> GetUserByLogin(string login)
        {
            try
            {
                User a = await Database.Users.GetUserLogin(login);
                var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>());
                var mapper = new Mapper(config);
                return mapper.Map<UserDTO>(a);
            }
            catch { return null; }
        }
        public async Task<UserDTO> GetUserByEmail(string email)
        {
            try
            {
                User a = await Database.Users.GetUserEmail(email);
                var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>());
                var mapper = new Mapper(config);
                return mapper.Map<UserDTO>(a);
            }
            catch { return null; }
        }
    }
}
