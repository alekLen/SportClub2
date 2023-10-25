using Microsoft.EntityFrameworkCore;
using SportClub.DAL.Interfaces;
using SportClub.DAL.EF;
using SportClub.DAL.Entities;


namespace SportClub.DAL.Repositories
{
    public class GroupRepository : ISetGetRepository<Group>
    {
        private SportClubContext db;

        public GroupRepository(SportClubContext context)
        {
            this.db = context;
        }
        public async Task<IEnumerable<Group>> GetAll()
        {
            return await db.Groups.ToListAsync();
        }
        public async Task<Group> Get(int id)
        {
            return await db.Groups.FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task AddItem(Group c)
        {
            await db.AddAsync(c);
        }
        public async Task Update(Group c)
        {
            var f = await db.Groups.FirstOrDefaultAsync(m => m.Id == c.Id);
            if (f != null)
            {
                db.Groups.Update(c);

            }
        }
        public async Task Delete(int id)
        {
            var c = await db.Coaches.FindAsync(id);
            if (c != null)
            {
                db.Coaches.Remove(c);

            }
        }
    }
}
