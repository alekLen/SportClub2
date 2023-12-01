using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using SportClub.BLL.DTO;

namespace SportClub.Models
{
    public class MakeSheduleView
    {
      //  public RoomDTO room { get; set; }
      // public TimetableDTO newT { get; set; }
       // public List<TimetableDTO> timetables { get; set; } = new();
        public List<TimetableShow> times { get; set; } = new();
        public List<TimetableShow> timesAdded { get; set; } = new();
        public RoomDTO room { get; set; }
        public string? message;
        //   public IEnumerable<TimeTDTO> mn { get; set; }
        //  public IEnumerable<TimeTDTO> tun { get; set; } 
        //  public TimetableDTO we { get; set; }
        // public TimetableDTO th { get; set; }
        //  public TimetableDTO fr { get; set; }
        //  public TimetableDTO sa { get; set; }
        // public TimetableDTO su { get; set; }
    }
}
