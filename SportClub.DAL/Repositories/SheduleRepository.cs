using Microsoft.EntityFrameworkCore;
using SportClub.DAL.Interfaces;
using SportClub.DAL.EF;
using SportClub.DAL.Entities;

namespace SportClub.DAL.Repositories
{
    public class SheduleRepository : ISetGetRepository<Shedule>
    {
        private SportClubContext db;

        public SheduleRepository(SportClubContext context)
        {
            this.db = context;
        }
        public async Task<IEnumerable<Shedule>> GetAll()
        {
            return await db.Shedules.Include(p => p.Monday).Include(p => p.Tuesday).Include(p => p.Wednesday).Include(p => p.Thursday).Include(p => p.Friday).Include(p => p.Saturday).Include(p => p.Sunday).ToListAsync();
        }
        public async Task<Shedule> Get(int id)
        {
            return await db.Shedules.Include(p=>p.Monday).Include(p => p.Tuesday).Include(p => p.Wednesday).Include(p => p.Thursday).Include(p => p.Friday).Include(p => p.Saturday).Include(p => p.Sunday).FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task AddItem(Shedule c)
        {
            await db.AddAsync(c);
        }
        public async Task Update(Shedule c)
        {
            var f = await db.Shedules.FirstOrDefaultAsync(m => m.Id == c.Id);
            if (f != null)
            {
                db.Shedules.Update(c);

            }
        }
        public async Task Delete(int id)
        {
            var c = await db.Shedules.FindAsync(id);
            if (c != null)
            {
                db.Shedules.Remove(c);

            }
        }
    }
}
