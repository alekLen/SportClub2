using SportClub.BLL.DTO;

namespace SportClub.Models
{
    public class TimeTimetableModel
    {
        public TimeTDTO Time { get; set; }
        public TimetableDTO Timetable { get; set; }
        public List<TimetableShow>? times { get; set; } = new();
    }
}
