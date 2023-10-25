using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.DAL.Entities
{
    public class Salt
    {
        public int Id { get; set; }
        public string? salt { get; set; }
        public User? user { get; set; }
        public Admin? admin { get; set; }
        public Coach? coach { get; set; }
    }
}
