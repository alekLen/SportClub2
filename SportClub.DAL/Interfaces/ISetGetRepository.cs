using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.DAL.Interfaces
{
    public interface ISetGetRepository<T> where T : class
    {
        Task<T> Get(int id);
        Task AddItem(T s);
        Task<IEnumerable<T>> GetAll();
        Task  Delete(int id);
        Task Update(T s);
    }
}
