using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using SportClub.BLL.DTO;

namespace SportClub.Models
{
    public class MakeSheduleView
    {
        public RoomDTO room { get; set; }
        public IEnumerable<TimetableDTO> timetables { get; set; }
        TimetableDTO mn;
        TimetableDTO tun;
        TimetableDTO we;
        TimetableDTO th;
        TimetableDTO fr;
        TimetableDTO sa;
        TimetableDTO su;
    }
}
