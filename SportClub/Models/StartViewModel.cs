using SportClub.BLL.DTO;

namespace SportClub.Models
{
    public class StartViewModel
    {
        public List<CoachDTO> coaches { get; set; }=new List<CoachDTO>();
        public List<RoomDTO> rooms { get; set; } = new List<RoomDTO>();
    }
}
