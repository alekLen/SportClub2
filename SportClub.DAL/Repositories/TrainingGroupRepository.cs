using SportClub.DAL.EF;
using SportClub.DAL.Entities;
using SportClub.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using SportClub.DAL.Interfaces;
using SportClub.DAL.EF;
using SportClub.DAL.Entities;

namespace SportClub.DAL.Repositories
{
    public class TrainingGroupRepository : ITrainingGroupRepository
    {
        private SportClubContext db;

        public TrainingGroupRepository(SportClubContext context)
        {
            this.db = context;
        }
        public async Task<IEnumerable<TrainingGroup>> GetAll()
        {
            return await db.TrainingsGroup.Include((p) => p.Room)/*.Include((p) => p.Group)*/.Include((p) => p.Coach)/*.Include((p) => p.Speciality)*/.Include((p) => p.Time).ToListAsync();
        }
        public async Task<IEnumerable<TrainingGroup>> GetAllOfCoach(int id)
        {
            return await db.TrainingsGroup.Where((p) => p.Coach.Id == id)/*.Include((p) => p.Group)*/.Include((p) => p.Coach)/*.Include((p) => p.Speciality)*/.Include((p) => p.Time).ToListAsync();
        }
        public async Task<IEnumerable<TrainingGroup>> GetAllOfClient(User u)
        {
            return await db.TrainingsGroup.Where((p) => p/*.Group.users*/.Users.Contains(u))/*.Include((p) => p.Group)*/.Include((p) => p.Coach)/*.Include((p) => p.Speciality)*/.Include((p) => p.Time).ToListAsync();
        }

        public async Task<TrainingGroup> Get(int id)
        {
            return await db.TrainingsGroup.Include((p) => p.Room)/*.Include((p) => p.Group)*/.Include((p) => p.Coach)/*.Include((p) => p.Speciality)*/.Include((p) => p.Time).FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task AddItem(TrainingGroup c)
        {
            await db.AddAsync(c);
        }
        public async Task Update(TrainingGroup c)
        {
            var f = await db.TrainingsGroup.FirstOrDefaultAsync(m => m.Id == c.Id);
            if (f != null)
            {
                db.TrainingsGroup.Update(c);

            }
        }
        public async Task Delete(int id)
        {
            var c = await db.TrainingsGroup.FindAsync(id);
            if (c != null)
            {
                db.TrainingsGroup.Remove(c);

            }
        }
    }
}
