using SportClub.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        ISetGetRepository<Coach> Coaches { get; }
        ISetGetRepository<Post> Posts { get; }
        ISetGetRepository<Speciality> Specialitys { get; }
        ISetGetRepository<User> Users { get; }
        ISetGetRepository<Admin> Admins { get; }
        ISetGetRepository<Salt> Salts { get; }
        ISetGetRepository<Room> Rooms { get; }
        ISetGetRepository<Group> Groups { get; }
        ISetGetRepository<Shedule> Shedules { get; }
        ISetGetRepository<TimeT> Times { get; }
        ISetGetRepository<Timetable> Timetables { get; }
        ISetGetRepository<Training> Trainings { get; }
        ISetGetRepository<TypeOfTraining> TypeOfTrainings { get; }
    }
}
