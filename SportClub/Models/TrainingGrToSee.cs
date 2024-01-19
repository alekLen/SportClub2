using SportClub.BLL.DTO;

namespace SportClub.Models
{
    public class TrainingGrToSee
    {
        public String Day { get; set; }
        public String Time { get; set; }
        public CoachDTO? Coach { get; set; }
        public RoomDTO Room { get; set; }
        public GroupDTO? Group { get; set; }
        public List<UserDTO>? Users { get; set; } = new List<UserDTO>();

    }
}
