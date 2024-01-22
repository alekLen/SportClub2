using SportClub.BLL.DTO;

namespace SportClub.Models
{
    public class TrainingGrToSee
    {
        public int? Id { get; set; }
        public String DayName { get; set; }
        public int? Day { get; set; }
        public String Time { get; set; }
        public CoachDTO? Coach { get; set; }
        public RoomDTO Room { get; set; }
        public GroupDTO? Group { get; set; }
        public List<UserDTO>? Users { get; set; } = new List<UserDTO>();

    }
}
