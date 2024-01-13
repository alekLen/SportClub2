using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.DAL.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Number { get; set; }//

        //public int CoachId;
        public Coach? Coach { get; set; }
      // public virtual List<TrainingGroup> Trainings { get; set; }
        public virtual List<User> users { get; set; }=new List<User>();
    }
}
