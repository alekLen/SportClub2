using Microsoft.EntityFrameworkCore;
using SportClub.DAL.Interfaces;
using SportClub.DAL.EF;
using SportClub.DAL.Entities;

namespace SportClub.DAL.Repositories
{
    public class SaltRepository : ISaltRepository
    {
        private SportClubContext db;

        public SaltRepository(SportClubContext context)
        {
            this.db = context;
        }
        public async Task<IEnumerable<Salt>> GetAll()
        {
            return await db.Salts.ToListAsync();
        }
        public async Task<Salt> Get(int id)
        {
            return await db.Salts.FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task AddItem(Salt c)
        {
            await db.AddAsync(c);
        }
        public async Task Update(Salt c)
        {
            var f = await db.Salts.FirstOrDefaultAsync(m => m.Id == c.Id);
            if (f != null)
            {
                db.Salts.Update(c);

            }
        }
        public async Task Delete(int id)
        {
            var c = await db.Salts.FindAsync(id);
            if (c != null)
            {
                db.Salts.Remove(c);

            }
        }
        public async Task<Salt> GetAdminSalt(Admin a)
        {           
                return await db.Salts.FirstOrDefaultAsync(m => m.admin == a);           
        }
        public async Task<Salt> GetCoachSalt(Coach a)
        {
            return await db.Salts.FirstOrDefaultAsync(m => m.coach == a);
        }
        public async Task<Salt> GetUserSalt(User a)
        {
            return await db.Salts.FirstOrDefaultAsync(m => m.user == a);
        }
    }
}
