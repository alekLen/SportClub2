using Microsoft.EntityFrameworkCore;
using SportClub.DAL.Interfaces;
using SportClub.DAL.EF;
using SportClub.DAL.Entities;

namespace SportClub.DAL.Repositories
{
    public class TypeOfTrainingRepository : ISetGetRepository<TypeOfTraining>
    {
        private SportClubContext db;

        public TypeOfTrainingRepository(SportClubContext context)
        {
            this.db = context;
        }
        public async Task<IEnumerable<TypeOfTraining>> GetAll()
        {
            return await db.TypeOfTrainings.ToListAsync();
        }
        public async Task<TypeOfTraining> Get(int id)
        {
            return await db.TypeOfTrainings.FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task AddItem(TypeOfTraining c)
        {
            await db.AddAsync(c);
        }
        public async Task Update(TypeOfTraining c)
        {
            var f = await db.TypeOfTrainings.FirstOrDefaultAsync(m => m.Id == c.Id);
            if (f != null)
            {
                db.TypeOfTrainings.Update(c);

            }
        }
        public async Task Delete(int id)
        {
            var c = await db.TypeOfTrainings.FindAsync(id);
            if (c != null)
            {
                db.TypeOfTrainings.Remove(c);

            }
        }
    }
}
