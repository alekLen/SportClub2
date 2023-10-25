using Microsoft.EntityFrameworkCore;
using SportClub.DAL.Interfaces;
using SportClub.DAL.EF;
using SportClub.DAL.Entities;

namespace SportClub.DAL.Repositories
{
    public class TrainingRepository : ISetGetRepository<Training>
    {
        private SportClubContext db;

        public TrainingRepository(SportClubContext context)
        {
            this.db = context;
        }
        public async Task<IEnumerable<Training>> GetAll()
        {
            return await db.Trainings.ToListAsync();
        }
        public async Task<Training> Get(int id)
        {
            return await db.Trainings.FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task AddItem(Training c)
        {
            await db.AddAsync(c);
        }
        public async Task Update(Training c)
        {
            var f = await db.Trainings.FirstOrDefaultAsync(m => m.Id == c.Id);
            if (f != null)
            {
                db.Trainings.Update(c);

            }
        }
        public async Task Delete(int id)
        {
            var c = await db.Trainings.FindAsync(id);
            if (c != null)
            {
                db.Trainings.Remove(c);

            }
        }
    }
}
