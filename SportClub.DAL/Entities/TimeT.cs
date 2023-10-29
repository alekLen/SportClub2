using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.DAL.Entities
{
    public class TimeT
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public List<string> Times { get; set; }
    }
}
