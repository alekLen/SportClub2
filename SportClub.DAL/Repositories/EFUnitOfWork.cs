﻿using SportClub.DAL.EF;
using SportClub.DAL.Entities;
using SportClub.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private SportClubContext db;
        private CoachRepository coachRepository;
        private PostRepository postRepository;
        private AdminRepository adminRepository;
        private UserRepository userRepository;
        private GroupRepository groupRepository;
        private RoomRepository roomRepository;
        private SaltRepository saltRepository;
        private SheduleRepository sheduleRepository;
        private SpecialityRepository specialityRepository;
        private TimeTRepository timeRepository;
        private TimetableRepository timetableRepository;
        private TrainingRepository trainingRepository;
        private TypeOfTrainingRepository typeoftrainingRepository;


        public EFUnitOfWork(SportClubContext context)
        {

            db = context;
        }

        public ISetGetRepository<Coach> Coaches
        {
            get
            {
                if (coachRepository == null)
                    coachRepository = new CoachRepository(db);
                return coachRepository;
            }
        }
        public ISetGetRepository<Post> Posts
        {
            get
            {
                if (postRepository == null)
                    postRepository = new PostRepository(db);
                return postRepository;
            }
        }
        public ISetGetRepository<Admin> Admins
        {
            get
            {
                if (adminRepository == null)
                    adminRepository = new AdminRepository(db);
                return adminRepository;
            }
        }
        public ISetGetRepository<User> Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(db);
                return userRepository;
            }
        }
        public ISetGetRepository<Group> Groups
        {
            get
            {
                if (groupRepository == null)
                    groupRepository = new GroupRepository(db);
                return groupRepository;
            }
        }
        public ISetGetRepository<Room> Rooms
        {
            get
            {
                if (roomRepository == null)
                    roomRepository = new RoomRepository(db);
                return roomRepository;
            }
        }
        public ISetGetRepository<Salt> Salts
        {
            get
            {
                if (saltRepository == null)
                    saltRepository = new SaltRepository(db);
                return saltRepository;
            }
        }
        public ISetGetRepository<Shedule> Shedules
        {
            get
            {
                if (sheduleRepository == null)
                    sheduleRepository = new SheduleRepository(db);
                return sheduleRepository;
            }
        }
        public ISetGetRepository<Speciality> Specialitys
        {
            get
            {
                if (specialityRepository == null)
                    specialityRepository = new SpecialityRepository(db);
                return specialityRepository;
            }
        }
        public ISetGetRepository<TimeT> Times
        {
            get
            {
                if (timeRepository == null)
                    timeRepository = new TimeTRepository(db);
                return timeRepository;
            }
        }
        public ISetGetRepository<Timetable> Timetables
        {
            get
            {
                if (timetableRepository == null)
                    timetableRepository = new TimetableRepository(db);
                return timetableRepository;
            }
        }
        public ISetGetRepository<Training> Trainings
        {
            get
            {
                if (trainingRepository == null)
                    trainingRepository = new TrainingRepository(db);
                return trainingRepository;
            }
        }
        public ISetGetRepository<TypeOfTraining> TypeOfTrainings
        {
            get
            {
                if (typeoftrainingRepository == null)
                    typeoftrainingRepository = new TypeOfTrainingRepository(db);
                return typeoftrainingRepository;
            }
        }

        public async Task Save()
        {
            await db.SaveChangesAsync();
        }
    }
}
