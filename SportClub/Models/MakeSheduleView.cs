using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using SportClub.BLL.DTO;

namespace SportClub.Models
{
    public class MakeSheduleView
    {
      //  public RoomDTO room { get; set; }
       public TimetableDTO newT { get; set; }
        public IEnumerable<TimetableDTO> timetables { get; set; }
        public TimetableDTO mn { get; set; }
        public TimetableDTO tun { get; set; }
        public TimetableDTO we { get; set; }
        public TimetableDTO th { get; set; }
        public TimetableDTO fr { get; set; }
        public TimetableDTO sa { get; set; }
        public TimetableDTO su { get; set; }
    }
}
