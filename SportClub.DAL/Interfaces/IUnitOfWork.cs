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
        ICoachRepository Coaches { get; }
        ISetGetRepository<Post> Posts { get; }
        ISetGetRepository<Speciality> Specialitys { get; }
        IUserRepository Users { get; }
        IAdminRepository Admins { get; }
        ISaltRepository Salts { get; }
        ISetGetRepository<Room> Rooms { get; }
        ISetGetRepository<Group> Groups { get; }
        ISetGetRepository<Shedule> Shedules { get; }
        ITimeTRepository Times { get; }
        ISetGetRepository<Timetable> Timetables { get; }
        ITrainingIndRepository TrainingInds { get; }
        ITrainingGroupRepository TrainingGroups { get; }
        ISetGetRepository<TypeOfTraining> TypeOfTrainings { get; }
        Task Save();
    }
}
