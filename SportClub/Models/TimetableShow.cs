using SportClub.BLL.DTO;
namespace SportClub.Models
{
    public class TimetableShow
    {
        public int Id { get; set; }
        public List<string> Times { get; set; } = new();
       public List<TimeTDTO> T { get; set; } = new();
    }
}
