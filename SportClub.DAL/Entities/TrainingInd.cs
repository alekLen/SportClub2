using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.DAL.Entities
{
    public class TrainingInd
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Time { get; set; }
       // public Timetable Timetable { get; set; }
        public int Day {  get; set; }
        public Room Room { get; set; }
        public Coach Coach { get; set; } 
        public User? User { get; set; }
      //  public  Speciality Speciality { get; set; }
        //  public TypeOfTraining typeOfTraining { get; set; }
      //  public bool available { get; set; } = true;
    }
}
