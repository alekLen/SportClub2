using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;


namespace SportClub.BLL.DTO
{
    public class TimeTDTO
    {
        public int Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
