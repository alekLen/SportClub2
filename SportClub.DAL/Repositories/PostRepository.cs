using Microsoft.EntityFrameworkCore;
using SportClub.DAL.Interfaces;
using SportClub.DAL.EF;
using SportClub.DAL.Entities;

namespace SportClub.DAL.Repositories
{
    public class PostRepository : ISetGetRepository<Post>
    {
        private SportClubContext db;

        public PostRepository(SportClubContext context)
        {
            this.db = context;
        }
        public async Task<IEnumerable<Post>> GetAll()
        {
            return await db.Posts.ToListAsync();
        }
        public async Task<Post> Get(int id)
        {
            return await db.Posts.FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task AddItem(Post p)
        {
            await db.AddAsync(p);
        }
        public async Task Update(Post p)
        {
            var f = await db.Posts.FirstOrDefaultAsync(m => m.Id == p.Id);
            if (f != null)
            {
                db.Posts.Update(p);

            }
        }
        public async Task Delete(int id)
        {
            var p = await db.Posts.FindAsync(id);
            if (p != null)
            {
                db.Posts.Remove(p);

            }
        }
    }
}
