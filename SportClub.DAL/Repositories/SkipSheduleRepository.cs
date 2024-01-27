using SportClub.DAL.EF;
using SportClub.DAL.Entities;
using SportClub.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using SportClub.DAL.Interfaces;
using SportClub.DAL.EF;
using SportClub.DAL.Entities;

namespace SportClub.DAL.Repositories
{
    public class SkipSheduleRepository : ISetGetRepository<SkipShedule>
    {
        private SportClubContext db;

        public SkipSheduleRepository(SportClubContext context)
        {
            this.db = context;
        }

        public async Task<IEnumerable<SkipShedule>> GetAll()
        {
            // return await db.Shedules.Include(p => p.Monday).Include(p => p.Tuesday).Include(p => p.Wednesday).Include(p => p.Thursday).Include(p => p.Friday).Include(p => p.Saturday).Include(p => p.Sunday).ToListAsync();
            return await db.SkipShedule.ToListAsync();
        }
        public async Task<SkipShedule> Get(int id)
        {
            // return await db.Shedules.Include(p => p.Monday).Include(p => p.Tuesday).Include(p => p.Wednesday).Include(p => p.Thursday).Include(p => p.Friday).Include(p => p.Saturday).Include(p => p.Sunday).FirstOrDefaultAsync(m => m.Id == id);
            return await db.SkipShedule.FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task AddItem(SkipShedule c)
        {
            await db.AddAsync(c);
        }
        public async Task Update(SkipShedule c)
        {
            var f = await db.SkipShedule.FirstOrDefaultAsync(m => m.Id == c.Id);
            if (f != null)
            {
                db.SkipShedule.Update(c);

            }
        }
        public async Task Delete(int id)
        {
            var c = await db.SkipShedule.FindAsync(id);
            if (c != null)
            {
                db.SkipShedule.Remove(c);

            }
        }
    }
}
