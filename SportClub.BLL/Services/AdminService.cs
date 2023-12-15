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
    public class AdminService:IAdmin
    {
        IUnitOfWork Database { get; set; }

        public AdminService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public async Task AddAdmin(AdminDTO adminDto)
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
            string password = salt + adminDto.Password;
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var a = new Admin()
            {              
                Name = adminDto.Name,
              //  Surname = adminDto.Surname,
               // Dopname = adminDto.Dopname,
                DateOfBirth = adminDto.DateOfBirth,
                Phone = adminDto.Phone,
                Email = adminDto.Email,
                Age = adminDto.Age,
                Gender = adminDto.Gender,
                Login = adminDto.Login,
                Password =hashedPassword
            };
            await Database.Admins.AddItem(a);
            await Database.Save();
            s.admin = a;
            await Database.Salts.AddItem(s);
            await Database.Save();
        }
        public async Task<AdminDTO> GetAdmin(int id)
        {
            Admin a = await Database.Admins.Get(id);
            if (a == null)
                throw new ValidationException("Wrong artist!", "");
            /* return new AdminDTO
             {
                 Id = a.Id,
                 Name = a.Name,

             };*/
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Admin, AdminDTO>());
                var mapper = new Mapper(config);
                return mapper.Map<AdminDTO>(a);
            }
            catch { return null; }
        }
        public async Task<IEnumerable<AdminDTO>> GetAllAdmins()
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Admin, AdminDTO>());
                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<Admin>, IEnumerable<AdminDTO>>(await Database.Admins.GetAll());
            }
            catch { return null; }
        }
        public async Task DeleteAdmin(int id)
        {
            await Database.Admins.Delete(id);
            await Database.Save();
        }
        public async Task UpdateAdmin(AdminDTO a)
        {
            Admin admin= await Database.Admins.Get(a.Id);
            admin.Name = a.Name;
           // admin.Surname = a.Surname;
           // admin.Dopname = a.Dopname;
            admin.DateOfBirth = a.DateOfBirth;
            admin.Phone = a.Phone;
            admin.Email = a.Email;
            admin.Age = a.Age;
            admin.Gender = a.Gender;
            admin.Login = a.Login;
            if (admin.Password != a.Password)
            {

            }
            await Database.Admins.Update(admin);
            await Database.Save();
        }
        public async Task<bool> CheckPasswordA(AdminDTO u, string p)
        {
            var us = new Admin
            {
                Id = u.Id,
                Login = u.Login,
                Password = u.Password,

            };
            Salt s = await Database.Salts.GetAdminSalt(us);
            string conf = s.salt + p;
            if (BCrypt.Net.BCrypt.Verify(conf, us.Password))
                return true;
            else
                return false;
        }
        public async Task<AdminDTO> GetAdminByLogin(string login)
        {
            try
            {
                Admin a = await Database.Admins.GetAdminLogin(login);
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Admin, AdminDTO>());
                var mapper = new Mapper(config);
                return mapper.Map<AdminDTO>(a);
            }
            catch { return null; }
        }
        public async Task<AdminDTO> GetAdminByEmail(string email)
        {
            try
            {
                Admin a = await Database.Admins.GetAdminEmail(email);
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Admin, AdminDTO>());
                var mapper = new Mapper(config);
                return mapper.Map<AdminDTO>(a);
            }
            catch { return null; }
        }
    }
}
