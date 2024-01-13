using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using SportClub.BLL.DTO;

namespace SportClub.Models
{
    public class MakeSheduleView
    {

        public List<TimetableShow> times { get; set; } = new();
        public List<TimetableShow> timesAdded { get; set; } = new();
        public RoomDTO? room { get; set; }
        public string? message;
        public List<TrainingIndDTO>? trainingInd { get; set; } = new();
        public bool flag { get; set; } = false;
        
      // public int? shedule { get;set; }
    }
}
