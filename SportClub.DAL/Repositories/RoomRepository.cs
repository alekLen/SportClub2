using Microsoft.EntityFrameworkCore;
using SportClub.DAL.Interfaces;
using SportClub.DAL.EF;
using SportClub.DAL.Entities;

namespace SportClub.DAL.Repositories
{
    public class RoomRepository : ISetGetRepository<Room>
    {
        private SportClubContext db;

        public RoomRepository(SportClubContext context)
        {
            this.db = context;
        }
        public async Task<IEnumerable<Room>> GetAll()
        {
            return await db.Rooms.Include(r => r.Shedule).ToListAsync();
        }
        public async Task<Room> Get(int id)
        {
            return await db.Rooms.Include(r=>r.Shedule).FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task AddItem(Room c)
        {
            await db.AddAsync(c);
        }
        public async Task Update(Room c)
        {
            var f = await db.Rooms.FirstOrDefaultAsync(m => m.Id == c.Id);
            if (f != null)
            {
                db.Rooms.Update(c);

            }
        }
        public async Task Delete(int id)
        {
            var c = await db.Rooms.FindAsync(id);
            if (c != null)
            {
                db.Rooms.Remove(c);

            }
        }
    }
}
