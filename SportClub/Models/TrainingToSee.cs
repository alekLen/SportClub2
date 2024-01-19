using SportClub.BLL.DTO;
using SportClub.DAL.Entities;

namespace SportClub.Models
{
    public class TrainingToSee
    {
        public String Day {  get; set; }
        public String Time { get; set; }
        public CoachDTO? Coach { get; set; }
        public RoomDTO Room { get; set; }
        public String? User { get; set; }
        public GroupDTO? Group { get; set; }
        public List<UserDTO>? Users { get; set; }=new List<UserDTO>();
    }
}
