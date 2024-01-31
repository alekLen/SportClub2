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
                Admin a = new Admin { Name = "Elena",/* Surname = "Alekseeva",*/ Login = "admin", Password = hashedPassword, DateOfBirth = "27-11-2002", Email = "asd@gmail.com", Phone = "0971234567", Gender = "женский", Level = 2 };
                Admins.Add(a);
                SaveChanges();
                s.admin = a;
                Salts.Add(s);
                SaveChanges();
                TimeT t = new TimeT { StartTime = "09:00", EndTime = "10:00" };
                TimeT t1 = new TimeT { StartTime = "10:00", EndTime = "11:00" };
                TimeT t2 = new TimeT { StartTime = "10:00", EndTime = "11:00" };
                TimeT t3 = new TimeT { StartTime = "11:00", EndTime = "12:00" };
                TimeT t4 = new TimeT { StartTime = "12:00", EndTime = "13:00" };
                TimeT t5 = new TimeT { StartTime = "13:00", EndTime = "14:00" };
                TimeT t6 = new TimeT { StartTime = "14:00", EndTime = "15:00" };
                TimeT t7 = new TimeT { StartTime = "15:00", EndTime = "16:00" };
                TimeT t8 = new TimeT { StartTime = "16:00", EndTime = "17:00" };
                TimeT t9 = new TimeT { StartTime = "17:00", EndTime = "18:00" };
                TimeT t10 = new TimeT { StartTime = "18:00", EndTime = "19:00" };
                Times.Add(t);
                Times.Add(t1);
                Times.Add(t2);
                Times.Add(t3);
                Times.Add(t4);
                Times.Add(t5);
                Times.Add(t6);
                Times.Add(t7);
                Times.Add(t8);
                Times.Add(t9);
                Times.Add(t10);
                SaveChanges();
                Post p= new Post { Name="Тренер"};
                Post p1 = new Post { Name = "Старший тренер" };
                Posts.Add(p);
                Posts.Add(p1);
                Speciality sp=new Speciality { Name="Кроссфит"};
                Speciality sp1 = new Speciality { Name = "Йога" };
                Speciality sp2 = new Speciality { Name = "Пилатес" };
                Specialitys.Add(sp);
                Specialitys.Add(sp1);
                Specialitys.Add(sp2);
                Room r1 = new Room { Name = "Зал 1",Photo= "/Rooms/4.jpg" };
                Room r2 = new Room { Name = "Зал 2", Photo = "/Rooms/3.jpg" };
                Room r3 = new Room { Name = "Зал 3", Photo = "/Rooms/7.jpg" };
                Room r4 = new Room { Name = "Зал 4", Photo = "/Rooms/йога_зал.png" };
                Rooms.Add(r1); Rooms.Add(r2); Rooms.Add(r3); Rooms.Add(r4);
                Coach c = new Coach { Name = "Иванов Иван", Login = "ivan", Password = hashedPassword,
                    DateOfBirth = "27-11-2002", Email = "iv@gmail.com", Phone = "+380971234567",
                    Gender = "мужской", Photo = "/Coaches/тренер6.jpg", Description = "Тренер по кроссфиту с 2-летним опытом работы в фитнес-клубе премиум-класса. Составил эффективный план индивидуальных тренировок для 15 клиентов клуба. Дважды занимал 1 место в областных соревнованиях по кроссфиту среди профессионалов. Получил более 50 положительных отзывов на сайте клуба."
                };
                    Coach c1 = new Coach
                {
                    Name = "Сидорова Алена",
                    Login = "alena",
                    Password = hashedPassword,
                    DateOfBirth = "12-03-2001",
                    Email = "al@gmail.com",
                    Phone = "+380971234567",
                    Gender = "женский",
                    Photo = "/Coaches/тренер_7.jpg",
                    Description = "Персональный фитнес-тренер с 5-летним стажем работы в фитнес-центрах. Обладаю проверенной репутацией в оказании помощи клиентам в достижении целей в фитнесе с помощью индивидуальных программ упражнений и планов диеты. Разрабатываю новые программы."
                };
                Coach c2 = new Coach
                {
                    Name = "Петров Сергей",
                    Login = "petro",
                    Password = hashedPassword,
                    DateOfBirth = "22-09-2003",
                    Email = "pt@gmail.com",
                    Phone = "+380971234567",
                    Gender = "мужской",
                    Photo = "/Coaches/тренер2.jpg",
                    Description = "Фитнес-тренер с 7-летним опытом работы в фитнес-центре. Имею 1-й взрослый разряд по плаванию. Разработал комплекс упражнений для начинающих, позволяющий снизить вес в 2 раза быстрее традиционных методик. Занимал 1 место в областных соревнованиях по кроссфиту в 2020 и 2021 годах."
                };
                    Coach c3 = new Coach
                {
                    Name = "Загренко Ирина",
                    Login = "irina",
                    Password = hashedPassword,
                    DateOfBirth = "27-11-2000",
                    Email = "ira@gmail.com",
                    Phone = "+380971234567",
                    Gender = "женский",
                    Photo = "/Coaches/тренер_11.jpg",
                    Description = "Самодостаточный и преданный своему делу инструктор по йоге с 12-летним опытом работы по укреплению здоровья и правильному питанию . Успешно проводил четыре занятия в день для клиентов разных возрастных групп, включая детей и пожилых людей. "
                    };
                Coaches.Add(c);
                Coaches.Add(c1);
                Coaches.Add(c2);
                Coaches.Add(c3);

                SaveChanges();
            }
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Salt> Salts { get; set; }
        public DbSet<Speciality> Specialitys { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<SportClub.DAL.Entities.Group> Groups { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<TrainingInd> TrainingsInd { get; set; }
        public DbSet<TrainingGroup> TrainingsGroup { get; set; }
        public DbSet<TimeT> Times { get; set; }
        public DbSet<Shedule> Shedules { get; set; }
        public DbSet<SkipShedule> SkipShedule { get; set; }

        public DbSet<TypeOfTraining> TypeOfTrainings { get; set; }
        public DbSet<Timetable> Timetables { get; set; }
    }
}
