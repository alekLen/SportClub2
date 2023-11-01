using Microsoft.EntityFrameworkCore;
using SportClub.DAL.Interfaces;
using SportClub.DAL.EF;
using SportClub.DAL.Entities;

namespace SportClub.DAL.Repositories
{
    public class TimeTRepository : ITimeTRepository
    {
        private SportClubContext db;

        public TimeTRepository(SportClubContext context)
        {
            this.db = context;
        }
        public async Task<IEnumerable<TimeT>> GetAll()
        {
            return await db.Times.ToListAsync();
        }
        public async Task<TimeT> Get(int id)
        {
            return await db.Times.FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<TimeT> Find(string s,string e)
        {
            return await db.Times.FirstOrDefaultAsync(m => m.StartTime == s && m.EndTime==e);
        }
        public async Task AddItem(TimeT c)
        {
            await db.AddAsync(c);
        }
        public async Task Update(TimeT c)
        {
            var f = await db.Times.FirstOrDefaultAsync(m => m.Id == c.Id);
            if (f != null)
            {
                db.Times.Update(c);

            }
        }
        public async Task Delete(int id)
        {
            var c = await db.Times.FindAsync(id);
            if (c != null)
            {
                db.Times.Remove(c);

            }
        }
        public async Task DeleteAll()
        {
            var allEntities = await db.Times.ToListAsync();
            // Удалите все записи из DbSet
            db.Times.RemoveRange(allEntities);           
        }
    }
}
