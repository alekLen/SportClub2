using Microsoft.EntityFrameworkCore;
using SportClub.DAL.Interfaces;
using SportClub.DAL.EF;
using SportClub.DAL.Entities;
using System.Linq;

namespace SportClub.DAL.Repositories
{
    public class TrainingIndRepository : ITrainingIndRepository 
    {
        private SportClubContext db;

        public TrainingIndRepository(SportClubContext context)
        {
            this.db = context;
        }
        public async Task<IEnumerable<TrainingInd>> GetAll()
        {
            return await db.TrainingsInd.Include((p) => p.User).Include((p) => p.Coach).Include((p) => p.Room).ToListAsync();
        }
        public async Task<IEnumerable<TrainingInd>> GetAllOfCoach(int id)
        {
            return await db.TrainingsInd.Where((p) => p.Coach.Id == id).Include((p) => p.User).Include((p) => p.Coach).Include((p) => p.Room).ToListAsync();
        }
        public async Task<IEnumerable<TrainingInd>> GetAllOfClient(int id)
        {
            return await db.TrainingsInd.Where((p) => p.User.Id==id).Include((p) => p.User).Include((p) => p.Coach).Include((p) => p.Room).ToListAsync();
        }

        public async Task<TrainingInd> Get(int id)
        {
            return await db.TrainingsInd.Include((p) => p.User).Include((p) => p.Coach).Include((p) => p.Room).FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task AddItem(TrainingInd c)
        {
            await db.AddAsync(c);
        }
        public async Task Update(TrainingInd c)
        {
            var f = await db.TrainingsInd.FirstOrDefaultAsync(m => m.Id == c.Id);
            if (f != null)
            {
                db.TrainingsInd.Update(c);

            }
        }
        public async Task Delete(int id)
        {
            var c = await db.TrainingsInd.FindAsync(id);
            if (c != null)
            {
                db.TrainingsInd.Remove(c);

            }
        }
    }
}
