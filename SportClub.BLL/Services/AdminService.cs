using SportClub.BLL.Interfaces;
using SportClub.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportClub.BLL.DTO;
using SportClub.DAL.Entities;
using System.Security.Cryptography;

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
                Surname = adminDto.Surname,
                Dopname = adminDto.Dopname,
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
    }
}
