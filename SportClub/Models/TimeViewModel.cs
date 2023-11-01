using SportClub.BLL.DTO;
namespace SportClub.Models
{
    public class TimeViewModel
    {
        public TimeTDTO timeTDTO { get; set; }
        public TimetableDTO timetableDTO { get; set; }  
        public TimeViewModel(TimeTDTO timeTDTO, TimetableDTO timetableDTO)
        {
            this.timeTDTO = timeTDTO;
            this.timetableDTO = timetableDTO;
        }   
    }
}
