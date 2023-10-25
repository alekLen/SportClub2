using Microsoft.EntityFrameworkCore;
using SportClub.DAL.Interfaces;
using SportClub.DAL.EF;
using SportClub.DAL.Entities;

namespace SportClub.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private SportClubContext db;

        public UserRepository(SportClubContext context)
        {
            this.db = context;
        }
        public async Task<IEnumerable<User>> GetAll()
        {
            return await db.Users.ToListAsync();
        }
        public async Task<User> Get(int id)
        {
            return await db.Users.FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task AddItem(User c)
        {
            await db.AddAsync(c);
        }
        public async Task Update(User c)
        {
            var f = await db.Users.FirstOrDefaultAsync(m => m.Id == c.Id);
            if (f != null)
            {
                db.Users.Update(c);

            }
        }
        public async Task Delete(int id)
        {
            var c = await db.Users.FindAsync(id);
            if (c != null)
            {
                db.Users.Remove(c);

            }
        }
        public async Task<User> GetUserLogin(string login)
        {
            return await db.Users.FirstOrDefaultAsync(m => m.Login == login);
        }
    }
}
