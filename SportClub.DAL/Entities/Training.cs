using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.DAL.Entities
{
    public class Training
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeT Time { get; set; }
        public Room Room { get; set; }
        public TypeOfTraining typeOfTraining { get; set; }
    }
}
