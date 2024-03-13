using SportClub.BLL.DTO;

namespace SportClub.Models
{
    public class StartViewModel
    {
        public List<CoachDTO> coaches { get; set; }=new List<CoachDTO>();
        public int CurrentPageCoaches { get; set; }
        public int TotalPagesCoaches { get; set; }
        public List<RoomDTO> rooms { get; set; } = new List<RoomDTO>();
        public int CurrentPageRooms { get; set; }
        public int TotalPagesRooms { get; set; }
    }
}
