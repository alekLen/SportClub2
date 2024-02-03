using SportClub.BLL.DTO;
using SportClub.DAL.Entities;

namespace SportClub.Models
{
    public class TrainingIndToSee
    {
        public int? Id { get; set; }
        public String DayName {  get; set; }
        public int? Day { get; set; }
        public String Time { get; set; }
        public CoachDTO? Coach { get; set; }
        public RoomDTO Room { get; set; }
        public String? User { get; set; }      
    }
}
