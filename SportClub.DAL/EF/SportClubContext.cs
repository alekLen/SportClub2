using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportClub.DAL.Entities;

namespace SportClub.DAL.EF
{
    public class SportClubContext : DbContext
    {
        public SportClubContext(DbContextOptions<SportClubContext> options)
               : base(options)
        {
            if (Database.EnsureCreated())
            {
                string pass = "Qwerty-123";
                byte[] saltbuf = new byte[16];
                RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
                randomNumberGenerator.GetBytes(saltbuf);
                StringBuilder sb = new StringBuilder(16);
                for (int i = 0; i < 16; i++)
                    sb.Append(string.Format("{0:X2}", saltbuf[i]));
                string salt = sb.ToString();
                Salt s = new();
                s.salt = salt;
                string password = salt + pass;
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                Admin a = new Admin { Name = "Elena",/* Surname = "Alekseeva",*/ Login = "admin", Password = hashedPassword, DateOfBirth = "27-11-2002", Email = "asd@gmail.com", Phone = "0971234567", Gender = "female", Level = 2 };
                Admins.Add(a);
                SaveChanges();
                s.admin = a;
                Salts.Add(s);
                SaveChanges();
                TimeT t = new TimeT { StartTime = "9:00", EndTime = "10:00" };
                TimeT t1 = new TimeT { StartTime = "10:00", EndTime = "11:00" };
                Times.Add(t);
                Times.Add(t1);
                SaveChanges();
                Room r1 = new Room { Name = "Room number 1" };
                Room r2 = new Room { Name = "Room number 2" };
                Room r3 = new Room { Name = "Room number 3" };
                Rooms.Add(r1); Rooms.Add(r2); Rooms.Add(r3);
                SaveChanges();
            }
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Salt> Salts { get; set; }
        public DbSet<Speciality> Specialitys { get; set; }
        public DbSet<Post> Posts { get; set; }
        //public DbSet<SportClub.DAL.Entities.Group> Groups { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<TrainingInd> TrainingsInd { get; set; }
        public DbSet<TrainingGroup> TrainingsGroup { get; set; }
        public DbSet<TimeT> Times { get; set; }
        public DbSet<Shedule> Shedules { get; set; }

        public DbSet<TypeOfTraining> TypeOfTrainings { get; set; }
        public DbSet<Timetable> Timetables { get; set; }
    }
}
