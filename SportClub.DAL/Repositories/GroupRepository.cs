using Microsoft.EntityFrameworkCore;
using SportClub.DAL.Interfaces;
using SportClub.DAL.EF;
using SportClub.DAL.Entities;


namespace SportClub.DAL.Repositories
{
    public class GroupRepository :IGroupRepository
    {
        private SportClubContext db;

        public GroupRepository(SportClubContext context)
        {
            this.db = context;
        }
        public async Task<IEnumerable<Group>> GetAll()
        {
            return await db.Groups.Include((p)=> p.Coach).Include((p) => p.users).ToListAsync();
        }
        
        public async Task<Group> Get(int id)
        {
            return await db.Groups.Include((p) => p.Coach).Include((p) => p.users).FirstOrDefaultAsync(m => m.Id == id);
        }
        //public async Task<Group> GetGroupWithUsers(int groupId)
        //{
        //    return _context.Groups
        //        .Include(g => g.Users)
        //        .FirstOrDefault(g => g.GroupId == groupId);
        //}
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
            var c = await db.Groups.FindAsync(id);
            if (c != null)
            {
                db.Groups.Remove(c);

            }
        }

        public async Task<Group> GetGroupName(string name)
        {
            return await db.Groups.FirstOrDefaultAsync(m => m.Name == name);
        }

    }
}
