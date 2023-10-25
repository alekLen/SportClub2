using Microsoft.EntityFrameworkCore;
using SportClub.DAL.Interfaces;
using SportClub.DAL.EF;
using SportClub.DAL.Entities;

namespace SportClub.DAL.Repositories
{
    public class TimetableRepository : ISetGetRepository<Timetable>
    {
        private SportClubContext db;

        public TimetableRepository(SportClubContext context)
        {
            this.db = context;
        }
        public async Task<IEnumerable<Timetable>> GetAll()
        {
            return await db.Timetables.ToListAsync();
        }
        public async Task<Timetable> Get(int id)
        {
            return await db.Timetables.FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task AddItem(Timetable c)
        {
            await db.AddAsync(c);
        }
        public async Task Update(Timetable c)
        {
            var f = await db.Timetables.FirstOrDefaultAsync(m => m.Id == c.Id);
            if (f != null)
            {
                db.Timetables.Update(c);

            }
        }
        public async Task Delete(int id)
        {
            var c = await db.Timetables.FindAsync(id);
            if (c != null)
            {
                db.Timetables.Remove(c);

            }
        }
    }
}
