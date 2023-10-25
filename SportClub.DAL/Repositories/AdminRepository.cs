using SportClub.DAL.EF;
using SportClub.DAL.Entities;
using SportClub.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.DAL.Repositories
{
    public class AdminRepository : ISetGetRepository<Admin>
    {
        private SportClubContext db;

        public AdminRepository(SportClubContext context)
        {
            this.db = context;
        }
        public async Task<IEnumerable<Admin>> GetAll()
        {
            return await db.Admins.ToListAsync();
        }
        public async Task<Admin> Get(int id)
        {
            return await db.Admins.FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task AddItem(Admin c)
        {
            await db.AddAsync(c);
        }
        public async Task Update(Admin c)
        {
            var f = await db.Coaches.FirstOrDefaultAsync(m => m.Id == c.Id);
            if (f != null)
            {
                db.Admins.Update(c);

            }
        }
        public async Task Delete(int id)
        {
            var c = await db.Admins.FindAsync(id);
            if (c != null)
            {
                db.Admins.Remove(c);

            }
        }
    }
}
