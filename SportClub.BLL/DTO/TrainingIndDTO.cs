using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub.BLL.DTO
{
    public class TrainingIndDTO
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
       // public int TimeId { get; set; }
       public string Time {  get; set; }
        public int Day { get; set; }
        public string DayName { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public string? CoachName { get; set; }
        public int? CoachId { get; set; }
        public string? UserName { get; set; }
        public int? UserId { get; set; }
        public string? SpecialityName { get; set; }
        public int? SpecialityId { get; set; }
        //  public int typeOfTrainingId { get; set; }
    }
}
