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
    public class CoachService: ICoach
    {
        IUnitOfWork Database { get; set; }

        public CoachService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public async Task AddCoach(CoachDTO cDto)
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
            string password = salt + cDto.Password;
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var a = new Coach()
            {
                Name = cDto.Name,
                Surname = cDto.Surname,
                Dopname = cDto.Dopname,
                DateOfBirth = cDto.DateOfBirth,
                Phone = cDto.Phone,
                Email = cDto.Email,
                Age = cDto.Age,
                Gender = cDto.Gender,
                Login = cDto.Login,
                Password = hashedPassword
            };
            await Database.Coaches.AddItem(a);
            await Database.Save();
            s.coach = a;
            await Database.Salts.AddItem(s);
            await Database.Save();
        }
        public async Task<CoachDTO> GetCoach(int id)
        {
            Coach a = await Database.Coaches.Get(id);
            if (a == null)
                throw new ValidationException("Wrong artist!", "");
            /* return new AdminDTO
             {
                 Id = a.Id,
                 Name = a.Name,

             };*/
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Coach, CoachDTO>());
                var mapper = new Mapper(config);
                return mapper.Map<CoachDTO>(a);
            }
            catch { return null; }
        }
        public async Task<IEnumerable<CoachDTO>> GetAllCoaches()
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Coach, CoachDTO>());
                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<Coach>, IEnumerable<CoachDTO>>(await Database.Coaches.GetAll());
            }
            catch { return null; }
        }
        public async Task DeleteCoach(int id)
        {
            await Database.Coaches.Delete(id);
            await Database.Save();
        }
        public async Task UpdateCoach(CoachDTO a)
        {
            Coach c = await Database.Coaches.Get(a.Id);
            c.Name = a.Name;
            c.Surname = a.Surname;
            c.Dopname = a.Dopname;
            c.DateOfBirth = a.DateOfBirth;
            c.Phone = a.Phone;
            c.Email = a.Email;
            c.Age = a.Age;
            c.Gender = a.Gender;
            c.Login = a.Login;
            if (c.Password != a.Password)
            {

            }
            await Database.Coaches.Update(c);
            await Database.Save();
        }
        public async Task<bool> CheckPasswordC(CoachDTO u, string p)
        {
            var us = new Coach
            {
                Id = u.Id,
                Login = u.Login,
                Password = u.Password,

            };
            Salt s = await Database.Salts.GetCoachSalt(us);
            string conf = s.salt + p;
            if (BCrypt.Net.BCrypt.Verify(conf, us.Password))
                return true;
            else
                return false;
        }
        public async Task<CoachDTO> GetCoachByLogin(string login)
        {
            try
            {
                Coach a = await Database.Coaches.GetCoachLogin(login);
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Coach, CoachDTO>());
                var mapper = new Mapper(config);
                return mapper.Map<CoachDTO>(a);
            }
            catch { return null; }
        }
    }
}
